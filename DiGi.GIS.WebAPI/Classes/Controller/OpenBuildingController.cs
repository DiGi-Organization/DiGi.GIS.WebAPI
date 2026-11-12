using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Web API controller for the open building datasets (EUBUCCO, Overture, OSM, DBSM, Microsoft, GHS-OBAT) stored beside BDOT10k: the release register and per-county summaries of the footprint tables (DiGi.GIS.PostgreSQL#121).
    /// <para>Read-only. It exposes the evidence paper 01 needs to state which release of each dataset was compared, when it was fetched, and how much of it comes from a register, OpenStreetMap or a detector.</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class OpenBuildingController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;
        private readonly PostgreSQL.Classes.OpenBuildingPostgreSQLConverter openBuildingPostgreSQLConverter;

        /// <summary>Initializes a new instance of the <see cref="OpenBuildingController"/> class.</summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher for the GIS PostgreSQL Web API.</param>
        /// <param name="openBuildingPostgreSQLConverter">The converter used to read the open building release register and footprint tables in PostgreSQL.</param>
        public OpenBuildingController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, PostgreSQL.Classes.OpenBuildingPostgreSQLConverter openBuildingPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.openBuildingPostgreSQLConverter = openBuildingPostgreSQLConverter;
        }

        /// <summary>
        /// Asynchronously retrieves the releases recorded in the <c>building_dataset</c> register: dataset key, version, download date, source URL, licence, observation date, file name, SHA-256, row count and attribute list.
        /// <para>The register identifier of each release is the <c>DatasetId</c> that <c>summariesbycountyids</c> reports.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key to read (<c>eubucco</c>, <c>overture</c>, <c>osm</c>, <c>dbsm</c>, <c>microsoft</c>, <c>ghs_obat</c>). Omit to read every dataset.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the releases as JSON (an empty list when none is registered for the key), 404 when the register does not exist yet, or an error status.</returns>
        [HttpGet("datasets", Name = $"{nameof(OpenBuildingController)}_{nameof(GetDatasetsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.BuildingDatasetRelease>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDatasetsAsync([FromQuery(Name = "datasetkey")] string? datasetKey = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for dataset {DatasetKey}", nameof(OpenBuildingController), nameof(GetDatasetsAsync), datasetKey ?? string.Empty);

            if (openBuildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "OpenBuildingPostgreSQLConverter cannot be null");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (datasetKey is not null && PostgreSQL.Query.OpenBuildingTableName(datasetKey) is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Unknown dataset key {DatasetKey}", datasetKey);
                return BadRequest($"Unknown dataset key '{datasetKey}'.");
            }

            List<PostgreSQL.Classes.BuildingDatasetRelease>? buildingDatasetReleases;
            try
            {
                buildingDatasetReleases = await openBuildingPostgreSQLConverter.GetReleasesAsync(datasetKey, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OpenBuildingController), nameof(GetDatasetsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (buildingDatasetReleases is null)
            {
                Serilog.Modify.Log("The building_dataset register does not exist");
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(buildingDatasetReleases);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
            }

            Serilog.Modify.Log("Number of BuildingDatasetReleases to be returned: {Count}", buildingDatasetReleases.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously summarises one dataset's footprint table per county part and release: the footprint count, its split by primary source (register, OpenStreetMap, Microsoft, Esri, other) and the number of footprints carrying a licence exception.
        /// <para>The data of the provenance diagram of paper 01 (figure R1, DiGi.GIS.PostgreSQL#126). Keyed by county part, never by code: a multi-part county is summarised by naming every part. A part holding no footprints is absent from the result rather than present with a zero.</para>
        /// <para>The licence-exception count reads the serialized footprint of every row, so a whole-table summary takes minutes; name the county parts in scope.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key to summarise (<c>eubucco</c>, <c>overture</c>, <c>osm</c>, <c>dbsm</c>, <c>microsoft</c>). Required; <c>ghs_obat</c> holds attributes, not footprints, and is refused.</param>
        /// <param name="countyIds">The identifiers of the county parts to summarise, repeated once per part. At most <see cref="Constants.OrtoDatas.MaximumSummaryCountyCount"/>; omit to summarise every partition.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the summaries as JSON, 404 when the dataset's table does not exist yet, or an error status.</returns>
        [HttpGet("summariesbycountyids", Name = $"{nameof(OpenBuildingController)}_{nameof(GetSummariesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.OpenBuildingCountyResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummariesByCountyIdsAsync([BindRequired, FromQuery(Name = "datasetkey")] string? datasetKey, [FromQuery(Name = "countyids")] List<int>? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for dataset {DatasetKey} and {CountyCount} counties", nameof(OpenBuildingController), nameof(GetSummariesByCountyIdsAsync), datasetKey ?? string.Empty, countyIds?.Count ?? 0);

            if (openBuildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "OpenBuildingPostgreSQLConverter cannot be null");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            string? tableName = PostgreSQL.Query.OpenBuildingTableName(datasetKey);
            if (tableName is null || string.Equals(tableName, PostgreSQL.Constants.TableName.GHS_OBAT_Building, StringComparison.Ordinal))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Dataset key {DatasetKey} does not name a footprint table", datasetKey ?? string.Empty);
                return BadRequest($"Dataset key '{datasetKey}' does not name a footprint table.");
            }

            if (countyIds is not null && countyIds.Count > Constants.OrtoDatas.MaximumSummaryCountyCount)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Too many counties named: {Count}, the limit is {Maximum}", countyIds.Count, Constants.OrtoDatas.MaximumSummaryCountyCount);
                return BadRequest($"At most {Constants.OrtoDatas.MaximumSummaryCountyCount} counties may be named. Omit the parameter to summarise every one.");
            }

            List<PostgreSQL.Classes.OpenBuildingCountyResult>? openBuildingCountyResults;
            try
            {
                openBuildingCountyResults = await openBuildingPostgreSQLConverter.GetSummariesByCountyIdsAsync(datasetKey, countyIds is null || countyIds.Count == 0 ? null : countyIds, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OpenBuildingController), nameof(GetSummariesByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (openBuildingCountyResults is null)
            {
                Serilog.Modify.Log("The footprint table of dataset {DatasetKey} does not exist", datasetKey ?? string.Empty);
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(openBuildingCountyResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
            }

            Serilog.Modify.Log("Number of OpenBuildingCountyResults to be returned: {Count}", openBuildingCountyResults.Count);
            return Content(json, "application/json");
        }
    }
}
