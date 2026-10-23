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
    /// Provides API endpoints for managing and updating year built data stored in a PostgreSQL database.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class YearBuiltDataController : DiGi.WebAPI.Classes.WebAPIController
    {
        private const int referenceCount_Maximum = 10000;

        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter; //States which polygon part of a multi-part county a datum belongs to, from the 2D building already stored under it.
        private readonly BuildingDataPostgreSQLConverter buildingDataPostgreSQLConverter; //Holds the predicted, user and calculated year built columns derived from the stored history.
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;
        private readonly YearBuiltDataPostgreSQLConverter yearBuiltDataPostgreSQLConverter;
        private readonly DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager;
        private readonly DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore;

        /// <summary>
        /// Initializes a new instance of the YearBuiltDataController class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher used to monitor changes to the PostgreSQL Web API configuration.</param>
        /// <param name="yearBuiltDataPostgreSQLConverter">The converter for YearBuiltData objects when interacting with a PostgreSQL database.</param>
        /// <param name="building2DPostgreSQLConverter">The converter for Building2D objects, used to read which county row a reference is already filed under.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter for administrative areal 2D data when interacting with a PostgreSQL database.</param>
        /// <param name="buildingDataPostgreSQLConverter">The converter of the building data table, whose derived year built columns are recomputed after the stored history changes.</param>
        /// <param name="securityKeyManager">The user extension&apos;s security key manager; <c>null</c> when the user extension is not loaded, in which case every user-token check denies.</param>
        /// <param name="tokenRevocationStore">The user extension&apos;s token revocation store; <c>null</c> when the user extension is not loaded, in which case every user-token check denies.</param>
        public YearBuiltDataController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, YearBuiltDataPostgreSQLConverter yearBuiltDataPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, BuildingDataPostgreSQLConverter buildingDataPostgreSQLConverter, DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager = null, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore = null)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.yearBuiltDataPostgreSQLConverter = yearBuiltDataPostgreSQLConverter;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
            this.buildingDataPostgreSQLConverter = buildingDataPostgreSQLConverter;
            this.securityKeyManager = securityKeyManager;
            this.tokenRevocationStore = tokenRevocationStore;
        }

        /// <summary>
        /// Updates multiple year built data items based on the provided JSON array and identification code.
        /// <para>A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each datum is filed under the part it actually belongs to - see <see cref="UpdateItemsByCountyIdsAsync"/> for how that is decided.</para>
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the data items to be updated.</param>
        /// <param name="code">The identification code required for the update operation.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPost("updateitems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "code")] string code, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(UpdateItemsAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Code cannot be null or empty");
                return BadRequest();
            }

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData update not allowed");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AdministrativeAreal2DPostgreSQLConverter is null");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No YearBuiltData to update");
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

            return await UpdateItemsByCountyIdsAsync(jsonArray, countyIds_Resolved, key, cancellationToken);
        }

        /// <summary>
        /// Updates multiple year built data items in the database for the given county rows.
        /// <para>The unambiguous counterpart of <see cref="UpdateItemsAsync"/>: it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.</para>
        /// <para>The identifiers are the parts of one county in play, and each datum is filed under the part already holding the <c>building_2d</c> row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same <c>(county_id, reference)</c> pair.</para>
        /// <para>A datum no named part holds is widened to every part of the county its parts name, and only a datum no part of the county holds is left unwritten - it carries no geometry of its own, so nothing states where it belongs, and storing it under a guessed part is the state this replaced.</para>
        /// <para>The county-part lookup not running is answered as a distinct 500 naming it, and a transient database failure as 503 with <c>Retry-After</c>, so an unreachable database is never mistaken for a quiet no-op.</para>
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the data items to be updated.</param>
        /// <param name="countyIds">The identifiers of the county rows the year built data belong to. Normally every polygon part of one county.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPost("updateitemsbycountyids")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsByCountyIdsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(UpdateItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData update not allowed");
                return BadRequest();
            }

            if (countyIds is null || countyIds.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null or empty");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No YearBuiltData to update");
                return NoContent();
            }

            try
            {
                List<GIS.Classes.YearBuiltData>? yearBuiltDatas_GIS = Core.Create.SerializableObjects<GIS.Classes.YearBuiltData>(jsonArray);
                if (yearBuiltDatas_GIS is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "YearBuiltDatas could not be converted from json");
                    return BadRequest();
                }

                Serilog.Modify.Log("YearBuiltDatas conversion to PostgreSQL started. YearBuiltDatas count: {Count}", yearBuiltDatas_GIS.Count);

                List<int> countyIds_Candidate = [.. new HashSet<int>(countyIds).OrderBy(x => x)];

                // A datum carries no geometry, so the 2D building its reference names is the only thing that can say
                // which part it belongs to. Every item is resolved through building_2d regardless of how many ids the
                // caller sent - naming one id is not evidence the code has one part. A datum no named part holds is
                // widened to every part of the named county before it is left unwritten, never filed under a guessed part.
                Dictionary<string, int>? countyIds_ByReference = await PostgreSQL.Query.CountyIdsByReferencesWithSiblingFallbackAsync(building2DPostgreSQLConverter, administrativeAreal2DPostgreSQLConverter, yearBuiltDatas_GIS.ConvertAll(x => x?.Reference), countyIds_Candidate, cancellationToken: cancellationToken);

                if (countyIds_ByReference is null)
                {
                    // The lookup could not run at all, which is a different failure from running and resolving
                    // nothing: answering it as "no datum resolved" is how an unreachable Main database came to
                    // look like a clean no-op while a batch was silently filed under nothing.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County parts could not be resolved: the building_2d lookup could not run (check the Main database configuration)");
                    return StatusCode(500, "County parts could not be resolved: the building_2d lookup could not run.");
                }

                List<string> references_Unresolved = [];

                List<YearBuiltData> yearBuiltDatas_PostgreSQL = [];
                foreach (GIS.Classes.YearBuiltData yearBuiltData_GIS in yearBuiltDatas_GIS)
                {
                    if (yearBuiltData_GIS?.Reference is null || !countyIds_ByReference.TryGetValue(yearBuiltData_GIS.Reference, out int countyId))
                    {
                        references_Unresolved.Add(yearBuiltData_GIS?.Reference ?? string.Empty);
                        continue;
                    }

                    if (PostgreSQL.Convert.ToPostgreSQL(yearBuiltData_GIS, countyId) is YearBuiltData yearBuiltData_PostgreSQL)
                    {
                        yearBuiltDatas_PostgreSQL.Add(yearBuiltData_PostgreSQL);
                    }
                }

                if (references_Unresolved.Count != 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltDatas not written because no Building2D under the given parts carries their reference: {Count}/{Total}. References: {References}", references_Unresolved.Count, yearBuiltDatas_GIS.Count, string.Join(", ", references_Unresolved.Take(20)));
                }

                if (yearBuiltDatas_PostgreSQL is null || yearBuiltDatas_PostgreSQL.Count == 0)
                {
                    // Reached only when the lookup ran and resolved nothing: the lookup-not-run case has its
                    // own 500 above, so this text no longer needs to hedge with "database unreachable".
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData not written because no Building2D under the given parts carries any of the references");
                    return StatusCode(500, "No YearBuiltData could be written; none of the references resolved to a county part.");
                }

                Serilog.Modify.Log("YearBuiltDatas conversion to PostgreSQL ended. YearBuiltDatas converted: {After}/{Before}", yearBuiltDatas_PostgreSQL.Count, yearBuiltDatas_GIS.Count);

                Serilog.Modify.Log("Updating to database starting");

                PostgreSQLUpdateResult? updateResult = await yearBuiltDataPostgreSQLConverter.UpdateAsync(yearBuiltDatas_PostgreSQL);

                if (updateResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no YearBuiltDatas have been updated");
                    return StatusCode(500, "Database update returned no modified YearBuiltData IDs.");
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
                        "YearBuiltDatas not written because no county part was stated: {Count}/{Total}. References: {References}",
                        updateResult.Rejections.Count, yearBuiltDatas_PostgreSQL.Count, references_Sample);
                }

                // Answering Ok here is what let a whole county regeneration report success while writing
                // nothing: the storage database was unreachable, every batch came back empty, and the client
                // treats 200 as done. YearBuiltDatas were converted and reached this point, so nothing updated
                // is a failure, not a quiet no-op. BuildingController already answers this case the same way.
                if (updateResult.Ids.Count == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no YearBuiltDatas have been updated");
                    return StatusCode(500, "Database update returned no modified YearBuiltData IDs.");
                }

                Serilog.Modify.Log("Updating to database ended. Updated YearBuiltDatas: {After}/{Before}, Rejected: {Rejected}",
                    updateResult.Ids.Count, yearBuiltDatas_PostgreSQL.Count, updateResult.Rejections.Count);

                return Ok();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(UpdateItemsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Unhandled error during YearBuiltDataController.UpdateItemsByCountyIdsAsync");
                return StatusCode(500, exception.Message);
            }
        }

        /// <summary>
        /// Records a single user-supplied year built entry for one building, on behalf of the signed-in visitor.
        /// <para>The bearer token replaces the machine <c>key</c> header: the identity is a user, and the entry is stored under that user&apos;s email with the time it was recorded. The write is gated by the same <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData"/> flag as the machine update endpoints, and each check is logged distinctly so the cause of a 4xx is recoverable even where the status is shared.</para>
        /// </summary>
        /// <param name="parameter">The user year built entry to record.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 on a committed write, 404 when no building holds the reference under the given part, 401 without a valid user token, 400 for a disabled flag or an invalid body, or 500 when the write failed.</returns>
        [HttpPost("setuseryearbuilt", Name = $"{nameof(YearBuiltDataController)}_{nameof(SetUserYearBuiltAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetUserYearBuiltAsync([FromBody] Parameter.UserYearBuiltParameter? parameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(SetUserYearBuiltAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: no valid user token");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: AllowUpdateYearBuiltData is disabled");
                return BadRequest();
            }

            if (parameter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: body is missing");
                return BadRequest();
            }

            if (parameter.CountyId is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: CountyId is missing");
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(parameter.Reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: Reference is missing");
                return BadRequest();
            }

            if (parameter.Year is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: Year is missing");
                return BadRequest();
            }

            int relation = parameter.Relation ?? 0;
            if (!System.Enum.IsDefined(typeof(GIS.Enums.YearBuiltRelation), relation))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt rejected: Relation {Relation} is not a YearBuiltRelation member", relation);
                return BadRequest();
            }

            int countyId = parameter.CountyId.Value;
            string reference = parameter.Reference!;
            short year = parameter.Year.Value;

            GIS.Classes.UserYearBuilt userYearBuilt = new(year, (GIS.Enums.YearBuiltRelation)relation, DateTimeOffset.UtcNow, email);

            bool? result;
            try
            {
                result = await yearBuiltDataPostgreSQLConverter.UpdateUserYearBuiltAsync(countyId, reference, userYearBuilt, commandTimeout: 30, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(SetUserYearBuiltAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(SetUserYearBuiltAsync));
                return StatusCode(500, "Internal server error during user year built write");
            }

            if (result is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt: no building under part {CountyId} carries reference {Reference}", countyId, reference);
                return NotFound();
            }

            if (result is false)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetUserYearBuilt: write for reference {Reference} of part {CountyId} failed and rolled back", reference, countyId);
                return StatusCode(500, "User year built write failed.");
            }

            Serilog.Modify.Log("SetUserYearBuilt recorded year {Year} for reference {Reference} of part {CountyId} as {User}", year, reference, countyId, email);

            await RefreshBuildingDataYearBuiltAsync(countyId, [reference], cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Withdraws the signed-in visitor&apos;s own user-provided year built entry from one building.
        /// <para>The bearer token identifies the user, exactly as for <see cref="SetUserYearBuiltAsync"/>, and the same <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData"/> flag gates it: withdrawing one&apos;s own label is the counterpart of setting it, not a moderation delete. The entry is looked for under every polygon part of the county the given part belongs to, and only an entry recorded by this user is withdrawn - a building whose entry another user recorded answers 403 and is left untouched. The objects themselves are kept.</para>
        /// <para>After a committed withdrawal the building&apos;s derived <c>building_data</c> columns are recomputed, best-effort: a failure there is logged and the withdrawal still answers 200.</para>
        /// </summary>
        /// <param name="parameter">The building to withdraw the entry of; <see cref="Parameter.UserYearBuiltParameter.Year"/> and <see cref="Parameter.UserYearBuiltParameter.Relation"/> are ignored.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 when the entry was withdrawn, 404 when the building holds no user entry, 403 when the entry belongs to another user, 401 without a valid user token, 400 for a disabled flag or an invalid body, or 500 when the withdrawal failed.</returns>
        [HttpPost("removeuseryearbuilt", Name = $"{nameof(YearBuiltDataController)}_{nameof(RemoveUserYearBuiltAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUserYearBuiltAsync([FromBody] Parameter.UserYearBuiltParameter? parameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(RemoveUserYearBuiltAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveUserYearBuilt rejected: no valid user token");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveUserYearBuilt rejected: AllowUpdateYearBuiltData is disabled");
                return BadRequest();
            }

            if (parameter?.CountyId is not int countyId || string.IsNullOrWhiteSpace(parameter.Reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveUserYearBuilt rejected: CountyId and Reference are required");
                return BadRequest();
            }

            string reference = parameter.Reference!;

            UserYearBuiltRemoveResult? userYearBuiltRemoveResult;
            try
            {
                // A building's data is filed under the part holding its building_2d row, which need not be the part the visitor's view names.
                HashSet<int>? countyIds = await PostgreSQL.Query.SiblingCountyIdsAsync(administrativeAreal2DPostgreSQLConverter, [countyId], cancellationToken: cancellationToken);
                if (countyIds is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "RemoveUserYearBuilt: the polygon parts of county {CountyId} could not be read", countyId);
                    return StatusCode(500, "The county parts could not be read.");
                }

                userYearBuiltRemoveResult = await yearBuiltDataPostgreSQLConverter.RemoveUserYearBuiltsAsync(countyIds, [reference], email, false, commandTimeout: 30, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(RemoveUserYearBuiltAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(RemoveUserYearBuiltAsync));
                return StatusCode(500, "Internal server error during user year built withdrawal");
            }

            if (userYearBuiltRemoveResult is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveUserYearBuilt: withdrawal for reference {Reference} of part {CountyId} failed and rolled back", reference, countyId);
                return StatusCode(500, "User year built withdrawal failed.");
            }

            if (userYearBuiltRemoveResult.NotOwnedReferences.Contains(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveUserYearBuilt rejected: the entry of reference {Reference} was not recorded by {User}", reference, email);
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (!userYearBuiltRemoveResult.RemovedReferences.Contains(reference))
            {
                Serilog.Modify.Log("RemoveUserYearBuilt: reference {Reference} of part {CountyId} holds no user entry", reference, countyId);
                return NotFound();
            }

            Serilog.Modify.Log("RemoveUserYearBuilt withdrew the entry of reference {Reference} of part {CountyId} recorded by {User}", reference, countyId, email);

            await RefreshBuildingDataYearBuiltAsync(countyId, [reference], cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Deletes stored year built data objects of the given county parts - by default only the objects holding no entry at all.
        /// <para>Gated by the access key and by <see cref="GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData"/>, a flag of its own because a delete has no undo; either gate failing answers 401. The scope is always the named county parts - normally every polygon part of one county - narrowed to the references in the body when one is sent. Without a body only <c>emptyonly=true</c> is accepted, so the widest request this action takes deletes the empty objects of a county.</para>
        /// <para><c>dryrun</c> defaults to true: the rows are selected and counted, nothing is deleted. A request matching more rows than <c>limit</c> deletes nothing and answers 413 carrying the counts, so a mistaken scope is refused whole rather than cut short. The result is a <see cref="YearBuiltDataRemoveResult"/>.</para>
        /// </summary>
        /// <param name="references">The references of the buildings whose objects are deleted, or none for every building of the parts (only with <paramref name="emptyOnly"/>).</param>
        /// <param name="countyIds">The county parts to delete from, as a repeated <c>countyids</c> parameter. Normally every polygon part of one county.</param>
        /// <param name="emptyOnly">A value indicating whether only objects holding no entry are deleted. Defaults to true.</param>
        /// <param name="dryRun">A value indicating whether the rows are only counted. Defaults to true.</param>
        /// <param name="limit">The largest number of rows the request may delete, from 1 to 10000. Defaults to 10000.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the counts, 413 with the counts when the scope exceeds <paramref name="limit"/>, 401 without a valid key or with the flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the delete failed.</returns>
        [HttpPost("removeitemsbycountyids")]
        [ProducesResponseType(typeof(YearBuiltDataRemoveResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(YearBuiltDataRemoveResult), StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveItemsByCountyIdsAsync([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] IEnumerable<string>? references, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromQuery(Name = "emptyonly")] bool emptyOnly = true, [FromQuery(Name = "dryrun")] bool dryRun = true, [Minimum(1), FromQuery(Name = "limit")] int limit = referenceCount_Maximum, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(RemoveItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}, EmptyOnly: {EmptyOnly}, DryRun: {DryRun}, Limit: {Limit}", countyIds is null ? string.Empty : string.Join(", ", countyIds), emptyOnly, dryRun, limit);

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData delete not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "YearBuiltData delete not allowed (AllowDeleteYearBuiltData is disabled)");
                return Unauthorized();
            }

            string[]? references_Array = references is null ? null : [.. references];
            if (!IsValid(countyIds, references_Array, limit, commandTimeout, out IActionResult? actionResult))
            {
                return actionResult!;
            }

            if ((references_Array is null || references_Array.Length == 0) && !emptyOnly)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Without references only empty objects can be deleted");
                return BadRequest("Without references only empty objects (emptyonly=true) can be deleted.");
            }

            try
            {
                YearBuiltDataRemoveResult? yearBuiltDataRemoveResult = await yearBuiltDataPostgreSQLConverter.RemoveItemsAsync(countyIds, references_Array is null || references_Array.Length == 0 ? null : references_Array, emptyOnly, dryRun, limit, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (yearBuiltDataRemoveResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "YearBuiltData delete could not run");
                    return StatusCode(500, "Year built data delete could not run.");
                }

                Serilog.Modify.Log("YearBuiltData delete ended. Matched: {Matched}, Removed: {Removed}, Unmatched references: {Unmatched}, DryRun: {DryRun}", yearBuiltDataRemoveResult.Matched, yearBuiltDataRemoveResult.Removed, yearBuiltDataRemoveResult.UnmatchedReferences.Count, dryRun);

                return Result(yearBuiltDataRemoveResult, !dryRun && yearBuiltDataRemoveResult.Matched > limit);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(RemoveItemsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(RemoveItemsByCountyIdsAsync));
                return StatusCode(500, "Internal server error during year built data delete");
            }
        }

        /// <summary>
        /// Removes one prediction run - the predicted year built entries stamped <paramref name="ticks"/> - from the stored year built data objects of the given county parts, in one transaction, and optionally recomputes the derived <c>building_data</c> columns of the buildings it changed.
        /// <para>Gated by the access key and by <see cref="GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData"/>; with <c>updatebuildingdata=true</c> also by <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData"/>, checked before anything is written. Any gate failing answers 401.</para>
        /// <para>The stamp is the run&apos;s <see cref="DateTime.Ticks"/> - the key each entry is stored under, as <c>predictedyearbuiltruns</c> reports it - never a formatted date. Other stamps and user entries are untouched; an object left with no entry is kept and listed in <see cref="PredictedYearBuiltRemoveResult.EmptiedReferences"/> for an explicit <c>removeitemsbycountyids</c>. <c>dryrun</c> defaults to true, and a run on more objects than <c>limit</c> answers 413 and writes nothing. Removing a run already removed matches nothing, so a retry is harmless.</para>
        /// </summary>
        /// <param name="references">The references of the buildings to remove the run from, or none for every building of the parts.</param>
        /// <param name="countyIds">The county parts to remove the run from, as a repeated <c>countyids</c> parameter. Normally every polygon part of one county.</param>
        /// <param name="ticks">The stamp of the run, as <see cref="DateTime.Ticks"/>. Required.</param>
        /// <param name="dryRun">A value indicating whether the entries are only counted. Defaults to true.</param>
        /// <param name="limit">The largest number of objects the request may rewrite, from 1 to 10000. Defaults to 10000.</param>
        /// <param name="updateBuildingData">A value indicating whether the derived <c>building_data</c> columns of the changed buildings are recomputed after the removal commits. Defaults to false.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a <see cref="PredictedYearBuiltRemoveResult"/>, 413 with it when the run exceeds <paramref name="limit"/>, 401 without a valid key or with a flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the removal - or the recompute after it - failed.</returns>
        [HttpPost("removepredictedyearbuiltsbycountyids")]
        [ProducesResponseType(typeof(PredictedYearBuiltRemoveResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PredictedYearBuiltRemoveResult), StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemovePredictedYearBuiltsByCountyIdsAsync([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] IEnumerable<string>? references, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromQuery(Name = "ticks")] long? ticks, [FromQuery(Name = "dryrun")] bool dryRun = true, [Minimum(1), FromQuery(Name = "limit")] int limit = referenceCount_Maximum, [FromQuery(Name = "updatebuildingdata")] bool updateBuildingData = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(RemovePredictedYearBuiltsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}, Ticks: {Ticks}, DryRun: {DryRun}, Limit: {Limit}, UpdateBuildingData: {UpdateBuildingData}", countyIds is null ? string.Empty : string.Join(", ", countyIds), ticks?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty, dryRun, limit, updateBuildingData);

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "PredictedYearBuilt removal not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "PredictedYearBuilt removal not allowed (AllowDeleteYearBuiltData is disabled)");
                return Unauthorized();
            }

            if (updateBuildingData && !GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "PredictedYearBuilt removal with building data recompute not allowed (AllowUpdateBuildingData is disabled)");
                return Unauthorized();
            }

            string[]? references_Array = references is null ? null : [.. references];
            if (!IsValid(countyIds, references_Array, limit, commandTimeout, out IActionResult? actionResult))
            {
                return actionResult!;
            }

            if (ticks is null || ticks.Value < DateTime.MinValue.Ticks || ticks.Value > DateTime.MaxValue.Ticks)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Ticks has to name a DateTime");
                return BadRequest("ticks is required and has to be a DateTime.Ticks value.");
            }

            try
            {
                PredictedYearBuiltRemoveResult? predictedYearBuiltRemoveResult = await yearBuiltDataPostgreSQLConverter.RemovePredictedYearBuiltsAsync(countyIds, new DateTime(ticks.Value), references_Array is null || references_Array.Length == 0 ? null : references_Array, dryRun, limit, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (predictedYearBuiltRemoveResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "PredictedYearBuilt removal could not run or rolled back");
                    return StatusCode(500, "Prediction run removal could not run.");
                }

                Serilog.Modify.Log("PredictedYearBuilt removal ended. Matched: {Matched}, Removed: {Removed}, Emptied: {Emptied}, DryRun: {DryRun}", predictedYearBuiltRemoveResult.Matched, predictedYearBuiltRemoveResult.Removed, predictedYearBuiltRemoveResult.EmptiedReferences.Count, dryRun);

                if (updateBuildingData && predictedYearBuiltRemoveResult.Removed > 0)
                {
                    BuildingDataYearBuiltUpdateResult? buildingDataYearBuiltUpdateResult = await PostgreSQL.Modify.UpdateBuildingDataYearBuiltAsync(buildingDataPostgreSQLConverter, yearBuiltDataPostgreSQLConverter, building2DPostgreSQLConverter, countyIds, predictedYearBuiltRemoveResult.References, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                    if (buildingDataYearBuiltUpdateResult is null)
                    {
                        // The removal is committed; a retry matches nothing, so the recompute has to be asked for on its own.
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "PredictedYearBuilt run removed from {Count} objects but the building data recompute failed", predictedYearBuiltRemoveResult.Removed);
                        return StatusCode(500, $"The run was removed from {predictedYearBuiltRemoveResult.Removed} objects, but the building data recompute failed. Call updatebuildingdatabycountyids for the same county parts.");
                    }

                    Serilog.Modify.Log("Building data year built recomputed. Matched: {Matched}, Updated: {Updated}, Cleared: {Cleared}", buildingDataYearBuiltUpdateResult.Matched, buildingDataYearBuiltUpdateResult.Updated, buildingDataYearBuiltUpdateResult.Cleared);
                }

                return Result(predictedYearBuiltRemoveResult, !dryRun && predictedYearBuiltRemoveResult.Matched > limit);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(RemovePredictedYearBuiltsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(RemovePredictedYearBuiltsByCountyIdsAsync));
                return StatusCode(500, "Internal server error during prediction run removal");
            }
        }

        /// <summary>
        /// Withdraws the user-provided year built entry of the given buildings, whoever recorded it - the moderation counterpart of <see cref="RemoveUserYearBuiltAsync"/>.
        /// <para>Gated by the access key and by <see cref="GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData"/>; either failing answers 401. The references are required, and are looked for under the named county parts only. <c>dryrun</c> defaults to true. The objects are kept. After a committed withdrawal the derived <c>building_data</c> columns of the withdrawn buildings are recomputed best-effort when <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData"/> allows it.</para>
        /// </summary>
        /// <param name="references">The references of the buildings to withdraw the user entry of. Required.</param>
        /// <param name="countyIds">The county parts the buildings are stored under, as a repeated <c>countyids</c> parameter. Normally every polygon part of one county.</param>
        /// <param name="dryRun">A value indicating whether the buildings are only classified. Defaults to true.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a <see cref="UserYearBuiltRemoveResult"/>, 401 without a valid key or with the flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the withdrawal failed.</returns>
        [HttpPost("removeuseryearbuiltsbycountyids")]
        [ProducesResponseType(typeof(UserYearBuiltRemoveResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUserYearBuiltsByCountyIdsAsync([FromBody] IEnumerable<string>? references, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromQuery(Name = "dryrun")] bool dryRun = true, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(RemoveUserYearBuiltsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}, DryRun: {DryRun}", countyIds is null ? string.Empty : string.Join(", ", countyIds), dryRun);

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "UserYearBuilt withdrawal not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "UserYearBuilt withdrawal not allowed (AllowDeleteYearBuiltData is disabled)");
                return Unauthorized();
            }

            string[]? references_Array = references is null ? null : [.. references];
            if (!IsValid(countyIds, references_Array, referenceCount_Maximum, commandTimeout, out IActionResult? actionResult))
            {
                return actionResult!;
            }

            if (references_Array is null || references_Array.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "At least one reference has to be provided");
                return BadRequest();
            }

            try
            {
                UserYearBuiltRemoveResult? userYearBuiltRemoveResult = await yearBuiltDataPostgreSQLConverter.RemoveUserYearBuiltsAsync(countyIds, references_Array, null, dryRun, commandTimeout, cancellationToken);
                if (userYearBuiltRemoveResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "UserYearBuilt withdrawal could not run or rolled back");
                    return StatusCode(500, "User year built withdrawal could not run.");
                }

                Serilog.Modify.Log("UserYearBuilt withdrawal ended. Removed: {Removed}, NotFound: {NotFound}, DryRun: {DryRun}", userYearBuiltRemoveResult.RemovedReferences.Count, userYearBuiltRemoveResult.NotFoundReferences.Count, dryRun);

                if (!dryRun && userYearBuiltRemoveResult.RemovedReferences.Count != 0)
                {
                    await RefreshBuildingDataYearBuiltAsync(countyIds!, userYearBuiltRemoveResult.RemovedReferences, cancellationToken);
                }

                return Content(Core.Convert.ToSystem_String(userYearBuiltRemoveResult) ?? string.Empty, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(RemoveUserYearBuiltsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(RemoveUserYearBuiltsByCountyIdsAsync));
                return StatusCode(500, "Internal server error during user year built withdrawal");
            }
        }

        /// <summary>
        /// Recomputes the derived year built columns of <c>building_data</c> - predicted, user and calculated - from the stored history of the given county parts, writing NULL where the history no longer holds a value.
        /// <para>Gated by the access key and by <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData"/>; either failing answers 401. With references in the body only those buildings are recomputed; without, every building holding a <c>year_built_data</c> or a <c>building_data</c> row under the parts. A building with neither a building data row nor a value is not written, so the call never adds empty rows. The result is a <see cref="BuildingDataYearBuiltUpdateResult"/>.</para>
        /// </summary>
        /// <param name="references">The references of the buildings to recompute, or none for every building of the parts.</param>
        /// <param name="countyIds">The county parts to recompute, as a repeated <c>countyids</c> parameter. Normally every polygon part of one county.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the counts, 401 without a valid key or with the flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the recompute failed.</returns>
        [HttpPost("updatebuildingdatabycountyids")]
        [ProducesResponseType(typeof(BuildingDataYearBuiltUpdateResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBuildingDataByCountyIdsAsync([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] IEnumerable<string>? references, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(UpdateBuildingDataByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "BuildingData year built recompute not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "BuildingData year built recompute not allowed (AllowUpdateBuildingData is disabled)");
                return Unauthorized();
            }

            string[]? references_Array = references is null ? null : [.. references];
            if (!IsValid(countyIds, references_Array, referenceCount_Maximum, commandTimeout, out IActionResult? actionResult))
            {
                return actionResult!;
            }

            try
            {
                BuildingDataYearBuiltUpdateResult? buildingDataYearBuiltUpdateResult = await PostgreSQL.Modify.UpdateBuildingDataYearBuiltAsync(buildingDataPostgreSQLConverter, yearBuiltDataPostgreSQLConverter, building2DPostgreSQLConverter, countyIds, references_Array is null || references_Array.Length == 0 ? null : references_Array, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (buildingDataYearBuiltUpdateResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "BuildingData year built recompute could not run");
                    return StatusCode(500, "Building data year built recompute could not run.");
                }

                Serilog.Modify.Log("BuildingData year built recompute ended. Matched: {Matched}, Updated: {Updated}, Cleared: {Cleared}", buildingDataYearBuiltUpdateResult.Matched, buildingDataYearBuiltUpdateResult.Updated, buildingDataYearBuiltUpdateResult.Cleared);

                return Content(Core.Convert.ToSystem_String(buildingDataYearBuiltUpdateResult) ?? string.Empty, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(UpdateBuildingDataByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(UpdateBuildingDataByCountyIdsAsync));
                return StatusCode(500, "Internal server error during building data year built recompute");
            }
        }

        /// <summary>
        /// Lists the prediction runs stored under the given county parts: one entry per part, stamp and model identifier, with the number of objects carrying it.
        /// <para>The stamp is reported as <see cref="DateTime.Ticks"/>, the value <c>removepredictedyearbuiltsbycountyids</c> takes, and the model identifier is null for entries written before it was recorded. A part holding no prediction answers <c>200 []</c>; a query that could not run answers 500, so "none" and "failed" are never confused.</para>
        /// </summary>
        /// <param name="countyIds">The county parts to list, as a repeated <c>countyids</c> parameter. Required.</param>
        /// <param name="commandTimeout">The timeout in seconds for the query. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a list of <see cref="PredictedYearBuiltRunResult"/>, 400 for invalid parameters, 503 on a transient database failure, or 500 when the query failed.</returns>
        [HttpGet("predictedyearbuiltruns", Name = $"{nameof(YearBuiltDataController)}_{nameof(GetPredictedYearBuiltRunsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PredictedYearBuiltRunResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPredictedYearBuiltRunsAsync([BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(GetPredictedYearBuiltRunsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!IsValid(countyIds, null, referenceCount_Maximum, commandTimeout, out IActionResult? actionResult))
            {
                return actionResult!;
            }

            try
            {
                List<PredictedYearBuiltRunResult>? predictedYearBuiltRunResults = await yearBuiltDataPostgreSQLConverter.GetPredictedYearBuiltRunsAsync(countyIds, commandTimeout, cancellationToken);
                if (predictedYearBuiltRunResults is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Prediction runs could not be read");
                    return StatusCode(500, "Prediction runs could not be read.");
                }

                return Content(Core.Convert.ToSystem_String(predictedYearBuiltRunResults) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(GetPredictedYearBuiltRunsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(GetPredictedYearBuiltRunsAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves items based on a provided reference and an optional county identifier.
        /// </summary>
        /// <param name="reference">The unique reference string used to identify the year built data items.</param>
        /// <param name="countyId">An optional integer representing the county ID to filter the results.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbyreference")]
        public async Task<IActionResult> GetItemsByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(GetItemsByReferenceAsync));

            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null or empty");
                return BadRequest();
            }

            if (yearBuiltDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "YearBuiltDataPostgreSQLConverter is null");
                return BadRequest();
            }

            List<YearBuiltData>? yearBuiltDatas_PostgreSQL = await yearBuiltDataPostgreSQLConverter.GetItemsByReferenceAsync(reference, countyId, null, true, cancellationToken: cancellationToken);

            if (yearBuiltDatas_PostgreSQL is null || yearBuiltDatas_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No YearBuiltDatas found for provided reference");
                return NoContent();
            }

            List<GIS.Interfaces.IYearBuiltData> yearBuiltDatas = [];
            foreach (YearBuiltData yearBuilt_PostgreSQL in yearBuiltDatas_PostgreSQL)
            {
                GIS.Interfaces.IYearBuiltData? yearBuiltData_GIS = yearBuilt_PostgreSQL.ToDiGi();
                if (yearBuiltData_GIS is null)
                {
                    continue;
                }

                yearBuiltDatas.Add(yearBuiltData_GIS);
            }

            if (yearBuiltDatas is null || yearBuiltDatas.Count == 0)
            {
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(yearBuiltDatas) ?? string.Empty, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the building references that carry stored year built data for a specified county identifier.
        /// </summary>
        /// <param name="countyId">The unique identifier of the county.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> containing a list of reference strings if found, or 404 if none are found.</returns>
        [HttpGet("referencesbycountyid", Name = $"{nameof(YearBuiltDataController)}_{nameof(GetReferencesByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReferencesByCountyIdAsync([BindRequired, Minimum(1), FromQuery(Name = "countyid")] int countyId, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId}", nameof(YearBuiltDataController), nameof(GetReferencesByCountyIdAsync), countyId);

            if (countyId <= 0 || commandTimeout < 0)
            {
                return BadRequest();
            }

            if (yearBuiltDataPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            HashSet<string>? references;
            try
            {
                references = await yearBuiltDataPostgreSQLConverter.GetReferencesAsync(countyId, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(GetReferencesByCountyIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(GetReferencesByCountyIdAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            if (references is null || references.Count == 0)
            {
                return NotFound();
            }

            JsonArray jsonArray = [.. references];
            string? json = jsonArray.ToJsonString();
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the number of year built data items stored for a specified county identifier.
        /// </summary>
        /// <param name="countyId">The unique identifier of the county.</param>
        /// <param name="estimated">A boolean value indicating whether to return an estimated count from table statistics rather than an exact count.</param>
        /// <param name="analyze">A boolean value indicating whether to perform an ANALYZE operation before reading the estimate.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the count, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county has no partition.</returns>
        [HttpGet("countbycountyid", Name = $"{nameof(YearBuiltDataController)}_{nameof(GetCountByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountByCountyIdAsync([BindRequired, Minimum(1), FromQuery(Name = "countyid")] int countyId, [FromQuery(Name = "estimated")] bool estimated = false, [FromQuery(Name = "analyze")] bool analyze = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId}", nameof(YearBuiltDataController), nameof(GetCountByCountyIdAsync), countyId);

            if (countyId <= 0 || commandTimeout < 0)
            {
                return BadRequest();
            }

            if (yearBuiltDataPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            long? count;
            try
            {
                count = estimated
                    ? await yearBuiltDataPostgreSQLConverter.GetEstimatedCountAsync(countyId, analyze, commandTimeout, cancellationToken)
                    : await yearBuiltDataPostgreSQLConverter.GetCountAsync(countyId, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(GetCountByCountyIdAsync));
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
                Serilog.Modify.Log("County {CountyId} has no year built data partition", countyId);
                return NotFound();
            }

            if (estimated && count < 0)
            {
                Serilog.Modify.Log("County {CountyId} year built data partition exists but has not been analysed", countyId);
                return NoContent();
            }

            return Content(count.Value.ToString(), "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves year built data items for each of the provided references and an optional county identifier.
        /// </summary>
        /// <param name="references">The collection of unique reference strings used to identify the year built data items.</param>
        /// <param name="countyId">An optional integer representing the county identifier used to filter the search.</param>
        /// <param name="fallbackByReference">A boolean value indicating whether to perform a fallback search by reference alone for any references not found in the initial search.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of year built data items or no content if none were found.</returns>
        [HttpPost("itemsbyreferences", Name = $"{nameof(YearBuiltDataController)}_{nameof(GetItemsByReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<GIS.Classes.YearBuiltData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemsByReferencesAsync([FromBody] IEnumerable<string>? references, [Minimum(1), FromQuery(Name = "countyid")] int? countyId = null, [FromQuery(Name = "fallbackbyreference")] bool fallbackByReference = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(GetItemsByReferencesAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (references is null || (countyId is not null && countyId <= 0) || commandTimeout < 0)
            {
                return BadRequest();
            }

            string[] references_Array = [.. references];
            if (references_Array.Length == 0)
            {
                return NoContent();
            }

            if (references_Array.Length > referenceCount_Maximum)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "At most {Maximum} references can be asked for in one request", referenceCount_Maximum);
                return BadRequest($"At most {referenceCount_Maximum} references can be asked for in one request.");
            }

            if (yearBuiltDataPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<YearBuiltData>? yearBuiltDatas_PostgreSQL;
            try
            {
                yearBuiltDatas_PostgreSQL = await yearBuiltDataPostgreSQLConverter.GetItemsByReferencesAsync(references_Array, countyId, null, fallbackByReference, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "YearBuiltDatas could not be read from {TableName} for CountyId {CountyId}", yearBuiltDataPostgreSQLConverter.TableName, countyId?.ToString() ?? string.Empty);
                return StatusCode(500, "Database read failed.");
            }

            if (yearBuiltDatas_PostgreSQL is null || yearBuiltDatas_PostgreSQL.Count == 0)
            {
                return NoContent();
            }

            List<GIS.Interfaces.IYearBuiltData> yearBuiltDatas = [];
            foreach (YearBuiltData yearBuilt_PostgreSQL in yearBuiltDatas_PostgreSQL)
            {
                GIS.Interfaces.IYearBuiltData? yearBuiltData_GIS = yearBuilt_PostgreSQL.ToDiGi();
                if (yearBuiltData_GIS is null)
                {
                    continue;
                }

                yearBuiltDatas.Add(yearBuiltData_GIS);
            }

            if (yearBuiltDatas.Count == 0)
            {
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(yearBuiltDatas) ?? string.Empty, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the building references that hold more than one year built data record, optionally filtered by county identifier, ordered by count descending.
        /// </summary>
        /// <param name="countyId">The optional integer identifier of the county to filter by; if null, searches across all counties.</param>
        /// <param name="limit">The maximum number of duplicate references to return. Defaults to 100.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of duplicate references - <c>200 []</c> when there are none - or 500 when the query could not run, so "clean" and "failed" are never the same answer.</returns>
        [HttpGet("referenceduplicates", Name = $"{nameof(YearBuiltDataController)}_{nameof(GetReferenceDuplicatesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Building2DReferenceDuplicate>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReferenceDuplicatesAsync([FromQuery(Name = "countyid")] int? countyId = null, [Minimum(1), FromQuery(Name = "limit")] int limit = 100, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(GetReferenceDuplicatesAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}, Limit provided: {Limit}", countyId?.ToString() ?? string.Empty, limit);

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

            if (yearBuiltDataPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "YearBuiltDataPostgreSQLConverter is null");
                return BadRequest();
            }

            try
            {
                // A 404 for "none" could not be told from a missing route, and a null from the converter - a query that
                // could not run - was answered the same way, so a clean result, a failure and an undeployed build all
                // looked alike (DiGi.GIS.WebAPI#49). None is an empty list; a failure is a 500.
                List<Building2DReferenceDuplicate>? building2DReferenceDuplicates = await yearBuiltDataPostgreSQLConverter.GetBuilding2DReferenceDuplicatesAsync(countyId, limit, commandTimeout, cancellationToken);
                if (building2DReferenceDuplicates is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Year built data reference duplicates could not be read");
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
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(GetReferenceDuplicatesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(GetReferenceDuplicatesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously reports, for every polygon part of a multi-part county, the year built data rows it holds whose reference <c>building_2d</c> does not hold under the same part.
        /// <para>A county code names one <c>administrative_areal_2d</c> row per polygon part, and a row here is filed under one of them. A row mismatches when <c>building_2d</c> holds its reference under a different part, or under none at all. The mismatches split into <see cref="Building2DReferencedObjectCountyPartMismatchResult.CountHeldElsewhere"/> - a reference <c>building_2d</c> holds under another part, so the repair has somewhere to put the row - and <see cref="Building2DReferencedObjectCountyPartMismatchResult.CountOrphan"/> - a reference <c>building_2d</c> holds under no part, so there is no destination and the gap is a missing building row.</para>
        /// <para>Only parts holding at least one mismatched row are returned, so a clean measurement is an empty list, and single-part codes are left out entirely because with one part there is nothing to be filed under by mistake.</para>
        /// </summary>
        /// <param name="code">An optional county code to restrict the measurement to. When omitted every multi-part code is measured.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token used to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying one entry per part holding a mismatched row - <c>200 []</c> when no measured part holds one - or 500 when the measurement could not run.</returns>
        [HttpGet("countypartmismatches", Name = $"{nameof(YearBuiltDataController)}_{nameof(GetCountyPartMismatchesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Building2DReferencedObjectCountyPartMismatchResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountyPartMismatchesAsync([FromQuery(Name = "code")] string? code = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(YearBuiltDataController), nameof(GetCountyPartMismatchesAsync));
            Serilog.Modify.Log("Code provided: {Code}, CommandTimeout provided: {CommandTimeout}", code ?? string.Empty, commandTimeout);

            if (commandTimeout < 0)
            {
                return BadRequest();
            }

            if (yearBuiltDataPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<Building2DReferencedObjectCountyPartMismatchResult>? building2DReferencedObjectCountyPartMismatchResults = await yearBuiltDataPostgreSQLConverter.GetCountyPartMismatchesAsync(code, commandTimeout, cancellationToken);
                if (building2DReferencedObjectCountyPartMismatchResults is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Year built data part mismatches could not be measured for {Code}", code ?? string.Empty);
                    return StatusCode(500, "County part mismatches could not be measured.");
                }

                if (building2DReferencedObjectCountyPartMismatchResults.Count == 0)
                {
                    Serilog.Modify.Log("No year built data part mismatch found for {Code}", code ?? string.Empty);
                }

                return Content(Core.Convert.ToSystem_String(building2DReferencedObjectCountyPartMismatchResults) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(YearBuiltDataController), nameof(GetCountyPartMismatchesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(YearBuiltDataController), nameof(GetCountyPartMismatchesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Checks the parameters the year built maintenance actions share: at least one county part, a non-negative command timeout, a limit from 1 to the reference maximum, and no more references than that maximum.
        /// </summary>
        /// <param name="countyIds">The county parts of the request.</param>
        /// <param name="references">The references of the request, or null when none was sent.</param>
        /// <param name="limit">The limit of the request.</param>
        /// <param name="commandTimeout">The command timeout of the request.</param>
        /// <param name="actionResult">When the parameters are refused, the 400 to answer with; otherwise, null.</param>
        /// <returns>True when the parameters are accepted; otherwise, false.</returns>
        private bool IsValid(int[]? countyIds, string[]? references, int limit, int commandTimeout, out IActionResult? actionResult)
        {
            actionResult = null;

            if (countyIds is null || countyIds.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null or empty");
                actionResult = BadRequest();
                return false;
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                actionResult = BadRequest();
                return false;
            }

            if (limit < 1 || limit > referenceCount_Maximum)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Limit has to be between 1 and {Maximum}", referenceCount_Maximum);
                actionResult = BadRequest($"limit has to be between 1 and {referenceCount_Maximum}.");
                return false;
            }

            if (references is not null && references.Length > referenceCount_Maximum)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "At most {Maximum} references can be sent in one request", referenceCount_Maximum);
                actionResult = BadRequest($"At most {referenceCount_Maximum} references can be sent in one request.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Answers a maintenance result as DiGi JSON: 200, or 413 when the request matched more than its limit and was therefore refused whole.
        /// </summary>
        /// <param name="serializableObject">The result to answer with.</param>
        /// <param name="exceeded">A value indicating whether the request exceeded its limit.</param>
        /// <returns>The <see cref="ContentResult"/> carrying the result.</returns>
        private ContentResult Result(Core.Interfaces.ISerializableObject serializableObject, bool exceeded)
        {
            ContentResult contentResult = Content(Core.Convert.ToSystem_String(serializableObject) ?? string.Empty, "application/json");
            if (exceeded)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Request refused: it matched more than its limit, nothing was written");
                contentResult.StatusCode = StatusCodes.Status413PayloadTooLarge;
            }

            return contentResult;
        }

        /// <summary>
        /// Recomputes the derived <c>building_data</c> year built columns of the given buildings after a committed user write, widening the given part to every polygon part of its county first. Best-effort - see <see cref="RefreshBuildingDataYearBuiltAsync(IEnumerable{int}, IEnumerable{string}, CancellationToken)"/>.
        /// </summary>
        /// <param name="countyId">The county part the request named.</param>
        /// <param name="references">The references of the buildings to recompute.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task RefreshBuildingDataYearBuiltAsync(int countyId, IEnumerable<string> references, CancellationToken cancellationToken)
        {
            HashSet<int>? countyIds;
            try
            {
                countyIds = await PostgreSQL.Query.SiblingCountyIdsAsync(administrativeAreal2DPostgreSQLConverter, [countyId], cancellationToken: cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                Serilog.Modify.Log(exception, "Building data year built recompute skipped: the polygon parts of county {CountyId} could not be read", countyId);
                return;
            }

            if (countyIds is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building data year built recompute skipped: the polygon parts of county {CountyId} could not be read", countyId);
                return;
            }

            await RefreshBuildingDataYearBuiltAsync(countyIds, references, cancellationToken);
        }

        /// <summary>
        /// Recomputes the derived <c>building_data</c> year built columns of the given buildings after a committed write to their history, so <c>Calculated year built</c> follows a user entry at once rather than at the next building data run.
        /// <para>Best-effort: the history write has already committed and stands, so a recompute that is not allowed (<see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData"/> disabled) or fails is logged and swallowed, and the next recompute or building data run catches the columns up.</para>
        /// </summary>
        /// <param name="countyIds">The county parts the buildings are stored under.</param>
        /// <param name="references">The references of the buildings to recompute.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task RefreshBuildingDataYearBuiltAsync(IEnumerable<int> countyIds, IEnumerable<string> references, CancellationToken cancellationToken)
        {
            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building data year built recompute skipped: AllowUpdateBuildingData is disabled");
                return;
            }

            try
            {
                BuildingDataYearBuiltUpdateResult? buildingDataYearBuiltUpdateResult = await PostgreSQL.Modify.UpdateBuildingDataYearBuiltAsync(buildingDataPostgreSQLConverter, yearBuiltDataPostgreSQLConverter, building2DPostgreSQLConverter, countyIds, references, commandTimeout: 30, cancellationToken: cancellationToken);
                if (buildingDataYearBuiltUpdateResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building data year built recompute could not run for {References}", string.Join(", ", references.Take(20)));
                    return;
                }

                Serilog.Modify.Log("Building data year built recomputed. Matched: {Matched}, Updated: {Updated}, Cleared: {Cleared}", buildingDataYearBuiltUpdateResult.Matched, buildingDataYearBuiltUpdateResult.Updated, buildingDataYearBuiltUpdateResult.Cleared);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                Serilog.Modify.Log(exception, "Building data year built recompute failed for {References}", string.Join(", ", references.Take(20)));
            }
        }
    }
}