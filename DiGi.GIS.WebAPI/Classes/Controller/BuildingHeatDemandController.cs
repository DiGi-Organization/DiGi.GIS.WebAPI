using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Serves the summary of the stored heat-demand results (<c>building_heat_demand</c>).
    /// <para>A read open to every visitor: counts and aggregates only, no building data leaves. The production measure of a heat-demand run (DiGi.Analytical#10): the run keeps its counts in memory, and production is read only through this API, never through a configuration file (<c>Coding - PostgreSQL.md</c> §6).</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingHeatDemandController : DiGi.WebAPI.Classes.WebAPIController
    {
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
        /// Summarises what each of the named county parts holds per method version, building type and year-built band: the rows stored, the shaded and unshaded counts, the total and per-area heating energy need with its deciles, the design heat load, the quality counts (excluded components, glazing fallbacks, missing irradiation), and the first and last write.
        /// <para>The production measure of a heat-demand run (DiGi.Analytical#10): the run keeps its counts in memory, and production is read only through this API, never through a configuration file (<c>Coding - PostgreSQL.md</c> §6). Counts and aggregates only - no building data leaves. Open to every visitor, like the other <c>summariesbycountyids</c> reads.</para>
        /// <para>Naming no county part summarises every part holding results. A part holding none is absent rather than present with zeros, and a missing table answers an empty list.</para>
        /// </summary>
        /// <param name="countyIds">The identifiers of the county parts to summarise, repeated once per part (<c>countyids=55417&amp;countyids=53477</c>). Omit to summarise every part.</param>
        /// <param name="methodVersion">The method version to summarise; omit to summarise every version, one row each.</param>
        /// <param name="yearBuiltBounds">The ascending band bounds of the year built (e.g. the TABULA PL periods), repeated once per bound. Omit for one band of every year. Must be strictly ascending after de-duplication; a non-ascending sequence is refused with HTTP 400.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a JSON list of <see cref="BuildingHeatDemandCountyResult"/> (empty when nothing is stored or the table is missing), 400 for a negative timeout or non-ascending year-built bounds, 503 for a transient database failure, or 500.</returns>
        [HttpGet("summariesbycountyids", Name = $"{nameof(BuildingHeatDemandController)}_{nameof(GetSummariesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<BuildingHeatDemandCountyResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummariesByCountyIdsAsync([FromQuery(Name = "countyids")] List<int>? countyIds, [FromQuery(Name = "methodversion")] int? methodVersion = null, [FromQuery(Name = "yearbuiltbounds")] List<short>? yearBuiltBounds = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {CountyCount} county parts", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync), countyIds?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: commandtimeout cannot be negative", nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
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
                buildingHeatDemandCountyResults = await npgsqlConnection.BuildingHeatDemandCountyResultsAsync(countyIds is null || countyIds.Count == 0 ? null : countyIds, methodVersion, yearBuiltBounds, commandTimeout, cancellationToken);
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
    }
}
