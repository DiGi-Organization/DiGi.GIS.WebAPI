using DiGi.BDL.Classes;
using DiGi.GIS.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.WebAPI.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Controller responsible for handling API requests related to BDL territorial units, statistical units, matching, and data compliance in PostgreSQL.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class UnitController : WebAPIController
    {
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;
        private readonly UnitPostgreSQLConverter unitPostgreSQLConverter;
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitController"/> class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher for GIS Web API settings.</param>
        /// <param name="unitPostgreSQLConverter">The converter used for handling territorial unit operations.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used for administrative area operations.</param>
        public UnitController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, UnitPostgreSQLConverter unitPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.unitPostgreSQLConverter = unitPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <summary>
        /// Asynchronously updates or inserts a collection of territorial units.
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the territorial units to update or insert.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of updated or inserted unit identifiers.</returns>
        [HttpPost("updateitems", Name = $"{nameof(UnitController)}_{nameof(UpdateItemsAsync)}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsAsync([FromBody] JsonArray? jsonArray, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(UpdateItemsAsync));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Unit update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateUnit)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Unit update not allowed");
                return Unauthorized();
            }

            if (jsonArray is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Units cannot be null");
                return BadRequest();
            }

            List<Unit>? units = System.Text.Json.JsonSerializer.Deserialize<List<Unit>>(jsonArray);
            if (units is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Units could not be deserialized");
                return BadRequest();
            }

            int count_Before = units.Count;
            if (count_Before == 0)
            {
                Serilog.Modify.Log("No Units to update");
                return NoContent();
            }

            if (unitPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<string> ids = await unitPostgreSQLConverter.InsertAsync(units, cancellationToken: cancellationToken);
                if (ids.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no Units have been updated");
                    return StatusCode(500, "Database update returned no modified Unit IDs.");
                }

                Serilog.Modify.Log("Updating to database ended. Updated Units: {After}/{Before}", ids.Count, count_Before);
                return Ok(ids);
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Database could not be updated");
                return StatusCode(500, "Internal server error during database update");
            }
        }

        /// <summary>
        /// Asynchronously retrieves a territorial unit by its unique 12-digit identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the unit.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The territorial unit if found; otherwise, 404 Not Found.</returns>
        [HttpGet("item", Name = $"{nameof(UnitController)}_{nameof(GetItemByIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemByIdAsync([BindRequired, FromQuery(Name = "id")] string? id, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(GetItemByIdAsync));
            Serilog.Modify.Log("Id provided: {Id}", id ?? string.Empty);

            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            if (unitPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            Unit? unit = await unitPostgreSQLConverter.GetUnitByIdAsync(id, cancellationToken: cancellationToken);
            if (unit is null)
            {
                return NotFound();
            }

            string? json = System.Text.Json.JsonSerializer.Serialize(unit);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves territorial units, optionally filtered by level.
        /// </summary>
        /// <param name="level">The optional level filter (0=country, 1=macroregion, 2=voivodeship, etc.).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection of territorial units.</returns>
        [HttpGet("items", Name = $"{nameof(UnitController)}_{nameof(GetItemsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsAsync([FromQuery(Name = "level")] short? level = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(GetItemsAsync));
            Serilog.Modify.Log("Level provided: {Level}", level?.ToString() ?? "all");

            if (unitPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<Unit>? units = await unitPostgreSQLConverter.GetUnitsAsync(level, cancellationToken: cancellationToken);
            if (units is null || units.Count == 0)
            {
                return NotFound();
            }

            string? json = System.Text.Json.JsonSerializer.Serialize(units);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the distribution of territorial units grouped by level.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A dictionary mapping hierarchy level to unit count.</returns>
        [HttpGet("counts", Name = $"{nameof(UnitController)}_{nameof(GetCountsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(Dictionary<short, int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountsAsync(CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(GetCountsAsync));

            if (unitPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            Dictionary<short, int>? counts = await unitPostgreSQLConverter.GetCountsByLevelAsync(cancellationToken: cancellationToken);
            if (counts is null || counts.Count == 0)
            {
                return NotFound();
            }

            return Ok(counts);
        }

        /// <summary>
        /// Asynchronously constructs and retrieves the root <see cref="StatisticalUnit"/> hierarchy from all stored units.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The root statistical unit tree.</returns>
        [HttpGet("statisticalunit", Name = $"{nameof(UnitController)}_{nameof(GetStatisticalUnitAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(StatisticalUnit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStatisticalUnitAsync(CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(GetStatisticalUnitAsync));

            if (unitPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            StatisticalUnit? statisticalUnit = await unitPostgreSQLConverter.GetStatisticalUnitAsync(cancellationToken: cancellationToken);
            if (statisticalUnit is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(statisticalUnit);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously finds the matching <see cref="StatisticalUnit"/> for an administrative area by its code and type, or by its integer identifier.
        /// </summary>
        /// <param name="code">The unique administrative code. Either (code and administrativearealtype) or id must be supplied.</param>
        /// <param name="administrativeArealType">The administrative area type. Either (code and administrativearealtype) or id must be supplied.</param>
        /// <param name="id">The integer identifier of the administrative area in PostgreSQL. Either (code and administrativearealtype) or id must be supplied.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The matched statistical unit if found; otherwise, 404 Not Found.</returns>
        [HttpGet("match", Name = $"{nameof(UnitController)}_{nameof(GetMatchAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(StatisticalUnit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMatchAsync([FromQuery(Name = "code")] string? code = null, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType = null, [FromQuery(Name = "id")] int? id = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(GetMatchAsync));

            if (!id.HasValue && (string.IsNullOrWhiteSpace(code) || !administrativeArealType.HasValue || administrativeArealType.Value == AdministrativeArealType.Undefined))
            {
                return BadRequest();
            }

            if (id.HasValue && id.Value <= 0)
            {
                return BadRequest();
            }

            if (unitPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            StatisticalUnit? root = await unitPostgreSQLConverter.GetStatisticalUnitAsync(cancellationToken: cancellationToken);
            if (root is null)
            {
                return NotFound();
            }

            StatisticalUnit? matched = null;

            if (id.HasValue && id.Value > 0)
            {
                if (administrativeAreal2DPostgreSQLConverter is null)
                {
                    return BadRequest();
                }

                AdministrativeAreal2DReference? reference = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByIdAsync(id.Value, cancellationToken: cancellationToken);
                if (reference is null)
                {
                    return NotFound();
                }

                matched = GIS.PostgreSQL.Query.Match(root, reference);
            }
            else if (!string.IsNullOrWhiteSpace(code) && administrativeArealType.HasValue && administrativeArealType.Value != AdministrativeArealType.Undefined)
            {
                if (administrativeAreal2DPostgreSQLConverter is not null)
                {
                    AdministrativeAreal2DReference? reference = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByCodeAsync(code, administrativeArealType.Value, cancellationToken: cancellationToken);
                    if (reference is not null)
                    {
                        matched = GIS.PostgreSQL.Query.Match(root, reference);
                    }
                }

                if (matched is null)
                {
                    matched = GIS.PostgreSQL.Query.Match(root, null, code, administrativeArealType.Value);
                }
            }
            else
            {
                return BadRequest();
            }

            if (matched is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(matched);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously evaluates and returns data compliance of administrative areas against BDL statistical units for the specified administrative area type.
        /// </summary>
        /// <param name="administrativeArealType">The administrative area type to evaluate (e.g. County, Municipality, Voivodeship).</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The data compliance report containing totals, matched counts, compliance percentage, and unmapped entities.</returns>
        [HttpGet("compliance", Name = $"{nameof(UnitController)}_{nameof(GetComplianceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(UnitComplianceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetComplianceAsync([BindRequired, FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(UnitController), nameof(GetComplianceAsync));
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (administrativeArealType is null || administrativeArealType.Value == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (unitPostgreSQLConverter is null || administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            UnitComplianceResult? complianceResult = await unitPostgreSQLConverter.GetComplianceAsync(administrativeAreal2DPostgreSQLConverter, administrativeArealType.Value, cancellationToken: cancellationToken);
            if (complianceResult is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(complianceResult);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }
    }
}
