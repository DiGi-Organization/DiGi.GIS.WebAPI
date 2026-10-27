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
    /// Controller responsible for handling requests related to occupancy data within the GIS PostgreSQL Web API.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class OccupancyDataController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly AdministrativeAreal2DOccupancyDataPostgreSQLConverter administrativeAreal2DOccupancyDataPostgreSQLConverter;
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly Building2DOccupancyDataPostgreSQLConverter building2DOccupancyDataPostgreSQLConverter;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter; //States which polygon part of a multi-part county a datum belongs to, from the 2D building already stored under it.
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        /// <summary>
        /// Initializes a new instance of the OccupancyDataController class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher used to monitor settings for the GIS PostgreSQL Web API.</param>
        /// <param name="building2DOccupancyDataPostgreSQLConverter">The converter used for building 2D occupancy data operations in the PostgreSQL database.</param>
        /// <param name="administrativeAreal2DOccupancyDataPostgreSQLConverter">The converter used for administrative areal 2D occupancy data operations in the PostgreSQL database.</param>
        /// <param name="building2DPostgreSQLConverter">The converter for Building2D objects, used to read which county row a reference is already filed under.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used for administrative areal 2D data operations in the PostgreSQL database.</param>
        public OccupancyDataController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, Building2DOccupancyDataPostgreSQLConverter building2DOccupancyDataPostgreSQLConverter, AdministrativeAreal2DOccupancyDataPostgreSQLConverter administrativeAreal2DOccupancyDataPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.building2DOccupancyDataPostgreSQLConverter = building2DOccupancyDataPostgreSQLConverter;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
            this.administrativeAreal2DOccupancyDataPostgreSQLConverter = administrativeAreal2DOccupancyDataPostgreSQLConverter;
        }

        /// <summary>
        /// Asynchronously updates occupancy data items for administrative areal 2D entities.
        /// </summary>
        /// <param name="jsonArray">The <see cref="JsonArray"/> containing the occupancy data items to be updated.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation, returning a bad request if updates are disabled or no content if the input array is null or empty.</returns>
        [HttpPost("administrativeareal2d/updateitems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdministrativeAreal2DUpdateItemsAsync([FromBody] JsonArray? jsonArray, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(AdministrativeAreal2DUpdateItemsAsync));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateOccupancyData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData update not allowed");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No OccupancyData to update");
                return NoContent();
            }

            List<GIS.Classes.OccupancyData>? occupancyDatas_GIS = Core.Create.SerializableObjects<GIS.Classes.OccupancyData>(jsonArray);
            if (occupancyDatas_GIS is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "OccupancyDatas could not be converted from json");
                return BadRequest();
            }

            Serilog.Modify.Log("OccupancyDatas conversion to PostgreSQL started. OccupancyDatas count: {Count}", occupancyDatas_GIS.Count);

            List<AdministrativeAreal2DOccupancyData> administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL = [];
            foreach (GIS.Classes.OccupancyData occupancyData_GIS in occupancyDatas_GIS)
            {
                if (PostgreSQL.Convert.ToPostgreSQL(occupancyData_GIS) is AdministrativeAreal2DOccupancyData administrativeAreal2DOccupancyData_PostgreSQL)
                {
                    administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL.Add(administrativeAreal2DOccupancyData_PostgreSQL);
                }
            }

            if (administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL is null || administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No AdministrativeAreal2DOccupancyData PostgreSQL to update");
                return NoContent();
            }

            Serilog.Modify.Log("OccupancyDatas conversion to PostgreSQL ended. OccupancyDatas converted: {After}/{Before}", administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL.Count, occupancyDatas_GIS.Count);

            Serilog.Modify.Log("Updating to database starting");

            HashSet<int>? ids = null;
            try
            {
                ids = await administrativeAreal2DOccupancyDataPostgreSQLConverter.UpdateAsync(administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL);
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Database could not be updated");
                return StatusCode(500, "Database update failed.");
            }

            // Answering Ok here is what let a whole county regeneration report success while writing
            // nothing: the storage database was unreachable, every batch came back empty, and the client
            // treats 200 as done. OccupancyDatas were converted and reached this point, so nothing updated
            // is a failure, not a quiet no-op. BuildingController already answers this case the same way.
            if (ids is null || ids.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no AdministrativeAreal2DOccupancyDatas have been updated");
                return StatusCode(500, "Database update returned no modified AdministrativeAreal2DOccupancyData IDs.");
            }

            Serilog.Modify.Log("Updating to database ended. Updated AdministrativeAreal2DOccupancyDatas: {After}/{Before}", ids.Count, administrativeAreal2DBuilding2DOccupancyDatas_PostgreSQL.Count);

            return Ok();
        }

        /// <summary>
        /// Asynchronously updates building 2D items based on the provided JSON data and identification code.
        /// <para>A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each datum is filed under the part it actually belongs to - see <see cref="Building2DUpdateItemsByCountyIdsAsync"/> for how that is decided.</para>
        /// </summary>
        /// <param name="jsonArray">The <see cref="JsonArray"/> containing the item data to be updated.</param>
        /// <param name="code">The identification code used to validate or categorize the update request.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("building2d/updateitems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Building2DUpdateItemsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "code")] string code, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(Building2DUpdateItemsAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Code cannot be null or empty");
                return BadRequest();
            }

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateOccupancyData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData update not allowed");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AdministrativeAreal2DPostgreSQLConverter is null");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No OccupancyData to update");
                return NoContent();
            }

            HashSet<int>? countyIds = await administrativeAreal2DPostgreSQLConverter.GetIdsByCodeAsync(code, PostgreSQL.Enums.AdministrativeArealType.County);
            if (countyIds is null || countyIds.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "County code '{Code}' was not found in database", code);
                return BadRequest();
            }

            int[] countyIds_Resolved = [.. countyIds.OrderBy(x => x)];

            // Collapsing an ambiguous code onto one row is what let the skew in this table go unnoticed:
            // the upload reported success while everything filed under a sibling row read back empty.
            // Every part is passed on instead, and the batch is split between them per datum.
            if (countyIds_Resolved.Length > 1)
            {
                Serilog.Modify.Log("County code '{Code}' matches {Count} rows ({CountyIds}) because the county has that many polygon parts. Each datum is being filed under the part its Building2D is stored in", code, countyIds_Resolved.Length, string.Join(", ", countyIds_Resolved));
            }

            return await Building2DUpdateItemsByCountyIdsAsync(jsonArray, countyIds_Resolved, key, cancellationToken);
        }

        /// <summary>
        /// Asynchronously updates building 2D occupancy items in the database for the given county rows.
        /// <para>The unambiguous counterpart of <see cref="Building2DUpdateItemsAsync"/>: it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.</para>
        /// <para>The identifiers are the parts of one county in play, and each datum is filed under the part already holding the <c>building_2d</c> row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same <c>(county_id, reference)</c> pair.</para>
        /// <para>A datum no named part holds is widened to every part of the county its parts name, and only a datum no part of the county holds is left unwritten - it carries no geometry of its own, so nothing states where it belongs, and storing it under a guessed part is the state this replaced.</para>
        /// <para>The county-part lookup not running is answered as a distinct 500 naming it, and a transient database failure as 503 with <c>Retry-After</c>, so an unreachable database is never mistaken for a quiet no-op.</para>
        /// </summary>
        /// <param name="jsonArray">The <see cref="JsonArray"/> containing the item data to be updated.</param>
        /// <param name="countyIds">The identifiers of the county rows the occupancy data belong to. Normally every polygon part of one county.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("building2d/updateitemsbycountyids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Building2DUpdateItemsByCountyIdsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(Building2DUpdateItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateOccupancyData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData update not allowed");
                return BadRequest();
            }

            if (countyIds is null || countyIds.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null or empty");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No OccupancyData to update");
                return NoContent();
            }

            try
            {
                List<GIS.Classes.OccupancyData>? occupancyDatas_GIS = Core.Create.SerializableObjects<GIS.Classes.OccupancyData>(jsonArray);
                if (occupancyDatas_GIS is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "OccupancyDatas could not be converted from json");
                    return BadRequest();
                }

                Serilog.Modify.Log("OccupancyDatas conversion to PostgreSQL started. OccupancyDatas count: {Count}", occupancyDatas_GIS.Count);

                List<int> countyIds_Candidate = [.. new HashSet<int>(countyIds).OrderBy(x => x)];

                // A datum carries no geometry, so the 2D building its reference names is the only thing that can say
                // which part it belongs to. Every item is resolved through building_2d regardless of how many ids the
                // caller sent - naming one id is not evidence the code has one part. A datum no named part holds is
                // widened to every part of the named county before it is left unwritten, never filed under a guessed part.
                Dictionary<string, int>? countyIds_ByReference = await PostgreSQL.Query.CountyIdsByReferencesWithSiblingFallbackAsync(building2DPostgreSQLConverter, administrativeAreal2DPostgreSQLConverter, occupancyDatas_GIS.ConvertAll(x => x?.Reference), countyIds_Candidate, cancellationToken: cancellationToken);

                if (countyIds_ByReference is null)
                {
                    // The lookup could not run at all, which is a different failure from running and resolving
                    // nothing: answering it as "no datum resolved" is how an unreachable Main database came to
                    // look like a clean no-op while a batch was silently filed under nothing.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County parts could not be resolved: the building_2d lookup could not run (check the Main database configuration)");
                    return StatusCode(500, "County parts could not be resolved: the building_2d lookup could not run.");
                }

                List<string> references_Unresolved = [];

                List<Building2DOccupancyData> building2DOccupancyDatas_PostgreSQL = [];
                foreach (GIS.Classes.OccupancyData occupancyData_GIS in occupancyDatas_GIS)
                {
                    if (occupancyData_GIS?.Reference is null || !countyIds_ByReference.TryGetValue(occupancyData_GIS.Reference, out int countyId))
                    {
                        references_Unresolved.Add(occupancyData_GIS?.Reference ?? string.Empty);
                        continue;
                    }

                    if (PostgreSQL.Convert.ToPostgreSQL(occupancyData_GIS, countyId) is Building2DOccupancyData building2DOccupancyData_PostgreSQL)
                    {
                        building2DOccupancyDatas_PostgreSQL.Add(building2DOccupancyData_PostgreSQL);
                    }
                }

                if (references_Unresolved.Count != 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyDatas not written because no Building2D under the given parts carries their reference: {Count}/{Total}. References: {References}", references_Unresolved.Count, occupancyDatas_GIS.Count, string.Join(", ", references_Unresolved.Take(20)));
                }

                if (building2DOccupancyDatas_PostgreSQL is null || building2DOccupancyDatas_PostgreSQL.Count == 0)
                {
                    // Reached only when the lookup ran and resolved nothing: the lookup-not-run case has its
                    // own 500 above, so this text no longer needs to hedge with "database unreachable".
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "OccupancyData not written because no Building2D under the given parts carries any of the references");
                    return StatusCode(500, "No OccupancyData could be written; none of the references resolved to a county part.");
                }

                Serilog.Modify.Log("OccupancyDatas conversion to PostgreSQL ended. OccupancyDatas converted: {After}/{Before}", building2DOccupancyDatas_PostgreSQL.Count, occupancyDatas_GIS.Count);

                Serilog.Modify.Log("Updating to database starting");

                PostgreSQLUpdateResult? updateResult = await building2DOccupancyDataPostgreSQLConverter.UpdateAsync(building2DOccupancyDatas_PostgreSQL);

                if (updateResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no Building2DOccupancyDatas have been updated");
                    return StatusCode(500, "Database update returned no modified Building2DOccupancyData IDs.");
                }

                // Logged before the empty-ids check because it is the explanation for it: when every row is
                // rejected the identifier set is empty, and without this the 500 above carries no reason.
                if (updateResult.Rejections.Count != 0)
                {
                    string references_Sample = string.Join(", ",
                        updateResult.Rejections
                            .Where(rejection => !string.IsNullOrWhiteSpace(rejection.Reference))
                            .Select(rejection => rejection.Reference!)
                            .Take(20));

                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning,
                        "Building2DOccupancyDatas not written because no county part was stated: {Count}/{Total}. References: {References}",
                        updateResult.Rejections.Count, building2DOccupancyDatas_PostgreSQL.Count, references_Sample);
                }

                // Answering Ok here is what let a whole county regeneration report success while writing
                // nothing: the storage database was unreachable, every batch came back empty, and the client
                // treats 200 as done. OccupancyDatas were converted and reached this point, so nothing updated
                // is a failure, not a quiet no-op. BuildingController already answers this case the same way.
                if (updateResult.Ids.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no Building2DOccupancyDatas have been updated");
                    return StatusCode(500, "Database update returned no modified Building2DOccupancyData IDs.");
                }

                Serilog.Modify.Log("Updating to database ended. Updated Building2DOccupancyDatas: {After}/{Before}, Rejected: {Rejected}",
                    updateResult.Ids.Count, building2DOccupancyDatas_PostgreSQL.Count, updateResult.Rejections.Count);

                return Ok();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OccupancyDataController), nameof(Building2DUpdateItemsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Unhandled error during OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync");
                return StatusCode(500, exception.Message);
            }
        }

        /// <summary>
        /// Retrieves administrative areal 2D items based on the provided reference identifier.
        /// </summary>
        /// <param name="reference">The unique reference string used to identify the administrative areal 2D items.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation, containing the requested items or an error response.</returns>
        [HttpGet("administrativeareal2d/itemsbyreference")]
        public async Task<IActionResult> GetAdministrativeAreal2DItemsByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(GetAdministrativeAreal2DItemsByReferenceAsync));

            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null or empty");
                return BadRequest();
            }

            if (administrativeAreal2DOccupancyDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AdministrativeAreal2DOccupancyDataPostgreSQLConverter is null");
                return BadRequest();
            }

            List<AdministrativeAreal2DOccupancyData>? administrativeAreal2DOccupancyDatas_PostgreSQL = await administrativeAreal2DOccupancyDataPostgreSQLConverter.GetItemsByReferenceAsync(reference, null, cancellationToken: cancellationToken);

            if (administrativeAreal2DOccupancyDatas_PostgreSQL is null || administrativeAreal2DOccupancyDatas_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No AdministrativeAreal2DOccupancyDatas found for provided reference");
                return NoContent();
            }

            List<GIS.Interfaces.IOccupancyData> occupancyDatas = [];
            foreach (AdministrativeAreal2DOccupancyData administrativeAreal2DOccupancyData_PostgreSQL in administrativeAreal2DOccupancyDatas_PostgreSQL)
            {
                GIS.Interfaces.IOccupancyData? occupancyData_GIS = administrativeAreal2DOccupancyData_PostgreSQL.ToDiGi();
                if (occupancyData_GIS is null)
                {
                    continue;
                }

                occupancyDatas.Add(occupancyData_GIS);
            }

            if (occupancyDatas is null || occupancyDatas.Count == 0)
            {
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(occupancyDatas) ?? string.Empty, "application/json");
        }

        /// <summary>
        /// Retrieves Building 2D occupancy data items based on a specified reference and an optional county identifier.
        /// </summary>
        /// <param name="reference">The unique reference string used to identify the building 2D items.</param>
        /// <param name="countyId">The optional identifier of the county associated with the building data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> containing the requested building 2D items, or a <see cref="BadRequestResult"/> if the reference is null or whitespace.</returns>
        [HttpGet("building2d/itemsbyreference")]
        public async Task<IActionResult> GetBuilding2DItemsByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(GetBuilding2DItemsByReferenceAsync));

            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null or empty");
                return BadRequest();
            }

            if (building2DOccupancyDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "OccupancyDataPostgreSQLConverter is null");
                return BadRequest();
            }

            List<Building2DOccupancyData>? building2DOccupancyDatas_PostgreSQL = await building2DOccupancyDataPostgreSQLConverter.GetItemsByReferenceAsync(reference, countyId, null, true, cancellationToken: cancellationToken);

            if (building2DOccupancyDatas_PostgreSQL is null || building2DOccupancyDatas_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No Building2DOccupancyDatas found for provided reference");
                return NoContent();
            }

            List<GIS.Interfaces.IOccupancyData> occupancyDatas = [];
            foreach (Building2DOccupancyData building2DOccupancyData_PostgreSQL in building2DOccupancyDatas_PostgreSQL)
            {
                GIS.Interfaces.IOccupancyData? occupancyData_GIS = building2DOccupancyData_PostgreSQL.ToDiGi();
                if (occupancyData_GIS is null)
                {
                    continue;
                }

                occupancyDatas.Add(occupancyData_GIS);
            }

            if (occupancyDatas is null || occupancyDatas.Count == 0)
            {
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(occupancyDatas) ?? string.Empty, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the building references that hold more than one occupancy data record, optionally filtered by county identifier.
        /// </summary>
        /// <param name="countyId">The optional integer identifier of the county to filter by; if null, searches across all counties.</param>
        /// <param name="limit">The maximum number of duplicate references to return. Defaults to 100.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of duplicate references - <c>200 []</c> when there are none - or 500 when the query could not run, so "clean" and "failed" are never the same answer.</returns>
        [HttpGet("building2d/duplicatereferences", Name = $"{nameof(OccupancyDataController)}_{nameof(GetBuilding2DDuplicateReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Building2DReferenceDuplicate>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBuilding2DDuplicateReferencesAsync([FromQuery(Name = "countyid")] int? countyId = null, [Minimum(1), FromQuery(Name = "limit")] int limit = 100, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(GetBuilding2DDuplicateReferencesAsync));

            if (limit <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Limit has to be greater than zero");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (building2DOccupancyDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DOccupancyDataPostgreSQLConverter is null");
                return BadRequest();
            }

            try
            {
                // None is an empty list and a query that could not run is a 500 - a 404 for both could not be told
                // from a missing route (DiGi.GIS.WebAPI#49).
                List<Building2DReferenceDuplicate>? building2DReferenceDuplicates = await building2DOccupancyDataPostgreSQLConverter.GetBuilding2DReferenceDuplicatesAsync(countyId, limit, commandTimeout, cancellationToken);
                if (building2DReferenceDuplicates is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Occupancy data reference duplicates could not be read");
                    return StatusCode(500, "Reference duplicates could not be read.");
                }

                return Content(Core.Convert.ToSystem_String(building2DReferenceDuplicates) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OccupancyDataController), nameof(GetBuilding2DDuplicateReferencesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OccupancyDataController), nameof(GetBuilding2DDuplicateReferencesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves the total count of building references that hold more than one occupancy data record, optionally filtered by county identifier.
        /// </summary>
        /// <param name="countyId">The optional integer identifier of the county to filter by; if null, counts across all counties.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the duplicates count, or 404 if the partition does not exist or count is negative.</returns>
        [HttpGet("building2d/duplicatescount", Name = $"{nameof(OccupancyDataController)}_{nameof(GetBuilding2DDuplicatesCountAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBuilding2DDuplicatesCountAsync([FromQuery(Name = "countyid")] int? countyId = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(GetBuilding2DDuplicatesCountAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (building2DOccupancyDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DOccupancyDataPostgreSQLConverter is null");
                return BadRequest();
            }

            try
            {
                long count = await building2DOccupancyDataPostgreSQLConverter.GetDuplicatesCountAsync(countyId, commandTimeout, cancellationToken);
                if (count < 0)
                {
                    return NotFound();
                }

                return Content(count.ToString(), "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OccupancyDataController), nameof(GetBuilding2DDuplicatesCountAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OccupancyDataController), nameof(GetBuilding2DDuplicatesCountAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves, for one county part, each subdivision's occupancy figure next to the sum of the occupancy rows stored for the buildings filed under it, to check that the distribution conserves the figure.
        /// </summary>
        /// <param name="countyId">The integer identifier of the county part; required, as an omitted value is refused with HTTP 400 rather than read as zero.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of <see cref="Building2DOccupancySubdivisionResult"/> - <c>200 []</c> when the county part holds none - or 500 when the query could not run.</returns>
        [HttpGet("building2d/subdivisionsnapshotbycountyid", Name = $"{nameof(OccupancyDataController)}_{nameof(GetBuilding2DSubdivisionSnapshotByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Building2DOccupancySubdivisionResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBuilding2DSubdivisionSnapshotByCountyIdAsync([FromQuery(Name = "countyid")] int? countyId = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(GetBuilding2DSubdivisionSnapshotByCountyIdAsync));

            if (countyId is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyId is required");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (building2DOccupancyDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DOccupancyDataPostgreSQLConverter is null");
                return BadRequest();
            }

            try
            {
                List<Building2DOccupancySubdivisionResult>? building2DOccupancySubdivisionResults = await building2DOccupancyDataPostgreSQLConverter.GetOccupancySubdivisionSnapshotsAsync(countyId.Value, commandTimeout, cancellationToken);
                if (building2DOccupancySubdivisionResults is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Occupancy subdivision snapshot could not be read");
                    return StatusCode(500, "Occupancy subdivision snapshot could not be read.");
                }

                return Content(Core.Convert.ToSystem_String(building2DOccupancySubdivisionResults) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OccupancyDataController), nameof(GetBuilding2DSubdivisionSnapshotByCountyIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OccupancyDataController), nameof(GetBuilding2DSubdivisionSnapshotByCountyIdAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves a read-only snapshot of the stored building occupancy rows, one entry per county part, to compare the stored occupancies before and after a re-run.
        /// </summary>
        /// <param name="countyId">The optional integer identifier of the county part to restrict the snapshot to; if null, every county part is measured.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of <see cref="Building2DOccupancySnapshotResult"/> - <c>200 []</c> when no row is stored - or 500 when the query could not run, so "empty" and "failed" are never the same answer.</returns>
        [HttpGet("building2d/snapshotbycountyid", Name = $"{nameof(OccupancyDataController)}_{nameof(GetBuilding2DSnapshotByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Building2DOccupancySnapshotResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBuilding2DSnapshotByCountyIdAsync([FromQuery(Name = "countyid")] int? countyId = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OccupancyDataController), nameof(GetBuilding2DSnapshotByCountyIdAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (building2DOccupancyDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DOccupancyDataPostgreSQLConverter is null");
                return BadRequest();
            }

            try
            {
                List<Building2DOccupancySnapshotResult>? building2DOccupancySnapshotResults = await building2DOccupancyDataPostgreSQLConverter.GetOccupancySnapshotsAsync(countyId, commandTimeout, cancellationToken);
                if (building2DOccupancySnapshotResults is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Occupancy snapshot could not be read");
                    return StatusCode(500, "Occupancy snapshot could not be read.");
                }

                return Content(Core.Convert.ToSystem_String(building2DOccupancySnapshotResults) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OccupancyDataController), nameof(GetBuilding2DSnapshotByCountyIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OccupancyDataController), nameof(GetBuilding2DSnapshotByCountyIdAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }
    }
}