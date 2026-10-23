using DiGi.Geometry.Spatial.Classes;
using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Provides API endpoints for managing and updating Building data stored in a PostgreSQL database.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter; //States which polygon part of a multi-part county a building belongs to, from the 2D building already stored under it.
        private readonly BuildingPostgreSQLConverter buildingPostgreSQLConverter;
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        /// <summary>
        /// Initializes a new instance of the BuildingController class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher used to monitor changes to the PostgreSQL Web API configuration.</param>
        /// <param name="buildingPostgreSQLConverter">The converter for Building objects when interacting with a PostgreSQL database.</param>
        /// <param name="building2DPostgreSQLConverter">The converter for Building2D objects, used to read which county row a reference is already filed under.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter for administrative areal 2D data when interacting with a PostgreSQL database.</param>
        public BuildingController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, BuildingPostgreSQLConverter buildingPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.buildingPostgreSQLConverter = buildingPostgreSQLConverter;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <summary>
        /// Updates multiple building items based on the provided JSON array and identification code.
        /// <para>A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each building is filed under the part it actually belongs to - see <see cref="UpdateItemsByCountyIdsAsync"/> for how that is decided.</para>
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the building items to be updated.</param>
        /// <param name="code">The identification code required for the update operation.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPost("updateitems", Name = $"{nameof(BuildingController)}_{nameof(UpdateItemsAsync)}")]
        [ProducesResponseType(typeof(UpdateItemsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "code")] string? code, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(UpdateItemsAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Code cannot be null or empty");
                return BadRequest();
            }

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building update not allowed");
                return Unauthorized();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AdministrativeAreal2DPostgreSQLConverter is null");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No Building data to update");
                return NoContent();
            }

            HashSet<int>? countyIds = await administrativeAreal2DPostgreSQLConverter.GetIdsByCodeAsync(code, PostgreSQL.Enums.AdministrativeArealType.County, cancellationToken: cancellationToken);
            if (countyIds is null || countyIds.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "County code '{Code}' was not found in database", code);
                return BadRequest();
            }

            int[] countyIds_Resolved = [.. countyIds.OrderBy(x => x)];

            // Collapsing an ambiguous code onto one row is what let the skew in this table go unnoticed:
            // the upload reported success while everything filed under a sibling row read back empty.
            // Every part is passed on instead, and the batch is split between them per building.
            if (countyIds_Resolved.Length > 1)
            {
                Serilog.Modify.Log("County code '{Code}' matches {Count} rows ({CountyIds}) because the county has that many polygon parts. Each building is being filed under the part it belongs to", code, countyIds_Resolved.Length, string.Join(", ", countyIds_Resolved));
            }

            return await UpdateItemsByCountyIdsAsync(jsonArray, countyIds_Resolved, key, cancellationToken);
        }

        /// <summary>
        /// Updates multiple building items in the database for the given county rows.
        /// <para>The unambiguous counterpart of <see cref="UpdateItemsAsync"/>: it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.</para>
        /// <para>A single identifier is taken as stated and every building is filed under it. Several identifiers are the polygon parts of one multi-part county, and each building is then filed under the part it belongs to, decided in two steps:</para>
        /// <para>1. the part already holding the building's <c>building_2d</c> row, probed lowest part first. That row was filed by geometry when it was imported, and reusing its answer keeps both tables keyed by the same <c>(county_id, reference)</c> pair - a building filed under a part its footprint is not stored in reads back as missing.</para>
        /// <para>2. geometry, for a building no part holds a 2D row for: the part containing its bounding box, else the nearest part, else the part it overlaps most. Done by the converter, which drops a building it cannot place rather than filing it under a guess - such a building is reported as a rejection, not silently omitted.</para>
        /// <para>The county-part lookup (step 1) not running is answered as a distinct 500 naming it, before any geometry fallback runs, and a transient database failure as 503 with <c>Retry-After</c> - an unreachable database must not look like a decision by geometry.</para>
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the building items to be updated.</param>
        /// <param name="countyIds">The identifiers of the county rows the buildings belong to. Normally every polygon part of one county.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPost("updateitemsbycountyids", Name = $"{nameof(BuildingController)}_{nameof(UpdateItemsByCountyIdsAsync)}")]
        [ProducesResponseType(typeof(UpdateItemsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsByCountyIdsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(UpdateItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building update not allowed");
                return Unauthorized();
            }

            if (countyIds is null || countyIds.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null or empty");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No Building data to update");
                return NoContent();
            }

            try
            {
                List<int> countyIds_Candidate = [.. new HashSet<int>(countyIds).OrderBy(x => x)];

                List<CityGML.Classes.Building>? cityGMLBuildings = Core.Create.SerializableObjects<CityGML.Classes.Building>(jsonArray);
                if (cityGMLBuildings is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Buildings could not be converted from json");
                    return BadRequest();
                }

                Serilog.Modify.Log("Buildings conversion to PostgreSQL started. Buildings count: {Count}", cityGMLBuildings.Count);

                // Left unset while there is more than one candidate, so the county is decided below rather
                // than baked in here.
                int? countyId_Single = countyIds_Candidate.Count == 1 ? countyIds_Candidate[0] : null;

                List<Building> buildings = [];
                foreach (CityGML.Classes.Building cityGMLBuilding in cityGMLBuildings)
                {
                    Building? building = cityGMLBuilding.ToPostgreSQL(countyId_Single);
                    if (building is not null)
                    {
                        buildings.Add(building);
                    }
                }

                if (buildings.Count == 0)
                {
                    Serilog.Modify.Log("No Buildings PostgreSQL to update");
                    return NoContent();
                }

                if (countyId_Single is null)
                {
                    Dictionary<string, int>? countyIds_ByReference = await PostgreSQL.Query.CountyIdsByReferencesAsync(building2DPostgreSQLConverter, buildings.ConvertAll(x => x.Reference), countyIds_Candidate, cancellationToken: cancellationToken);

                    if (countyIds_ByReference is null)
                    {
                        // The lookup could not run at all. Letting the batch fall through to the geometry
                        // fallback here is what filed rows under parts building_2d disagrees with, with no
                        // signal to anyone - so the failure is answered instead of worked around.
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County parts could not be resolved: the building_2d lookup could not run (check the Main database configuration)");
                        return StatusCode(500, "County parts could not be resolved: the building_2d lookup could not run.");
                    }

                    List<string> references_Unresolved = [];
                    foreach (Building building in buildings)
                    {
                        if (building.Reference is not null && countyIds_ByReference.TryGetValue(building.Reference, out int countyId))
                        {
                            building.CountyId = countyId;
                            continue;
                        }

                        references_Unresolved.Add(building.Reference ?? string.Empty);
                    }

                    if (references_Unresolved.Count != 0)
                    {
                        // Not a failure: these fall through to the converter, which decides them by geometry
                        // and rejects only what it cannot place at all.
                        Serilog.Modify.Log("Buildings with no Building2D under the given parts, left to be decided by geometry: {Count}/{Total}. References: {References}", references_Unresolved.Count, buildings.Count, string.Join(", ", references_Unresolved.Take(20)));
                    }
                }

                Serilog.Modify.Log("Updating to database starting");

                PostgreSQLUpdateResult? postgreSQLUpdateResult = await buildingPostgreSQLConverter.UpdateAsync(buildings, countyIds_Candidate);

                UpdateItemsResult? updateItemsResult = postgreSQLUpdateResult.UpdateItemsResult(buildings.Count);
                if (updateItemsResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database could not be attempted");
                    return StatusCode(500, "Database update failed.");
                }

                // A drop means the row carried no geometry, no part could be decided for it, or a partition
                // could not be created. It is still a partial write, and it used to leave no trace.
                if (updateItemsResult.Rejected.Count != 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Buildings rejected before the database: {Count}/{Total}. References: {References}", updateItemsResult.Rejected.Count, updateItemsResult.Sent, updateItemsResult.Rejected.RejectionSample());
                }

                if (updateItemsResult.Updated == 0)
                {
                    if (updateItemsResult.Rejected.Count == updateItemsResult.Sent)
                    {
                        return StatusCode(500, $"All {updateItemsResult.Sent} Buildings were rejected before the database; none could be filed under a county.");
                    }

                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no Buildings have been updated");
                    return StatusCode(500, "Database update returned no modified building IDs.");
                }

                // Updated counts distinct identifiers, and rows colliding on the conflict key share one, so
                // Updated < Sent on its own proves nothing. Rejected is the exact figure.
                Serilog.Modify.Log("Updating to database ended. Updated Buildings: {After}/{Before}, rejected: {Rejected}", updateItemsResult.Updated, updateItemsResult.Sent, updateItemsResult.Rejected.Count);
                return Ok(updateItemsResult);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingController), nameof(UpdateItemsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Unhandled error during BuildingController.UpdateItemsByCountyIdsAsync");
                return StatusCode(500, exception.Message);
            }
        }

        /// <summary>
        /// Asynchronously checks for the existence of a collection of building references, optionally filtered by a county identifier.
        /// </summary>
        /// <param name="references">A list of strings representing the building references to be checked.</param>
        /// <param name="countyId">The optional county identifier used to filter the search.</param>
        /// <param name="inverted">A boolean indicating whether to return missing references (true) or existing references (false).</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> containing the set of matching reference strings.</returns>
        [HttpPost("containsbyreferences", Name = $"{nameof(BuildingController)}_{nameof(ContainsByReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(HashSet<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ContainsByReferencesAsync([FromBody] List<string>? references, [FromQuery(Name = "countyid")] int? countyId, [FromQuery(Name = "inverted")] bool? inverted, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(ContainsByReferencesAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);
            Serilog.Modify.Log("Inverted: {Inverted}", (inverted ?? false).ToString());

            if (references is null || references.Count == 0)
            {
                Serilog.Modify.Log("No references to check");
                return BadRequest("The references list cannot be empty.");
            }

            if (buildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingPostgreSQLConverter is null");
                return BadRequest();
            }

            HashSet<string> uniqueReferences = [.. references.Where(r => !string.IsNullOrWhiteSpace(r))];
            if (uniqueReferences.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "References could not be converted or are empty");
                return BadRequest("Provided list contains only empty values.");
            }

            Serilog.Modify.Log("References count: {Count}", uniqueReferences.Count);
            Serilog.Modify.Log("Query database starting");

            try
            {
                HashSet<string>? referencesExisting = await buildingPostgreSQLConverter.ContainsByReferencesAsync(uniqueReferences, countyId, inverted ?? false, cancellationToken: cancellationToken);
                referencesExisting ??= [];

                return Ok(referencesExisting);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingController), nameof(ContainsByReferencesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves the count of building records from the database, optionally filtered by a county identifier.
        /// </summary>
        /// <param name="countyId">The optional integer identifier of the county to filter the count; if null, the count is retrieved across all counties.</param>
        /// <param name="estimated">A boolean value indicating whether to read the estimated count from database statistics for faster execution on large partitions. An unanalysed partition returns 204 NoContent.</param>
        /// <param name="analyze">A boolean value indicating whether to run an analysis operation before fetching the estimated count to ensure higher accuracy.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> containing the row count as a long integer, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county partition does not exist.</returns>
        [HttpGet("count", Name = $"{nameof(BuildingController)}_{nameof(GetCountAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountAsync([FromQuery(Name = "countyid")] int? countyId, [FromQuery(Name = "estimated")] bool estimated = false, [FromQuery(Name = "analyze")] bool analyze = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId}", nameof(BuildingController), nameof(GetCountAsync), countyId?.ToString() ?? string.Empty);

            if (buildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingPostgreSQLConverter is null");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            long? count;
            try
            {
                count = estimated
                    ? await buildingPostgreSQLConverter.GetEstimatedCountAsync(countyId, analyze, commandTimeout, cancellationToken)
                    : await buildingPostgreSQLConverter.GetCountAsync(countyId, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingController), nameof(GetCountAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (count is null || (!estimated && count < 0))
            {
                Serilog.Modify.Log("County {CountyId} has no building partition", countyId?.ToString() ?? string.Empty);
                return NotFound();
            }

            if (estimated && count < 0)
            {
                Serilog.Modify.Log("County {CountyId} building partition exists but has not been analysed", countyId?.ToString() ?? string.Empty);
                return NoContent();
            }

            return Ok(count.Value);
        }

        /// <summary>
        /// Asynchronously retrieves buildings based on a provided reference and an optional county identifier.
        /// </summary>
        /// <param name="reference">The unique reference string used to identify the buildings.</param>
        /// <param name="countyId">An optional integer representing the county ID to filter the results.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbyreference", Name = $"{nameof(BuildingController)}_{nameof(GetItemsByReferenceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<CityGML.Classes.Building>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetItemsByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string? reference, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(GetItemsByReferenceAsync));

            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null or empty");
                return BadRequest();
            }

            if (buildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingPostgreSQLConverter is null");
                return BadRequest();
            }

            List<Building>? buildings_PostgreSQL = await buildingPostgreSQLConverter.GetBuildingsByReferenceAsync(reference, countyId, true, cancellationToken: cancellationToken);

            if (buildings_PostgreSQL is null || buildings_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No Buildings found for provided reference");
                return NoContent();
            }

            List<CityGML.Classes.Building> buildings = [];
            foreach (Building building_PostgreSQL in buildings_PostgreSQL)
            {
                CityGML.Classes.Building? building = building_PostgreSQL.ToDiGi();
                if (building is null)
                {
                    continue;
                }

                buildings.Add(building);
            }

            if (buildings.Count == 0)
            {
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(buildings) ?? string.Empty, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the single most relevant building for the provided reference and an optional county identifier.
        /// <para>When the X, Y or Z coordinates are provided they are used to break ties between candidates resolved from the reference.</para>
        /// <para>When the reference cannot be resolved and a point is provided, a spatial fallback search limited in X and Y by the maximum distance is performed.</para>
        /// </summary>
        /// <param name="reference">The unique reference string used to identify the building.</param>
        /// <param name="countyId">An optional integer representing the county ID to filter the results.</param>
        /// <param name="x">The optional X coordinate of the point used to break ties and to locate the building when the reference cannot be resolved.</param>
        /// <param name="y">The optional Y coordinate of the point used to break ties and to locate the building when the reference cannot be resolved.</param>
        /// <param name="z">The optional Z coordinate of the point used to break ties and to locate the building when the reference cannot be resolved.</param>
        /// <param name="maxDistance">The optional distance used to inflate the point in X and Y into the bounding box of the spatial fallback search. Defaults to 1.0 when not provided or invalid.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itembyreference", Name = $"{nameof(BuildingController)}_{nameof(GetItemByReferenceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(CityGML.Classes.Building), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetItemByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string? reference, [BindRequired, FromQuery(Name = "countyid")] int? countyId, [FromQuery(Name = "x")] double? x = null, [FromQuery(Name = "y")] double? y = null, [FromQuery(Name = "z")] double? z = null, [FromQuery(Name = "maxdistance")] double? maxDistance = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(GetItemByReferenceAsync));

            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);
            Serilog.Modify.Log("Point provided: X {X}, Y {Y}, Z {Z}", x?.ToString() ?? string.Empty, y?.ToString() ?? string.Empty, z?.ToString() ?? string.Empty);
            Serilog.Modify.Log("MaxDistance provided: {MaxDistance}", maxDistance?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null or empty");
                return BadRequest();
            }

            if (buildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingPostgreSQLConverter is null");
                return BadRequest();
            }

            if ((x is not null && double.IsNaN(x.Value)) || (y is not null && double.IsNaN(y.Value)) || (z is not null && double.IsNaN(z.Value)))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Point coordinates cannot be NaN");
                return BadRequest();
            }

            double maxDistance_Temp = maxDistance ?? 1.0;
            if (double.IsNaN(maxDistance_Temp) || maxDistance_Temp <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "MaxDistance is invalid. Default value will be used");
                maxDistance_Temp = 1.0;
            }

            Point3D? point3D = null;
            if (x is not null || y is not null || z is not null)
            {
                point3D = new(x ?? 0, y ?? 0, z ?? 0);
            }

            CityGML.Classes.Building? building;
            try
            {
                Building? building_PostgreSQL = await buildingPostgreSQLConverter.GetBuildingByReferenceAsync(reference, countyId, point3D, maxDistance_Temp, cancellationToken: cancellationToken);
                if (building_PostgreSQL is null)
                {
                    Serilog.Modify.Log("No Building found for provided reference");
                    return NoContent();
                }

                building = building_PostgreSQL.ToDiGi();
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Failed to retrieve Building for reference {Reference} (countyId {CountyId})", reference, countyId?.ToString() ?? string.Empty);
                return Problem(detail: "Failed to retrieve the building for the provided reference.", statusCode: StatusCodes.Status500InternalServerError);
            }

            if (building is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building could not be converted from PostgreSQL");
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(building) ?? string.Empty, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the single most relevant building for each of the provided references.
        /// <para>Several rows can share one reference (different level of detail or year); each reference is reduced to one building ranked by level of detail and then by year, matching the behaviour of <see cref="GetItemByReferenceAsync"/> when no coordinates are supplied.</para>
        /// <para>References without a matching building are omitted, so an empty array is a valid response and does not indicate an error.</para>
        /// </summary>
        /// <param name="references">The collection of unique reference strings used to identify the buildings.</param>
        /// <param name="countyId">An optional integer representing the county ID to filter the results.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("itemsbyreferences", Name = $"{nameof(BuildingController)}_{nameof(GetItemsByReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<CityGML.Classes.Building>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByReferencesAsync([FromBody] IEnumerable<string>? references, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(GetItemsByReferencesAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (references is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "References cannot be null");
                return BadRequest();
            }

            if (buildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingPostgreSQLConverter is null");
                return BadRequest();
            }

            List<Building>? buildings_PostgreSQL = await buildingPostgreSQLConverter.GetBuildingsByReferencesAsync(references, countyId, true, cancellationToken: cancellationToken);

            List<CityGML.Classes.Building> buildings = [];

            if (buildings_PostgreSQL is not null && buildings_PostgreSQL.Count != 0)
            {
                Dictionary<string, List<Building>> dictionary = [];
                foreach (Building building_PostgreSQL in buildings_PostgreSQL)
                {
                    string? reference = building_PostgreSQL?.Reference;
                    if (string.IsNullOrWhiteSpace(reference))
                    {
                        continue;
                    }

                    if (!dictionary.TryGetValue(reference, out List<Building>? buildings_Reference))
                    {
                        buildings_Reference = [];
                        dictionary[reference] = buildings_Reference;
                    }

                    buildings_Reference.Add(building_PostgreSQL!);
                }

                foreach (KeyValuePair<string, List<Building>> keyValuePair in dictionary)
                {
                    // No point is available in a batch request, so ranking falls back to level of detail and year.
                    Building? building_PostgreSQL = PostgreSQL.Query.Building(keyValuePair.Value, null);
                    if (building_PostgreSQL is null)
                    {
                        continue;
                    }

                    CityGML.Classes.Building? building = building_PostgreSQL.ToDiGi();
                    if (building is null)
                    {
                        continue;
                    }

                    buildings.Add(building);
                }
            }

            Serilog.Modify.Log("Buildings returned: {After}/{Before}", buildings.Count, buildings_PostgreSQL?.Count ?? 0);

            // An empty result is a valid outcome - callers page through references and must not treat it as a failure.
            if (buildings.Count == 0)
            {
                return Content(new JsonArray().ToJsonString(), "application/json");
            }

            return Content(Core.Convert.ToSystem_String(buildings) ?? string.Empty, "application/json");
        }


        /// <summary>
        /// Asynchronously retrieves the building with the latest created date for an optional county identifier.
        /// </summary>
        /// <param name="countyId">An optional integer representing the county ID to filter the results.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itembylatestcreatedat", Name = $"{nameof(BuildingController)}_{nameof(GetItemByLatestCreatedAtAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(CityGML.Classes.Building), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetItemByLatestCreatedAtAsync([FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingController), nameof(GetItemByLatestCreatedAtAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (buildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingPostgreSQLConverter is null");
                return BadRequest();
            }

            Building? building_PostgreSQL = await buildingPostgreSQLConverter.GetBuildingByLatestCreatedAtAsync(countyId, cancellationToken: cancellationToken);
            if (building_PostgreSQL is null)
            {
                Serilog.Modify.Log("No Building found for latest created at");
                return NoContent();
            }

            CityGML.Classes.Building? building = building_PostgreSQL.ToDiGi();
            if (building is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building could not be converted from PostgreSQL");
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(building) ?? string.Empty, "application/json");
        }
    }
}