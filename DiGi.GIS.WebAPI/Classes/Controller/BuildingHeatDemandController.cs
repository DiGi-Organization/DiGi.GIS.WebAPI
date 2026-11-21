using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Serves the stored heat-demand results (<c>building_heat_demand</c>): the county summary of one method and input variant, and every stored result of given buildings for comparing methods and input variants (Heat Demand Method Identity, DiGi-Organization/DiGi.GIS.Analytical#19).
    /// <para>Both reads are open to every visitor, as the other per-building GIS reads are; the summary carries counts and aggregates only. The production measure of a heat-demand run (DiGi.Analytical#10): the run keeps its counts in memory, and production is read only through this API, never through a configuration file (<c>Coding - PostgreSQL.md</c> §6).</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingHeatDemandController : DiGi.WebAPI.Classes.WebAPIController
    {
        // The integers of HeatDemandMethod.Monthly and HeatDemandInputVariant.Generated, the defaults of every read:
        // the host references no energy assembly, and the integers never change.
        private const int monthly = 1;
        private const int generated = 1;

        // The most references one byreferences request reads: one batch of the read beneath.
        private const int referenceCountMax = 1000;

        private readonly BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingHeatDemandController"/> class. This is the controller's only public constructor: MVC activates it through <c>ActivatorUtilities</c>, and a second one would answer every action with 500 (<c>Coding - WebAPI Contracts.md</c> §5).
        /// </summary>
        /// <param name="buildingSolarRadiationPostgreSQLConverter">The Storage-database converter holding the <see cref="DiGi.PostgreSQL.Classes.ConnectionData"/> this controller uses to open its own connection for the heat-demand summary query.</param>
        public BuildingHeatDemandController(BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter)
        {
            this.buildingSolarRadiationPostgreSQLConverter = buildingSolarRadiationPostgreSQLConverter;
        }

        /// <summary>
        /// Summarises what each of the named county parts holds of one method and one input variant, per method version, building type and year-built band: the rows stored, the shaded and unshaded counts, the total and per-area heating energy need with its deciles, the design heat load, the quality counts (excluded components, glazing fallbacks, missing irradiation), and the first and last write.
        /// <para>The production measure of a heat-demand run (DiGi.Analytical#10): the run keeps its counts in memory, and production is read only through this API, never through a configuration file (<c>Coding - PostgreSQL.md</c> §6). Counts and aggregates only - no building data leaves. Open to every visitor, like the other <c>summariesbycountyids</c> reads.</para>
        /// <para>Naming no county part summarises every part holding results. A part holding none is absent rather than present with zeros, and a missing table answers an empty list.</para>
        /// <para>A summary never mixes methods or input variants (DiGi-Organization/DiGi.GIS.WebAPI#104): <c>method</c> and <c>inputvariant</c> select one of each, Monthly (1) and Generated (1) when omitted. Both are the integers of <c>HeatDemandMethod</c> and <c>HeatDemandInputVariant</c> - integers because the host references no energy assembly, and because integers never change while member names may. Compare methods or input variants building by building with <c>byreferences</c>.</para>
        /// </summary>
        /// <param name="countyIds">The identifiers of the county parts to summarise, repeated once per part (<c>countyids=55417&amp;countyids=53477</c>). Omit to summarise every part.</param>
        /// <param name="methodVersion">The method version to summarise; omit to summarise every version, one row each.</param>
        /// <param name="yearBuiltBounds">The ascending band bounds of the year built (e.g. the TABULA PL periods), repeated once per bound. Omit for one band of every year. Must be strictly ascending after de-duplication; a non-ascending sequence is refused with HTTP 400.</param>
        /// <param name="method">The integer of the <c>HeatDemandMethod</c> to summarise: 1 = Monthly, 0 = Undefined (results stored without a heat demand). Omit for Monthly; a negative value is refused with HTTP 400.</param>
        /// <param name="inputVariant">The integer of the <c>HeatDemandInputVariant</c> to summarise: 1 = Generated, 0 = Undefined. Omit for Generated; a negative value is refused with HTTP 400.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a JSON list of <see cref="BuildingHeatDemandCountyResult"/> (empty when nothing is stored or the table is missing), 400 for a negative timeout, method or input variant or non-ascending year-built bounds, 503 for a transient database failure, or 500.</returns>
        [HttpGet("summariesbycountyids", Name = $"{nameof(BuildingHeatDemandController)}_{nameof(GetSummariesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<BuildingHeatDemandCountyResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummariesByCountyIdsAsync(
            [FromQuery(Name = "countyids")] List<int>? countyIds,
            [FromQuery(Name = "methodversion")] int? methodVersion = null,
            [FromQuery(Name = "yearbuiltbounds")] List<short>? yearBuiltBounds = null,
            [Minimum(0), FromQuery(Name = "method")] int? method = null,
            [Minimum(0), FromQuery(Name = "inputvariant")] int? inputVariant = null,
            [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600,
            CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {CountyCount} county parts", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync), countyIds?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: commandtimeout cannot be negative", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
                return BadRequest();
            }

            if (method < 0 || inputVariant < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: method and inputvariant cannot be negative", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
                return BadRequest();
            }

            if (yearBuiltBounds is not null && yearBuiltBounds.Count > 1)
            {
                List<short> yearBuiltBounds_Deduplicated = [];
                foreach (short bound in yearBuiltBounds)
                {
                    if (!yearBuiltBounds_Deduplicated.Contains(bound))
                    {
                        yearBuiltBounds_Deduplicated.Add(bound);
                    }
                }

                for (int i = 1; i < yearBuiltBounds_Deduplicated.Count; i++)
                {
                    if (yearBuiltBounds_Deduplicated[i] <= yearBuiltBounds_Deduplicated[i - 1])
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: yearbuiltbounds not strictly ascending after de-duplication", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
                        return BadRequest();
                    }
                }
            }

            List<BuildingHeatDemandCountyResult>? buildingHeatDemandCountyResults;
            try
            {
                await using NpgsqlConnection? npgsqlConnection = DiGi.PostgreSQL.Create.NpgsqlConnection(this.buildingSolarRadiationPostgreSQLConverter.ConnectionData);
                if (npgsqlConnection is not null)
                {
                    await npgsqlConnection.OpenAsync(cancellationToken);
                }
                buildingHeatDemandCountyResults = await npgsqlConnection.BuildingHeatDemandCountyResultsAsync(countyIds is null || countyIds.Count == 0 ? null : countyIds, methodVersion, yearBuiltBounds, method: method ?? monthly, inputVariant: inputVariant ?? generated, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            // Null is a failure to reach the database, never "nothing stored" - an empty list says that (Coding - WebAPI Contracts.md).
            if (buildingHeatDemandCountyResults is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name} failed: no connection to the Storage database", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
                return StatusCode(500, "Heat demand summaries could not be read");
            }

            if (buildingHeatDemandCountyResults.Count == 0)
            {
                return Content("[]", "application/json");
            }

            string? json = Core.Convert.ToSystem_String(buildingHeatDemandCountyResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name}: {Count} summaries could not be serialized", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync), buildingHeatDemandCountyResults.Count);
                return StatusCode(500, "Heat demand summaries could not be serialized");
            }

            Serilog.Modify.Log("{Type}:{Name} returns {Count} summaries", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync), buildingHeatDemandCountyResults.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Returns every stored heat demand of each of the named buildings - every method, method version and input variant - for comparing them building by building (DiGi-Organization/DiGi.GIS.WebAPI#104, Heat Demand Method Identity DiGi-Organization/DiGi.GIS.Analytical#19).
        /// <para>The comparison route: the only read that mixes methods and input variants, and it is not filtered. Each element is a stored <c>BuildingHeatDemandResult</c> as JSON, labelled by the key it is stored under - <c>HeatDemandResult.HeatDemandMethod</c>, <c>HeatDemandResult.MethodVersion</c> and <c>HeatDemandInputVariant</c>, as integers - ordered by reference, method, method version and input variant. The engine is not needed to serve it (<c>Query.BuildingHeatDemandObjectsAsync</c>).</para>
        /// <para>The read is scoped to <c>countyid</c>; with <c>fallbackbyreference</c>, a building with no result there is looked for under every county part, since a building of a multi-part county may be filed under a sibling part. A building without a stored result is absent; a missing table answers an empty list.</para>
        /// </summary>
        /// <param name="countyId">The id of the county part the buildings are filed under - a part id, never a county code. Required: omitting it is refused with HTTP 400.</param>
        /// <param name="references">The <c>building_2d</c> references of the buildings, repeated once per building (<c>references=A&amp;references=B</c>). At least one and at most 1000 distinct non-blank references; otherwise HTTP 400.</param>
        /// <param name="fallbackByReference">True to look for a building with no result under <c>countyid</c> under every county part.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a JSON list of the stored results (empty when none is stored or the table is missing), 400 for a missing county part, no or too many references or a negative timeout, 503 for a transient database failure, or 500.</returns>
        [HttpGet("byreferences", Name = $"{nameof(BuildingHeatDemandController)}_{nameof(GetByReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByReferencesAsync([FromQuery(Name = "countyid")] int? countyId, [FromQuery(Name = "references")] List<string>? references, [FromQuery(Name = "fallbackbyreference")] bool fallbackByReference = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            HashSet<string> references_Unique = [];
            foreach (string? reference in references ?? [])
            {
                if (!string.IsNullOrWhiteSpace(reference))
                {
                    references_Unique.Add(reference!.Trim());
                }
            }

            Serilog.Modify.Log("{Type}:{Name} started for {ReferenceCount} references in county part {CountyId}", nameof(BuildingHeatDemandController), nameof(GetByReferencesAsync), references_Unique.Count, countyId?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "none");

            if (countyId is null || references_Unique.Count == 0 || references_Unique.Count > referenceCountMax || commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: countyid is required, references must hold 1 to {ReferenceCountMax} distinct values and commandtimeout cannot be negative", nameof(BuildingHeatDemandController), nameof(GetByReferencesAsync), referenceCountMax);
                return BadRequest();
            }

            List<JsonObject> jsonObjects;
            try
            {
                await using NpgsqlConnection? npgsqlConnection = DiGi.PostgreSQL.Create.NpgsqlConnection(this.buildingSolarRadiationPostgreSQLConverter.ConnectionData);
                if (npgsqlConnection is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name} failed: no connection to the Storage database", nameof(BuildingHeatDemandController), nameof(GetByReferencesAsync));
                    return StatusCode(500, "Heat demand results could not be read");
                }

                await npgsqlConnection.OpenAsync(cancellationToken);

                // With a connection, null is a missing table: nothing stored (Coding - PostgreSQL.md §4).
                jsonObjects = await npgsqlConnection.BuildingHeatDemandObjectsAsync(references_Unique, countyId.Value, fallbackByReference, commandTimeout, cancellationToken) ?? [];
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingHeatDemandController), nameof(GetByReferencesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingHeatDemandController), nameof(GetByReferencesAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            // The stored objects are the DiGi serialization of the results, so the list is returned as they are.
            JsonArray jsonArray = [];
            foreach (JsonObject jsonObject in jsonObjects)
            {
                jsonArray.Add(jsonObject);
            }

            Serilog.Modify.Log("{Type}:{Name} returns {Count} results", nameof(BuildingHeatDemandController), nameof(GetByReferencesAsync), jsonObjects.Count);
            return Content(jsonArray.ToJsonString(), "application/json");
        }
    }
}
