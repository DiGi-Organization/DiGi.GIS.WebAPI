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
    /// Web API controller for the open building datasets (EUBUCCO, Overture, OSM, DBSM, Microsoft, GHS-OBAT) stored beside BDOT10k: the release register, per-county summaries of the footprint tables, their disk use (DiGi.GIS.PostgreSQL#121) and the match summaries against BDOT10k (DiGi.GIS.WebAPI#108).
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

        /// <summary>
        /// Asynchronously retrieves the disk use of the open building tables: the total size in bytes (table, TOAST and indexes) and the planner's row estimate of every county partition of a footprint table, and of the unpartitioned GHS-OBAT table as a whole (DiGi.GIS.PostgreSQL#150).
        /// <para>Read from the system catalog only, so it answers in milliseconds however large the tables are. The sizes of a table's partitions sum to the table's size. The measured disk use of the pilot imports (DiGi.GIS.PostgreSQL#126) and the per-county figure a full-country roll-out is extrapolated from.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key to measure (<c>eubucco</c>, <c>overture</c>, <c>osm</c>, <c>dbsm</c>, <c>microsoft</c>, <c>ghs_obat</c>). Omit to measure all six tables.</param>
        /// <param name="countyIds">The identifiers of the county parts whose partitions are reported, repeated once per part. At most <see cref="Constants.OrtoDatas.MaximumSummaryCountyCount"/>; omit to report every partition. The GHS-OBAT table has no partitions and is reported whole either way, with a null <c>CountyId</c>.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the sizes as JSON, 404 when none of the measured tables exists yet, or an error status.</returns>
        [HttpGet("sizes", Name = $"{nameof(OpenBuildingController)}_{nameof(GetSizesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.OpenBuildingTableSizeResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSizesAsync([FromQuery(Name = "datasetkey")] string? datasetKey = null, [FromQuery(Name = "countyids")] List<int>? countyIds = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for dataset {DatasetKey} and {CountyCount} counties", nameof(OpenBuildingController), nameof(GetSizesAsync), datasetKey ?? string.Empty, countyIds?.Count ?? 0);

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

            if (countyIds is not null && countyIds.Count > Constants.OrtoDatas.MaximumSummaryCountyCount)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Too many counties named: {Count}, the limit is {Maximum}", countyIds.Count, Constants.OrtoDatas.MaximumSummaryCountyCount);
                return BadRequest($"At most {Constants.OrtoDatas.MaximumSummaryCountyCount} counties may be named. Omit the parameter to report every one.");
            }

            List<PostgreSQL.Classes.OpenBuildingTableSizeResult>? openBuildingTableSizeResults;
            try
            {
                openBuildingTableSizeResults = await openBuildingPostgreSQLConverter.GetSizesAsync(datasetKey, countyIds is null || countyIds.Count == 0 ? null : countyIds, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OpenBuildingController), nameof(GetSizesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (openBuildingTableSizeResults is null)
            {
                Serilog.Modify.Log("No open building table of dataset {DatasetKey} exists", datasetKey ?? "(all)");
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(openBuildingTableSizeResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
            }

            Serilog.Modify.Log("Number of OpenBuildingTableSizeResults to be returned: {Count}", openBuildingTableSizeResults.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously reads the completeness and geometry measures of one open dataset against BDOT10k over a bounding box (paper 01, DiGi.GIS.WebAPI#108): one total row per release of the dataset, then one row per primary source in ordinal order.
        /// <para>The scope is the box: a building component is in it when its representative point - the centroid of its owning footprint, the BDOT10k one when it has one - lies in the box, boundary included. A component is counted whole or not at all, so two adjacent boxes never share one. The answer carries no stratum; the caller attaches it.</para>
        /// <para>An omission is a BDOT10k footprint no dataset footprint reached, so it belongs to no source: the BDOT10k-side figures (<c>BDOTCount</c>, <c>OmissionCount</c>, <c>Completeness</c>, <c>Quality</c>, <c>AreaCompleteness</c>, ...) are null on the per-source rows. Null means unknown, never zero - the area figures and the size classes are also null when a link in scope was stored before the areas were added. <c>DatasetCount</c>, <c>DatasetExcludedCount</c> and <c>CommissionCount</c> sum over the sources to the total row; the class counts do not, because a component whose footprints come from two sources counts under each. <c>ComputedAt</c> tells a reader whether a BDOT10k re-import has outdated the links; the match parameters are null when the scope mixes snapshots computed with different parameters.</para>
        /// <para>Read-only: the figures exist only after the match run has stored the links. An empty list means no link in the box.</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key to read (<c>eubucco</c>, <c>overture</c>, <c>osm</c>, <c>dbsm</c>, <c>microsoft</c>). Required; <c>ghs_obat</c> holds attributes, not footprints, and is refused.</param>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the summaries as JSON (an empty list when no link is in the box), 400 for a missing or unknown dataset key or an invalid box, 404 when the link table does not exist yet, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("matchsummary", Name = $"{nameof(OpenBuildingController)}_{nameof(GetMatchSummariesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.OpenBuildingMatchSummaryResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMatchSummariesAsync([BindRequired, FromQuery(Name = "datasetkey")] string? datasetKey, [BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for dataset {DatasetKey}", nameof(OpenBuildingController), nameof(GetMatchSummariesAsync), datasetKey ?? string.Empty);

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

            Geometry.Planar.Classes.BoundingBox2D? boundingBox2D = Create.BoundingBox2D(x_1, y_1, x_2, y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            List<PostgreSQL.Classes.OpenBuildingMatchSummaryResult>? openBuildingMatchSummaryResults;
            try
            {
                openBuildingMatchSummaryResults = await openBuildingPostgreSQLConverter.GetMatchSummariesAsync(datasetKey, boundingBox2D, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OpenBuildingController), nameof(GetMatchSummariesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (openBuildingMatchSummaryResults is null)
            {
                Serilog.Modify.Log("The building_dataset_match table does not exist");
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(openBuildingMatchSummaryResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
            }

            Serilog.Modify.Log("Number of OpenBuildingMatchSummaryResults to be returned: {Count}", openBuildingMatchSummaryResults.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously reads the completeness and geometry measures of one open dataset against BDOT10k over county parts (paper 01, DiGi.GIS.WebAPI#108): the county-scoped variant of <c>matchsummary</c>, with the same rows and the same meaning of null.
        /// <para>The scope is the named county parts, never a code: name every sibling part to read a whole multi-part county. The paper states which scope a figure comes from - per box (<c>matchsummary</c>) or per county (this route).</para>
        /// </summary>
        /// <param name="datasetKey">The dataset key to read (<c>eubucco</c>, <c>overture</c>, <c>osm</c>, <c>dbsm</c>, <c>microsoft</c>). Required; <c>ghs_obat</c> holds attributes, not footprints, and is refused.</param>
        /// <param name="countyIds">The identifiers of the county parts to read, repeated once per part. Required, at most <see cref="Constants.OrtoDatas.MaximumSummaryCountyCount"/>.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the summaries as JSON (an empty list when no link is in scope), 400 for a missing or unknown dataset key or a missing, empty or oversized county list, 404 when the link table does not exist yet, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("matchsummarybycountyids", Name = $"{nameof(OpenBuildingController)}_{nameof(GetMatchSummariesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.OpenBuildingMatchSummaryResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMatchSummariesByCountyIdsAsync([BindRequired, FromQuery(Name = "datasetkey")] string? datasetKey, [FromQuery(Name = "countyids")] List<int>? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for dataset {DatasetKey} and {CountyCount} counties", nameof(OpenBuildingController), nameof(GetMatchSummariesByCountyIdsAsync), datasetKey ?? string.Empty, countyIds?.Count ?? 0);

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

            if (countyIds is null || countyIds.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "No county named");
                return BadRequest("At least one county must be named.");
            }

            if (countyIds.Count > Constants.OrtoDatas.MaximumSummaryCountyCount)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Too many counties named: {Count}, the limit is {Maximum}", countyIds.Count, Constants.OrtoDatas.MaximumSummaryCountyCount);
                return BadRequest($"At most {Constants.OrtoDatas.MaximumSummaryCountyCount} counties may be named.");
            }

            List<PostgreSQL.Classes.OpenBuildingMatchSummaryResult>? openBuildingMatchSummaryResults;
            try
            {
                openBuildingMatchSummaryResults = await openBuildingPostgreSQLConverter.GetMatchSummariesByCountyIdsAsync(datasetKey, countyIds, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OpenBuildingController), nameof(GetMatchSummariesByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (openBuildingMatchSummaryResults is null)
            {
                Serilog.Modify.Log("The building_dataset_match table does not exist");
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(openBuildingMatchSummaryResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
            }

            Serilog.Modify.Log("Number of OpenBuildingMatchSummaryResults to be returned: {Count}", openBuildingMatchSummaryResults.Count);
            return Content(json, "application/json");
        }
    }
}
