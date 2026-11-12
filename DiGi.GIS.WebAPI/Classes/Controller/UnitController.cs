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
using System.Globalization;
using System.Linq;
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
        private readonly StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitController"/> class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher for GIS Web API settings.</param>
        /// <param name="unitPostgreSQLConverter">The converter used for handling territorial unit operations.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used for administrative area operations.</param>
        /// <param name="statisticalDataCollectionPostgreSQLConverter">The converter used to read the stored BDL variable values of a statistical unit.</param>
        public UnitController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, UnitPostgreSQLConverter unitPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, StatisticalDataCollectionPostgreSQLConverter statisticalDataCollectionPostgreSQLConverter)
        {
            this.statisticalDataCollectionPostgreSQLConverter = statisticalDataCollectionPostgreSQLConverter;
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
            Serilog.Modify.Log("Level provided: {Level}", level?.ToString(CultureInfo.CurrentCulture) ?? "all");

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

            (StatisticalUnit? matched, IActionResult? actionResult) = await MatchAsync(code, administrativeArealType, id, cancellationToken);
            if (actionResult is not null)
            {
                return actionResult;
            }

            string? json = Core.Convert.ToSystem_String(matched);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the stored BDL variable values (<see cref="StatisticalDataCollection"/>) of the statistical unit matching an administrative area, optionally only the named variables.
        /// <para>The unit is resolved exactly as by <c>match</c>. Each variable carries its yearly values. BDL answers 0 for years it has not published (the population series for 2024 and 2025, paper 01 section 3.5), so a 0 must be read as a possible gap, not as a measurement; this endpoint returns the stored values unchanged.</para>
        /// <para>A gmina of the urban-rural type is one BDL unit for the whole gmina plus two partial units; the match resolves the whole-gmina unit, which is the one paper 01 compares with BDOT10k aggregates.</para>
        /// </summary>
        /// <param name="code">The unique administrative code. Either (code and administrativearealtype) or id must be supplied.</param>
        /// <param name="administrativeArealType">The administrative area type. Either (code and administrativearealtype) or id must be supplied.</param>
        /// <param name="id">The integer identifier of the administrative area in PostgreSQL. Either (code and administrativearealtype) or id must be supplied.</param>
        /// <param name="names">The variables to return, repeated once per variable; each value matches a variable's stored name (<c>population [person/km2]</c>) or its BDL variable identifier (<c>60559</c>), exactly. Omit to return every stored variable.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The collection as JSON (with only the named variables found, possibly none), 404 when no unit matches or no values are stored for it, 400 for invalid input, or an error status.</returns>
        [HttpGet("statisticaldatacollection", Name = $"{nameof(UnitController)}_{nameof(GetStatisticalDataCollectionAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(StatisticalDataCollection), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatisticalDataCollectionAsync([FromQuery(Name = "code")] string? code = null, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType = null, [FromQuery(Name = "id")] int? id = null, [FromQuery(Name = "names")] List<string>? names = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {NameCount} names", nameof(UnitController), nameof(GetStatisticalDataCollectionAsync), names?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (statisticalDataCollectionPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "StatisticalDataCollectionPostgreSQLConverter cannot be null");
                return BadRequest();
            }

            StatisticalDataCollection? statisticalDataCollection;
            try
            {
                (StatisticalUnit? matched, IActionResult? actionResult) = await MatchAsync(code, administrativeArealType, id, cancellationToken);
                if (actionResult is not null)
                {
                    return actionResult;
                }

                statisticalDataCollection = await statisticalDataCollectionPostgreSQLConverter.GetStatisticalDataCollectionAsync(matched, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(UnitController), nameof(GetStatisticalDataCollectionAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (statisticalDataCollection is null)
            {
                Serilog.Modify.Log("No statistical data is stored for the matched unit");
                return NotFound();
            }

            statisticalDataCollection = statisticalDataCollection.StatisticalDataCollectionByNames(names);

            string? json = Core.Convert.ToSystem_String(statisticalDataCollection);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
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

        /// <summary>
        /// Resolves the statistical unit matching an administrative area given by its code and type, or by its integer identifier - the resolution shared by <c>match</c> and <c>statisticaldatacollection</c>.
        /// </summary>
        /// <param name="code">The unique administrative code, or null when <paramref name="id"/> is supplied.</param>
        /// <param name="administrativeArealType">The administrative area type, or null when <paramref name="id"/> is supplied.</param>
        /// <param name="id">The integer identifier of the administrative area, or null when the code and type are supplied.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The matched unit with a null action result, or a null unit with the status to answer (400 for invalid input or a missing converter, 404 when nothing matches).</returns>
        private async Task<(StatisticalUnit? StatisticalUnit, IActionResult? ActionResult)> MatchAsync(string? code, AdministrativeArealType? administrativeArealType, int? id, CancellationToken cancellationToken)
        {
            if (!id.HasValue && (string.IsNullOrWhiteSpace(code) || !administrativeArealType.HasValue || administrativeArealType.Value == AdministrativeArealType.Undefined))
            {
                return (null, BadRequest());
            }

            if (id.HasValue && id.Value <= 0)
            {
                return (null, BadRequest());
            }

            if (unitPostgreSQLConverter is null)
            {
                return (null, BadRequest());
            }

            StatisticalUnit? root = await unitPostgreSQLConverter.GetStatisticalUnitAsync(cancellationToken: cancellationToken);
            if (root is null)
            {
                return (null, NotFound());
            }

            StatisticalUnit? matched = null;

            if (id.HasValue && id.Value > 0)
            {
                if (administrativeAreal2DPostgreSQLConverter is null)
                {
                    return (null, BadRequest());
                }

                AdministrativeAreal2DReference? reference = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByIdAsync(id.Value, cancellationToken: cancellationToken);
                if (reference is null)
                {
                    return (null, NotFound());
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
                return (null, BadRequest());
            }

            if (matched is null)
            {
                return (null, NotFound());
            }

            return (matched, null);
        }
    }
}
