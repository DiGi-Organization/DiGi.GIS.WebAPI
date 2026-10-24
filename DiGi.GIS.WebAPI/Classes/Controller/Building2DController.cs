using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.Classes;
using DiGi.GIS.PostgreSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Web API controller for building 2D operations, providing endpoints to retrieve, filter, and update building 2D data.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class Building2DController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter;
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        // This has to stay the ONLY public constructor. MVC activates controllers through
        // ActivatorUtilities.CreateFactory(type, Type.EmptyTypes), which matches every public
        // constructor when no explicit argument types are supplied and then throws
        // "Multiple constructors accepting all given argument types have been found in type ...".
        // A second, shorter convenience constructor therefore turns every endpoint on this
        // controller into an HTTP 500 before the action body ever runs (issue #6). Test code that
        // wants a partial controller passes the extra converters explicitly instead.
        /// <summary> Initializes a new instance of the <see cref="Building2DController"/> class. </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher for the GIS PostgreSQL Web API.</param>
        /// <param name="building2DPostgreSQLConverter">The converter used for Building 2D data operations in PostgreSQL.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used to resolve administrative area codes to county identifiers.</param>
        public Building2DController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <summary> Asynchronously counts the number of buildings based on the administrative areal 2D identifiers. </summary>
        /// <param name="countByAdministrativeAreal2DIdsParameter">The parameter object containing the collection of administrative areal 2D identifiers.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("count", Name = $"{nameof(Building2DController)}_{nameof(CountAsync)}")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CountAsync([FromBody] CountByAdministrativeAreal2DIdsParameter countByAdministrativeAreal2DIdsParameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(CountAsync));
            Serilog.Modify.Log("AdministrativeAreal2DIds count: {Count}", countByAdministrativeAreal2DIdsParameter?.AdministrativeAreal2DIds?.Count() ?? 0);

            if (countByAdministrativeAreal2DIdsParameter is null || countByAdministrativeAreal2DIdsParameter.AdministrativeAreal2DIds is null)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            long count = await building2DPostgreSQLConverter.CountAsync(countByAdministrativeAreal2DIdsParameter.AdministrativeAreal2DIds, cancellationToken: cancellationToken);
            if (count < 0)
            {
                return NotFound();
            }

            return Ok(count);
        }

        /// <summary>
        /// Asynchronously counts, for every polygon part of every multi-part county, the buildings it holds whose bounding box lies outside that part.
        /// <para>A county code names one <c>administrative_areal_2d</c> row per polygon part, and a building filed under the wrong one is unreachable through every read that filters on <c>county_id</c>. A building whose stored box does not intersect the box of the part holding it cannot be inside that part, so what this counts is certainly misfiled - and it costs no geometry.</para>
        /// <para>The count is a lower bound, not a total: a building inside the box of its part but outside the polygon needs the polygon to settle and is not counted. Read it as the number of rows known to be wrong, before and after a repair.</para>
        /// </summary>
        /// <param name="code">An optional county code to restrict the measurement to. When omitted every multi-part code is measured.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, returning one entry per county part holding buildings.</returns>
        [HttpGet("countypartmismatches", Name = $"{nameof(Building2DController)}_{nameof(GetCountyPartMismatchesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.Building2DCountyPartMismatchResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountyPartMismatchesAsync([FromQuery(Name = "code")] string? code = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetCountyPartMismatchesAsync));
            Serilog.Modify.Log("Code provided: {Code}, CommandTimeout provided: {CommandTimeout}", code ?? string.Empty, commandTimeout);

            if (commandTimeout < 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DCountyPartMismatchResult>? building2DCountyPartMismatchResults = await building2DPostgreSQLConverter.GetCountyPartMismatchesAsync(code, commandTimeout, cancellationToken);
            if (building2DCountyPartMismatchResults is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2DCountyPartMismatchResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously returns the estimated number of buildings held by each of the named county parts.
        /// <para>Read from the planner's row estimate of each county's <c>building_2d</c> partition, not counted: the whole country answers in one catalogue query, where an exact count reads every partition. The estimate is as fresh as the partition's last analyse, which is close enough for a coarse share such as a coverage marker and not for a figure anyone reconciles against.</para>
        /// <para>The values come back in the order the identifiers were given, one per identifier, so a caller pairs them up by position. A value is <c>null</c> where the part has no partition or the partition has never been analysed - an unknown, never a zero.</para>
        /// </summary>
        /// <param name="countyIds">The county part identifiers, as a repeated <c>countyids</c> parameter. At least one and at most <see cref="Constants.Building2D.MaximumEstimatedCountCountyCount"/>.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with one estimate or <c>null</c> per identifier, 400 for invalid parameters, 503 on a transient database failure, or 500 when the estimates could not be read.</returns>
        [HttpGet("estimatedcountsbycountyids", Name = $"{nameof(Building2DController)}_{nameof(GetEstimatedCountsByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<long?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEstimatedCountsByCountyIdsAsync([BindRequired, FromQuery(Name = "countyids")] List<int>? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {CountyCount} counties", nameof(Building2DController), nameof(GetEstimatedCountsByCountyIdsAsync), countyIds?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (countyIds is null || countyIds.Count == 0 || countyIds.Count > Constants.Building2D.MaximumEstimatedCountCountyCount || countyIds.Any(x => x <= 0))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds must hold between 1 and {Maximum} positive identifiers", Constants.Building2D.MaximumEstimatedCountCountyCount);
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            Dictionary<int, long>? counts = await building2DPostgreSQLConverter.GetEstimatedCountsAsync(countyIds, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
            if (counts is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Estimated Building2D counts could not be read");
                return StatusCode(500, "Estimated Building2D counts could not be read.");
            }

            // A never-analysed partition reports a negative estimate; that is an unknown, so it is answered null rather than as a count.
            List<long?> result = [];
            foreach (int countyId in countyIds)
            {
                result.Add(counts.TryGetValue(countyId, out long count) && count >= 0 ? count : null);
            }

            return Content(System.Text.Json.JsonSerializer.Serialize(result), "application/json");
        }

        /// <summary> Asynchronously retrieves a building 2D reference by its unique identifier and an optional county identifier. </summary>
        /// <param name="id">The unique identifier of the building.</param>
        /// <param name="countyId">An optional integer representing the county identifier used to filter the search.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("building2Dreferencebyid", Name = $"{nameof(Building2DController)}_{nameof(GetBuilding2DReferenceByIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(PostgreSQL.Classes.Building2DReference), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBuilding2DReferenceByIdAsync([BindRequired, FromQuery(Name = "id")] long id, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetBuilding2DReferenceByIdAsync));
            Serilog.Modify.Log("Id provided: {Id}", id);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (id <= 0 || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2DReference? building2DReference = await building2DPostgreSQLConverter.GetBuilding2DReferenceByIdAsync(id, countyId, cancellationToken: cancellationToken);
            if (building2DReference is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2DReference);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves a building 2D reference by its unique reference code and an optional county identifier. </summary>
        /// <param name="reference">The unique reference string of the building to retrieve.</param>
        /// <param name="countyId">The optional integer identifier of the county used to filter the search.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("building2Dreferencebyreference", Name = $"{nameof(Building2DController)}_{nameof(GetBuilding2DReferenceByReferenceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(PostgreSQL.Classes.Building2DReference), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBuilding2DReferenceByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetBuilding2DReferenceByReferenceAsync));
            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference) || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2DReference? building2DReference = await building2DPostgreSQLConverter.GetBuilding2DReferenceByReferenceAsync(reference, countyId, cancellationToken: cancellationToken);
            if (building2DReference is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2DReference);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Retrieves building 2D references filtered by administrative area 2D identifier. Can be used for relatively small number of buildings.
        /// <para>An area above the subdivision level resolves through its <b>Subdivision children</b> (<c>building_2d.subdivision_id</c> membership); a <b>Subdivision</b> resolves through its <b>polygon</b> - the buildings whose bounding-box centre it contains - because the subdivision layer nests (a city, its districts and their neighbourhoods are all subdivisions of one municipality) and <c>subdivision_id</c> files each building under a single container, so membership cannot name a district's buildings (DiGi.GIS.PostgreSQL#75). Parity with <c>point2dsbyadministrativeareal2Did</c>.</para>
        /// </summary>
        /// <param name="administrativeAreal2DId">The unique identifier of the administrative area 2D used to filter the building references.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("building2Dreferencesbyadministrativeareal2Did", Name = $"{nameof(Building2DController)}_{nameof(GetBuilding2DReferencesByAdministrativeAreal2DIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.Building2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBuilding2DReferencesByAdministrativeAreal2DIdAsync([BindRequired, FromQuery(Name = "administrativeareal2Did")] int administrativeAreal2DId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetBuilding2DReferencesByAdministrativeAreal2DIdAsync));
            Serilog.Modify.Log("AdministrativeAreal2DId provided: {AdministrativeAreal2DId}", administrativeAreal2DId);

            if (administrativeAreal2DId <= 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DReference>? building2DReferences = await building2DPostgreSQLConverter.GetBuilding2DReferencesByAdministrativeAreal2DIdsAsync([administrativeAreal2DId], cancellationToken: cancellationToken);
            string? json = Core.Convert.ToSystem_String(building2DReferences);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Retrieves the bounding-box centres of every building of the given administrative area, keyed by reference and county partition, for fast 2D dot rendering.
        /// <para>Area resolution (parity with <c>building2Dreferencesbyadministrativeareal2Did</c>): an area above the subdivision level goes through its <b>Subdivision children</b>, not geometry - one with no subdivisions answers 200 with an empty array, which is not the same as "the area holds no buildings"; it means the resolution found no subdivision to filter by. A <b>Subdivision</b> goes through its <b>polygon</b> instead - the buildings whose bounding-box centre it contains - because the subdivision layer nests and <c>subdivision_id</c> membership cannot name a district's buildings (DiGi.GIS.PostgreSQL#75); for it, an empty array does mean the polygon holds no building centre. A <see cref="StatusCodes.Status404NotFound"/> means the area lookup itself failed, not that the area is empty. Rows whose bounding box is NULL are skipped. The JSONB geometry column is never read on either path.</para>
        /// <para>A cold partition can exceed the default timeout (precedent: issue #27) - retry once with a higher <c>commandtimeout</c> before treating a timeout as a defect.</para>
        /// </summary>
        /// <param name="administrativeAreal2DId">The unique identifier of the administrative area 2D used to filter the building centroids.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("point2dsbyadministrativeareal2Did", Name = $"{nameof(Building2DController)}_{nameof(GetPoint2DsByAdministrativeAreal2DIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.Building2DCentroid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPoint2DsByAdministrativeAreal2DIdAsync([BindRequired, Minimum(1), FromQuery(Name = "administrativeareal2Did")] int administrativeAreal2DId, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetPoint2DsByAdministrativeAreal2DIdAsync));
            Serilog.Modify.Log("AdministrativeAreal2DId provided: {AdministrativeAreal2DId}, CommandTimeout provided: {CommandTimeout}", administrativeAreal2DId, commandTimeout);

            if (administrativeAreal2DId <= 0)
            {
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DCentroid>? building2DCentroids = await building2DPostgreSQLConverter.GetBuilding2DCentroidsByAdministrativeAreal2DIdsAsync([administrativeAreal2DId], commandTimeout, cancellationToken: cancellationToken);
            string? json = Core.Convert.ToSystem_String(building2DCentroids);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Retrieves the bounding-box centres of every building of the given administrative area as one compact, columnar document: <c>{"References":[…],"CountyIds":[…],"X":[…],"Y":[…]}</c>, the i-th entry of every array describing the same building, coordinates rounded to 0.01 m.
        /// <para>The same buildings, area resolution and status codes as <c>point2dsbyadministrativeareal2Did</c> (see its remarks), without a <c>_type</c> and four property names per building. That answer relays as 15.9 MB for county 1465 (155 307 buildings), and its client only ever keeps these four values (DiGi.GIS.WebAPI.UI#29). The full endpoint stays for callers that deserialize <c>Building2DCentroid</c> objects.</para>
        /// <para>An area with no buildings answers 200 with four empty arrays; a failed area lookup answers 404. Rows without a reference or a county identifier are left out (<see cref="Convert.ToSystem_JsonObject(IEnumerable{PostgreSQL.Classes.Building2DCentroid}?)"/>).</para>
        /// </summary>
        /// <param name="administrativeAreal2DId">The unique identifier of the administrative area 2D whose buildings are returned.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("centroidsbyadministrativeareal2Did", Name = $"{nameof(Building2DController)}_{nameof(GetCentroidsByAdministrativeAreal2DIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCentroidsByAdministrativeAreal2DIdAsync([BindRequired, Minimum(1), FromQuery(Name = "administrativeareal2Did")] int administrativeAreal2DId, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetCentroidsByAdministrativeAreal2DIdAsync));
            Serilog.Modify.Log("AdministrativeAreal2DId provided: {AdministrativeAreal2DId}, CommandTimeout provided: {CommandTimeout}", administrativeAreal2DId, commandTimeout);

            if (administrativeAreal2DId <= 0)
            {
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DCentroid>? building2DCentroids = await building2DPostgreSQLConverter.GetBuilding2DCentroidsByAdministrativeAreal2DIdsAsync([administrativeAreal2DId], commandTimeout, cancellationToken: cancellationToken);

            System.Text.Json.Nodes.JsonObject? jsonObject = Convert.ToSystem_JsonObject(building2DCentroids);
            if (jsonObject is null)
            {
                return NotFound();
            }

            return Content(jsonObject.ToJsonString(), "application/json");
        }

        /// <summary>
        /// Retrieves a paginated list of building 2D references.
        /// </summary>
        /// <param name="building2DReferencesByPagingParameter">The parameter containing paging options, including county identifier, cursor, and page size.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of building 2D references.</returns>
        [HttpPost("building2Dreferencesbypagingparameter", Name = $"{nameof(Building2DController)}_{nameof(GetBuilding2DReferencesByPagingParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.Building2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBuilding2DReferencesByPagingParameterAsync([FromBody] Building2DReferencesByPagingParameter building2DReferencesByPagingParameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetBuilding2DReferencesByPagingParameterAsync));
            Serilog.Modify.Log("Paging parameter provided: CountyId={CountyId}, PageSize={PageSize}, Cursor={Cursor}", building2DReferencesByPagingParameter?.CountyId ?? 0, building2DReferencesByPagingParameter?.PageSize ?? 0, building2DReferencesByPagingParameter?.Cursor ?? string.Empty);

            if (building2DReferencesByPagingParameter is null || building2DReferencesByPagingParameter.CountyId <= 0 || building2DReferencesByPagingParameter.PageSize <= 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DReference>? building2DReferences = await building2DPostgreSQLConverter.GetBuilding2DReferencesByCountyIdAsync(
                building2DReferencesByPagingParameter.CountyId,
                building2DReferencesByPagingParameter.SubdivisionId,
                building2DReferencesByPagingParameter.Cursor,
                building2DReferencesByPagingParameter.PageSize,
                cancellationToken: cancellationToken);

            if (building2DReferences is null)
            {
                return NotFound();
            }

            // An exhausted page is a valid paging result, not a missing resource - return an empty array so callers can terminate the cursor loop without handling a 404.
            if (building2DReferences.Count == 0)
            {
                return Content(new JsonArray().ToJsonString(), "application/json");
            }

            string? json = Core.Convert.ToSystem_String(building2DReferences);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves a building 2D item by its identifier. </summary>
        /// <param name="id">The unique identifier of the building 2D item to retrieve.</param>
        /// <param name="countyId">The optional county identifier associated with the building.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itembyid", Name = $"{nameof(Building2DController)}_{nameof(GetItemByIdAsync)}")]
        [ProducesResponseType(typeof(Building2D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemByIdAsync([BindRequired, FromQuery(Name = "id")] long id, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemByIdAsync));
            Serilog.Modify.Log("Id provided: {Id}", id);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (id <= 0 || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2D? building2D = await building2DPostgreSQLConverter.GetBuilding2DByIdAsync(id, countyId, cancellationToken: cancellationToken);
            if (building2D is null)
            {
                return NotFound();
            }

            Building2D? building2D_DiGi = building2D.ToDiGi();
            if (building2D_DiGi is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2D_DiGi);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves a building 2D item at or near a specified point. </summary>
        /// <param name="x">The X coordinate of the search point.</param>
        /// <param name="y">The Y coordinate of the search point.</param>
        /// <param name="tolerance">The optional tolerance distance in meters to use when searching for the item near the specified point. If not provided, NaN, or non-positive, a default tolerance of 0.5 meters is used.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>An <see cref="IActionResult" /> containing the building 2D item if found, or an error response.</returns>
        [HttpGet("itembypoint", Name = $"{nameof(Building2DController)}_{nameof(GetItemByPointAsync)}")]
        [ProducesResponseType(typeof(Building2D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemByPointAsync([BindRequired, FromQuery(Name = "x")] double x, [BindRequired, FromQuery(Name = "y")] double y, [FromQuery(Name = "tolerance")] double? tolerance, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemByPointAsync));
            Serilog.Modify.Log("Coordinates provided: X={X}, Y={Y}", x, y);
            Serilog.Modify.Log("Tolerance provided: {Tolerance}", tolerance?.ToString() ?? string.Empty);

            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return BadRequest();
            }

            if (tolerance is null || double.IsNaN(tolerance.Value) || tolerance.Value <= 0)
            {
                tolerance = 0.5;
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2D? building2D_PostgreSQL = await building2DPostgreSQLConverter.GetBuilding2DByPoint2DAsync(new Point2D(x, y), tolerance.Value, cancellationToken: cancellationToken);
            if (building2D_PostgreSQL is null)
            {
                return NotFound();
            }

            if (building2D_PostgreSQL.ToDiGi() is not Building2D building2D)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2D);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Asynchronously retrieves a building 2D item by its reference code and an optional county identifier. </summary>
        /// <param name="reference">The unique reference string used to locate the building 2D item.</param>
        /// <param name="countyId">The optional identifier of the county associated with the building.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itembyreference", Name = $"{nameof(Building2DController)}_{nameof(GetItemByReferenceAsync)}")]
        [ProducesResponseType(typeof(Building2D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemByReferenceAsync));
            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference) || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2D? building2D = await building2DPostgreSQLConverter.GetBuilding2DByReferenceAsync(reference, countyId, cancellationToken: cancellationToken);
            if (building2D is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2D.ToDiGi());
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves building 2D items within a specified bounding box. </summary>
        /// <param name="x_1">The X-coordinate of the first corner of the bounding box.</param>
        /// <param name="y_1">The Y-coordinate of the first corner of the bounding box.</param>
        /// <param name="x_2">The X-coordinate of the second corner of the bounding box.</param>
        /// <param name="y_2">The Y-coordinate of the second corner of the bounding box.</param>
        /// <param name="tolerance">An optional tolerance value for the spatial query. If not provided or NaN, a default macro distance is used.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbyboundingbox", Name = $"{nameof(Building2DController)}_{nameof(GetItemsByBoundingBoxAsync)}")]
        [ProducesResponseType(typeof(List<Building2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [FromQuery(Name = "tolerance")] double? tolerance, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemsByBoundingBoxAsync));
            Serilog.Modify.Log("BoundingBox provided: X_1={X_1}, Y_1={Y_1}, X_2={X_2}, Y_2={Y_2}", x_1, y_1, x_2, y_2);
            Serilog.Modify.Log("Tolerance provided: {Tolerance}", tolerance?.ToString() ?? string.Empty);

            if (double.IsNaN(x_1) || double.IsNaN(y_1) || double.IsNaN(x_2) || double.IsNaN(y_2))
            {
                return BadRequest();
            }

            if (tolerance is null || double.IsNaN(tolerance.Value))
            {
                tolerance = Core.Constants.Tolerance.MacroDistance;
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2D>? building2Ds_PostgreSQL = await building2DPostgreSQLConverter.GetBuilding2DsByBoundingBox2DAsync(new BoundingBox2D(new Core.Classes.Range<double>(x_1, x_2), new Core.Classes.Range<double>(y_1, y_2)), tolerance.Value, cancellationToken: cancellationToken);
            if (building2Ds_PostgreSQL is null || building2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<Building2D> building2Ds = [];
            foreach (PostgreSQL.Classes.Building2D building2D_PostgreSQL in building2Ds_PostgreSQL)
            {
                Building2D? building2D = building2D_PostgreSQL.ToDiGi();
                if (building2D is null)
                {
                    continue;
                }

                building2Ds.Add(building2D);
            }

            if (building2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves building 2D items by their references. </summary>
        /// <param name="jsonArray">The JSON array containing the building 2D references to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("itemsbybuilding2Dreferences", Name = $"{nameof(Building2DController)}_{nameof(GetItemsByBuilding2DReferencesAsync)}")]
        [ProducesResponseType(typeof(List<Building2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByBuilding2DReferencesAsync([FromBody] JsonArray? jsonArray, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemsByBuilding2DReferencesAsync));
            Serilog.Modify.Log("Building2DReferences count: {Count}", jsonArray?.Count ?? 0);

            if (jsonArray is null || jsonArray.Count == 0)
            {
                return BadRequest("The provided JSON array is null or empty.");
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DReference>? building2DReferences =
                Core.Create.SerializableObjects<PostgreSQL.Classes.Building2DReference>(jsonArray);

            if (building2DReferences is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2DReferences could not be converted from json");
                return BadRequest("Building2DReferences could not be converted from JSON.");
            }

            List<PostgreSQL.Classes.Building2D>? building2Ds_PostgreSQL =
                await building2DPostgreSQLConverter.GetBuilding2DsByBuilding2DReferencesAsync(building2DReferences, cancellationToken: cancellationToken);

            if (building2Ds_PostgreSQL is null || building2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<Building2D> building2Ds = [];
            foreach (PostgreSQL.Classes.Building2D building2D_PostgreSQL in building2Ds_PostgreSQL)
            {
                Building2D? building2D = building2D_PostgreSQL.ToDiGi();
                if (building2D is null)
                {
                    continue;
                }

                building2Ds.Add(building2D);
            }

            if (building2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves building 2D items within a specified circle. </summary>
        /// <param name="x">The X-coordinate of the center of the circle.</param>
        /// <param name="y">The Y-coordinate of the center of the circle.</param>
        /// <param name="radius">The radius of the search circle, in metres. Capped at <see cref="Constants.Terrain.MaximumRadius"/>.</param>
        /// <param name="diameter">The diameter of the search circle, in metres, used only when <paramref name="radius"/> is absent. Capped at twice <see cref="Constants.Terrain.MaximumRadius"/>.</param>
        /// <param name="tolerance">The tolerance value to be applied to the search area.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbycircle", Name = $"{nameof(Building2DController)}_{nameof(GetItemsByCircleAsync)}")]
        [ProducesResponseType(typeof(List<Building2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByCircleAsync([BindRequired, FromQuery(Name = "x")] double x, [BindRequired, FromQuery(Name = "y")] double y, [Range(0.0, Constants.Terrain.MaximumRadius), FromQuery(Name = "radius")] double? radius, [Range(0.0, 2 * Constants.Terrain.MaximumRadius), FromQuery(Name = "diameter")] double? diameter, [FromQuery(Name = "tolerance")] double? tolerance, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemsByCircleAsync));
            Serilog.Modify.Log("Coordinates provided: X={X}, Y={Y}", x, y);
            Serilog.Modify.Log("Radius provided: {Radius}, Diameter provided: {Diameter}", radius?.ToString() ?? string.Empty, diameter?.ToString() ?? string.Empty);
            Serilog.Modify.Log("Tolerance provided: {Tolerance}", tolerance?.ToString() ?? string.Empty);

            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return BadRequest();
            }

            if ((radius is null || !radius.HasValue || double.IsNaN(radius.Value)) && (diameter is null || !diameter.HasValue || double.IsNaN(diameter.Value)))
            {
                return BadRequest();
            }

            double radius_Temp = double.NaN;
            if (radius is not null && !double.IsNaN(radius.Value))
            {
                radius_Temp = radius.Value;
            }

            if (double.IsNaN(radius_Temp))
            {
                if (diameter is not null && !double.IsNaN(diameter.Value))
                {
                    radius_Temp = diameter.Value / 2;
                }
            }

            if (double.IsNaN(radius_Temp) || radius_Temp <= 0)
            {
                return BadRequest();
            }

            if (tolerance is null || double.IsNaN(tolerance.Value))
            {
                tolerance = Core.Constants.Tolerance.MacroDistance;
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2D>? building2Ds_PostgreSQL = await building2DPostgreSQLConverter.GetBuilding2DsByCircle2DAsync(new Circle2D(new Point2D(x, y), radius_Temp), tolerance.Value, cancellationToken: cancellationToken);
            if (building2Ds_PostgreSQL is null || building2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<Building2D> building2Ds = [];
            foreach (PostgreSQL.Classes.Building2D building2D_PostgreSQL in building2Ds_PostgreSQL)
            {
                Building2D? building2D = building2D_PostgreSQL.ToDiGi();
                if (building2D is null)
                {
                    continue;
                }

                building2Ds.Add(building2D);
            }

            if (building2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Retrieves building 2D items for a specified county identifier.
        /// </summary>
        /// <param name="countyId">The unique identifier of the county.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the building 2D items as a JSON response, or a 404 status if no items are found.</returns>
        [HttpGet("itemsbycountyid", Name = $"{nameof(Building2DController)}_{nameof(GetItemsByCountyIdAsync)}")]
        [ProducesResponseType(typeof(List<Building2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByCountyIdAsync([BindRequired, FromQuery(Name = "countyid")] int countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemsByCountyIdAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId);

            if (countyId <= 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2D>? building2Ds = await building2DPostgreSQLConverter.GetBuilding2DsByCountyIdAsync(countyId, cancellationToken: cancellationToken);
            if (building2Ds is null)
            {
                return NotFound();
            }

            List<Building2D>? building2D_DiGi = building2Ds.ToDiGi();
            if (building2D_DiGi is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2D_DiGi);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves building 2D items for each of the provided references and an optional county identifier.
        /// </summary>
        /// <param name="references">The collection of unique reference strings used to identify the 2D buildings.</param>
        /// <param name="countyId">An optional integer representing the county ID to filter the results.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of building 2D items.</returns>
        [HttpPost("itemsbyreferences", Name = $"{nameof(Building2DController)}_{nameof(GetItemsByReferencesAsync)}")]
        [ProducesResponseType(typeof(List<Building2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByReferencesAsync([FromBody] IEnumerable<string>? references, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetItemsByReferencesAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (references is null || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DReference> building2DReferences = [];
            foreach (string reference in references)
            {
                if (string.IsNullOrWhiteSpace(reference))
                {
                    continue;
                }

                building2DReferences.Add(new PostgreSQL.Classes.Building2DReference { Reference = reference, CountyId = countyId });
            }

            if (building2DReferences.Count == 0)
            {
                return Content(new JsonArray().ToJsonString(), "application/json");
            }

            List<PostgreSQL.Classes.Building2D>? building2Ds_PostgreSQL =
                await building2DPostgreSQLConverter.GetBuilding2DsByBuilding2DReferencesAsync(building2DReferences, fallbackByReference: countyId is null, cancellationToken: cancellationToken);

            if (building2Ds_PostgreSQL is null || building2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<Building2D> building2Ds = [];
            foreach (PostgreSQL.Classes.Building2D building2D_PostgreSQL in building2Ds_PostgreSQL)
            {
                Building2D? building2D = building2D_PostgreSQL.ToDiGi();
                if (building2D is null)
                {
                    continue;
                }

                building2Ds.Add(building2D);
            }

            if (building2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }
        /// <summary> Retrieves Point2D coordinates by their references. </summary>
        /// <param name="references">A collection of reference strings used to identify the Point2D objects.</param>
        /// <param name="countyId">The optional identifier for the county associated with the coordinates.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("point2dsbyreferences", Name = $"{nameof(Building2DController)}_{nameof(GetPoint2DsByReferencesAsync)}")]
        [ProducesResponseType(typeof(List<Point2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPoint2DsByReferencesAsync([FromBody] IEnumerable<string>? references, [FromQuery(Name = "countyid")] int? countyId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetPoint2DsByReferencesAsync));
            Serilog.Modify.Log("References count: {Count}", references?.Count() ?? 0);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (references is null || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<Point2D>? point2Ds = await building2DPostgreSQLConverter.GetPoint2DsByReferencesAsync(references, countyId, cancellationToken: cancellationToken);
            if (point2Ds is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(point2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves duplicate building references that occur across multiple counties, ordered by collision count descending.
        /// </summary>
        /// <param name="limit">The maximum number of duplicate references to return. Defaults to 100.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of duplicate building references.</returns>
        [HttpGet("referenceduplicates", Name = $"{nameof(Building2DController)}_{nameof(GetReferenceDuplicatesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<PostgreSQL.Classes.Building2DReferenceDuplicate>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReferenceDuplicatesAsync([Minimum(1), FromQuery(Name = "limit")] int limit = 100, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetReferenceDuplicatesAsync));
            Serilog.Modify.Log("Limit provided: {Limit}, CommandTimeout provided: {CommandTimeout}", limit, commandTimeout);

            if (limit <= 0 || commandTimeout < 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<PostgreSQL.Classes.Building2DReferenceDuplicate>? duplicates = await building2DPostgreSQLConverter.GetDuplicateReferencesAsync(limit, commandTimeout, cancellationToken);
            if (duplicates is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(duplicates);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves references of the building2Ds filtered by county Id. </summary>
        /// <param name="countyId">The unique identifier of the county used to filter the building 2D references.</param>
        /// <param name="subdivisionId">The optional unique identifier of the subdivision used to further filter the building 2D references. When set, the buildings are read by the subdivision's polygon (every container that holds a building answers), not by the stored <c>subdivision_id</c> - the subdivision layer is nested, so a building filed under its neighbourhood is still returned for its district and city.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("referencesbycountyid", Name = $"{nameof(Building2DController)}_{nameof(GetReferencesByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReferencesByCountyIdAsync([BindRequired, Minimum(1), FromQuery(Name = "countyid")] int countyId, [Minimum(1), FromQuery(Name = "subdivisionid")] int? subdivisionId = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetReferencesByCountyIdAsync));
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId);
            Serilog.Modify.Log("SubdivisionId provided: {SubdivisionId}", subdivisionId?.ToString() ?? string.Empty);
            Serilog.Modify.Log("CommandTimeout provided: {CommandTimeout}", commandTimeout);

            if (countyId <= 0 || (subdivisionId is not null && subdivisionId <= 0) || commandTimeout < 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            // A subdivision is a nested layer - a building filed under its neighbourhood must still answer for its district and city, so the read goes by the subdivision's polygon, not by the stored subdivision_id column.
            List<PostgreSQL.Classes.Building2DReference>? building2DReferences = subdivisionId is int subdivisionId_Value
                ? await building2DPostgreSQLConverter.GetBuilding2DReferencesBySubdivisionIdAsync(subdivisionId_Value, commandTimeout: commandTimeout, cancellationToken: cancellationToken)
                : await building2DPostgreSQLConverter.GetBuilding2DReferencesByCountyIdAsync(countyId, null, excludedReferences: null, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
            if (building2DReferences is null || building2DReferences.Count == 0)
            {
                return NotFound();
            }

            List<string> references = [];
            foreach (PostgreSQL.Classes.Building2DReference building2DReference in building2DReferences)
            {
                if (string.IsNullOrWhiteSpace(building2DReference.Reference))
                {
                    continue;
                }
                references.Add(building2DReference.Reference);
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
        /// Asynchronously retrieves overall building reference uniqueness metrics across all partitions in the database.
        /// </summary>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, returning the building reference uniqueness summary.</returns>
        [HttpGet("referenceuniquenesssummary", Name = $"{nameof(Building2DController)}_{nameof(GetReferenceUniquenessSummaryAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(PostgreSQL.Classes.Building2DReferenceUniquenessSummary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReferenceUniquenessSummaryAsync([Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(GetReferenceUniquenessSummaryAsync));
            Serilog.Modify.Log("CommandTimeout provided: {CommandTimeout}", commandTimeout);

            if (commandTimeout < 0)
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2DReferenceUniquenessSummary? summary = await building2DPostgreSQLConverter.GetReferenceUniquenessSummaryAsync(commandTimeout, cancellationToken);
            if (summary is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(summary);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Updates a single building 2D item. </summary>
        /// <param name="jsonObject">The <see cref="T:System.Text.Json.Nodes.JsonObject" /> containing the data to update the building 2D item. This value can be null.</param>
        /// <param name="code">The code identifying the specific building 2D item to be updated. This value can be null.</param>
        /// <param name="countyId">The optional county identifier associated with the building.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("updateitem", Name = $"{nameof(Building2DController)}_{nameof(UpdateItemAsync)}")]
        [ProducesResponseType(typeof(UpdateItemsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemAsync([FromBody] JsonObject? jsonObject, [FromQuery(Name = "code")] string? code, [FromQuery(Name = "countyid")] int? countyId = null, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(UpdateItemAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString() ?? string.Empty);

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding2D)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D update not allowed");
                return Unauthorized();
            }

            if (jsonObject is null || (countyId is not null && countyId <= 0))
            {
                return BadRequest();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            Building2D? building2D = Core.Create.SerializableObject<Building2D>(jsonObject);
            if (building2D is null)
            {
                return BadRequest();
            }

            PostgreSQL.Classes.Building2D? building2D_PostgreSQL = building2D.ToPostgreSQL(code);
            if (building2D_PostgreSQL is null)
            {
                return BadRequest();
            }

            if (countyId is not null)
            {
                building2D_PostgreSQL.CountyId = countyId.Value;
            }

            PostgreSQL.Classes.PostgreSQLUpdateResult? postgreSQLUpdateResult = await building2DPostgreSQLConverter.UpdateAsync([building2D_PostgreSQL]);

            UpdateItemsResult? updateItemsResult = postgreSQLUpdateResult.UpdateItemsResult(1);

            if (updateItemsResult is null || updateItemsResult.Updated == 0)
            {
                UpdateItemsResult.Rejection? rejection = updateItemsResult?.Rejected.Count > 0 ? updateItemsResult.Rejected[0] : null;
                if (rejection is not null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D rejected before the database. Reference: {Reference}, reason: {Reason}", rejection.Reference ?? string.Empty, rejection.Reason);
                    return StatusCode(500, $"Building2D was rejected before the database: {rejection.Reason}.");
                }

                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no Building2Ds have been updated");
                return StatusCode(500, "Database update returned no modified Building2D IDs.");
            }

            return Ok(updateItemsResult);
        }

        /// <summary> Updates multiple building 2D items based on the provided JSON array and identification code. </summary>
        /// <param name="jsonArray">The JSON array containing the building 2D items to be updated.</param>
        /// <param name="code">The identification code required for the update operation.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("updateitems", Name = $"{nameof(Building2DController)}_{nameof(UpdateItemsAsync)}")]
        [ProducesResponseType(typeof(UpdateItemsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsAsync([FromBody] JsonArray? jsonArray, [FromQuery(Name = "code")] string? code, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(UpdateItemsAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding2D)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D update not allowed");
                return Unauthorized();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No Building2Ds to update");
                return NoContent();
            }

            if (!string.IsNullOrWhiteSpace(code) && administrativeAreal2DPostgreSQLConverter is not null)
            {
                HashSet<int>? countyIds = await administrativeAreal2DPostgreSQLConverter.GetIdsByCodeAsync(code, PostgreSQL.Enums.AdministrativeArealType.County, cancellationToken: cancellationToken);
                if (countyIds is not null && countyIds.Count > 0)
                {
                    int[] countyIds_Resolved = [.. countyIds.OrderBy(x => x)];
                    if (countyIds_Resolved.Length > 1)
                    {
                        Serilog.Modify.Log("County code '{Code}' matches {Count} rows ({CountyIds}) because the county has that many polygon parts. Each building is being filed under the part it belongs to", code, countyIds_Resolved.Length, string.Join(", ", countyIds_Resolved));
                    }

                    // The code goes on as well as the parts. Dropping it here left a multi-part county with
                    // neither a part nor a code on each building, so every one of them was resolved against
                    // every county whose extent reached it - one query per building, each returning whole
                    // county polygons.
                    return await UpdateItemsByCountyIdsAsync(jsonArray, countyIds_Resolved, code, key, cancellationToken);
                }
            }

            List<Building2D>? building2Ds = Core.Create.SerializableObjects<Building2D>(jsonArray);
            if (building2Ds is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2Ds could not be converted from json");
                return BadRequest();
            }

            Serilog.Modify.Log("Building2Ds conversion to PostgreSQL started. Building2Ds count: {Count}", building2Ds.Count);

            List<PostgreSQL.Classes.Building2D> building2Ds_PostgreSQL = [];
            foreach (Building2D building2D in building2Ds)
            {
                PostgreSQL.Classes.Building2D? building2D_PostgreSQL = building2D.ToPostgreSQL(code);
                if (building2D_PostgreSQL is null)
                {
                    continue;
                }

                building2Ds_PostgreSQL.Add(building2D_PostgreSQL);
            }

            if (building2Ds_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No Building2Ds PostgreSQL to update");
                return NoContent();
            }

            Serilog.Modify.Log("Building2Ds conversion to PostgreSQL ended. Building2Ds converted: {After}/{Before}", building2Ds_PostgreSQL.Count, building2Ds.Count);
            Serilog.Modify.Log("Updating to database starting");

            if (building2DPostgreSQLConverter is null)
            {
                return StatusCode(500, "Database update failed.");
            }

            PostgreSQL.Classes.PostgreSQLUpdateResult? postgreSQLUpdateResult = null;
            try
            {
                postgreSQLUpdateResult = await building2DPostgreSQLConverter.UpdateAsync(building2Ds_PostgreSQL);
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Database could not be updated");
                return StatusCode(500, "Database update failed.");
            }

            UpdateItemsResult? updateItemsResult = postgreSQLUpdateResult.UpdateItemsResult(building2Ds_PostgreSQL.Count);
            if (updateItemsResult is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database could not be attempted");
                return StatusCode(500, "Database update failed.");
            }

            if (updateItemsResult.Rejected.Count != 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2Ds rejected before the database: {Count}/{Total}. References: {References}", updateItemsResult.Rejected.Count, updateItemsResult.Sent, updateItemsResult.Rejected.RejectionSample());
            }

            if (updateItemsResult.Updated == 0)
            {
                if (updateItemsResult.Rejected.Count == updateItemsResult.Sent)
                {
                    return StatusCode(500, $"All {updateItemsResult.Sent} Building2Ds were rejected before the database; none could be filed under a county.");
                }

                return StatusCode(500, "Database update returned no modified Building2D IDs.");
            }

            Serilog.Modify.Log("Updating to database ended. Updated Building2Ds: {After}/{Before}, rejected: {Rejected}", updateItemsResult.Updated, updateItemsResult.Sent, updateItemsResult.Rejected.Count);

            return Ok(updateItemsResult);
        }

        /// <summary>
        /// Updates multiple building 2D items in the database for the given county rows.
        /// <para>Naming several parts leaves the part of each building to geometry, which is the only thing that can decide it. Passing <paramref name="code"/> as well does not change that answer - it narrows the candidates to the parts of that county before the geometry runs, and it is what the row stores in its own <c>code</c> column.</para>
        /// <para>Without it a building of a multi-part county is resolved against every county whose extent reaches it, which costs one query returning whole county polygons <b>per building</b> rather than one set of polygons per county. That is the difference between minutes and hours over a county of tens of thousands of buildings, so an importer that knows the code should send it.</para>
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the building 2D items to be updated.</param>
        /// <param name="countyIds">The identifiers of the county rows the buildings belong to. Normally every polygon part of one county.</param>
        /// <param name="code">The optional county code the buildings came from. It narrows the geometric decision and is stored on the row; the parts named in <paramref name="countyIds"/> still decide.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("updateitemsbycountyids", Name = $"{nameof(Building2DController)}_{nameof(UpdateItemsByCountyIdsAsync)}")]
        [ProducesResponseType(typeof(UpdateItemsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsByCountyIdsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromQuery(Name = "code")] string? code = null, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(Building2DController), nameof(UpdateItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding2D)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2D update not allowed");
                return Unauthorized();
            }

            if (countyIds is null || countyIds.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null or empty");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No Building2D data to update");
                return NoContent();
            }

            if (building2DPostgreSQLConverter is null)
            {
                return StatusCode(500, "Database update failed.");
            }

            try
            {
                List<int> countyIds_Candidate = [.. new HashSet<int>(countyIds).OrderBy(x => x)];

                List<Building2D>? building2Ds = Core.Create.SerializableObjects<Building2D>(jsonArray);
                if (building2Ds is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Building2Ds could not be converted from json");
                    return BadRequest();
                }

                Serilog.Modify.Log("Building2Ds conversion to PostgreSQL started. Building2Ds count: {Count}", building2Ds.Count);

                int? countyId_Single = countyIds_Candidate.Count == 1 ? countyIds_Candidate[0] : null;

                List<PostgreSQL.Classes.Building2D> building2Ds_PostgreSQL = [];
                foreach (Building2D building2D in building2Ds)
                {
                    PostgreSQL.Classes.Building2D? building2D_PostgreSQL = building2D.ToPostgreSQL(code);
                    if (building2D_PostgreSQL is null)
                    {
                        continue;
                    }

                    if (countyId_Single is not null)
                    {
                        building2D_PostgreSQL.CountyId = countyId_Single.Value;
                    }

                    building2Ds_PostgreSQL.Add(building2D_PostgreSQL);
                }

                if (building2Ds_PostgreSQL.Count == 0)
                {
                    Serilog.Modify.Log("No Building2Ds PostgreSQL to update");
                    return NoContent();
                }

                Serilog.Modify.Log("Updating to database starting");

                PostgreSQL.Classes.PostgreSQLUpdateResult? postgreSQLUpdateResult = await building2DPostgreSQLConverter.UpdateAsync(building2Ds_PostgreSQL);

                UpdateItemsResult? updateItemsResult = postgreSQLUpdateResult.UpdateItemsResult(building2Ds_PostgreSQL.Count);
                if (updateItemsResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database could not be attempted");
                    return StatusCode(500, "Database update failed.");
                }

                if (updateItemsResult.Rejected.Count != 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building2Ds rejected before the database: {Count}/{Total}. References: {References}", updateItemsResult.Rejected.Count, updateItemsResult.Sent, updateItemsResult.Rejected.RejectionSample());
                }

                if (updateItemsResult.Updated == 0)
                {
                    if (updateItemsResult.Rejected.Count == updateItemsResult.Sent)
                    {
                        return StatusCode(500, $"All {updateItemsResult.Sent} Building2Ds were rejected before the database; none could be filed under a county.");
                    }

                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no Building2Ds have been updated");
                    return StatusCode(500, "Database update returned no modified building IDs.");
                }

                Serilog.Modify.Log("Updating to database ended. Updated Building2Ds: {After}/{Before}, rejected: {Rejected}", updateItemsResult.Updated, updateItemsResult.Sent, updateItemsResult.Rejected.Count);
                return Ok(updateItemsResult);
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Unhandled error during Building2DController.UpdateItemsByCountyIdsAsync");
                return StatusCode(500, exception.Message);
            }
        }
    }
}