using DiGi.GIS.PostgreSQL.Classes;
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
    /// Web API controller for PRG address points: stores the address points of a county and searches buildings by address.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class AddressPointController : DiGi.WebAPI.Classes.WebAPIController
    {
        private const int limit_Maximum = 100;
        private const int limit_Minimum = 1;

        private readonly AddressPointPostgreSQLConverter addressPointPostgreSQLConverter;
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter;
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        // This has to stay the ONLY public constructor. MVC activates controllers through
        // ActivatorUtilities.CreateFactory(type, Type.EmptyTypes), which matches every public
        // constructor when no explicit argument types are supplied and then throws
        // "Multiple constructors accepting all given argument types have been found in type ...".
        // A second, shorter convenience constructor therefore turns every endpoint on this
        // controller into an HTTP 500 before the action body ever runs (issue #6).
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressPointController"/> class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher holding the access key and the <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint"/> flag.</param>
        /// <param name="addressPointPostgreSQLConverter">The converter of the address point table.</param>
        /// <param name="building2DPostgreSQLConverter">The converter the building footprints the points are matched to are read through.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter the county part polygons and the sibling parts of a county are read through.</param>
        public AddressPointController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, AddressPointPostgreSQLConverter addressPointPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.addressPointPostgreSQLConverter = addressPointPostgreSQLConverter;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <summary>
        /// Searches the buildings by address: a free-text street or locality name, tolerating typos, missing diacritics and partial names, optionally narrowed by house number and county part.
        /// <para>Answers the matched address points in rank order - similarity first, then an exact house number before a prefix match (<c>12</c> finds <c>12A</c> and <c>12/3</c>, not <c>120</c>). Each <see cref="GIS.Classes.AddressPointMatch"/> carries the reference and guid of its building together with the address point; points matched to no building are left out. Read the building geometry through <c>gis/building2d/itemsbyreferences</c>.</para>
        /// <para><paramref name="countyId"/> filters on one county <b>part</b>: a county with disconnected territory is stored as one row per part, so search such a county without the filter, or once per part.</para>
        /// </summary>
        /// <param name="text">The street or locality name to search for, as typed. Required.</param>
        /// <param name="houseNumber">The house number to narrow the result to, or null for any.</param>
        /// <param name="countyId">The county part id to narrow the result to, or null for every county.</param>
        /// <param name="limit">The maximum number of results, from 1 to 100. Out-of-range values are refused, not clamped.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the matches (an empty array when none), or 400 for a missing or empty text, a text that normalises to nothing, or an invalid limit or county id.</returns>
        [HttpGet("buildingsbyaddress", Name = $"{nameof(AddressPointController)}_{nameof(GetBuildingsByAddressAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<GIS.Classes.AddressPointMatch>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBuildingsByAddressAsync([BindRequired, FromQuery(Name = "text")] string? text, [FromQuery(Name = "housenumber")] string? houseNumber, [Minimum(1), FromQuery(Name = "countyid")] int? countyId, [Range(limit_Minimum, limit_Maximum), FromQuery(Name = "limit")] int? limit = 20, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AddressPointController), nameof(GetBuildingsByAddressAsync));
            Serilog.Modify.Log("Text provided: {Text}, HouseNumber provided: {HouseNumber}, CountyId provided: {CountyId}, Limit provided: {Limit}", text ?? string.Empty, houseNumber ?? string.Empty, countyId?.ToString() ?? string.Empty, limit?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest();
            }

            if (limit is null || limit.Value < limit_Minimum || limit.Value > limit_Maximum)
            {
                return BadRequest();
            }

            if (countyId is not null && countyId.Value <= 0)
            {
                return BadRequest();
            }

            if (addressPointPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<GIS.Classes.AddressPointMatch>? addressPointMatches = await addressPointPostgreSQLConverter.GetAddressPointMatchesByAddressAsync(text, houseNumber, countyId, limit.Value, cancellationToken: cancellationToken);
            if (addressPointMatches is null)
            {
                // The text held no letter or digit, so there is nothing to search for.
                return BadRequest();
            }

            List<GIS.Classes.AddressPointMatch> addressPointMatches_Matched = [.. addressPointMatches.Where(x => !string.IsNullOrWhiteSpace(x?.Reference))];
            if (addressPointMatches_Matched.Count == 0)
            {
                return Content(new JsonArray().ToJsonString(), "application/json");
            }

            string? json = Core.Convert.ToSystem_String(addressPointMatches_Matched);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Address point matches could not be serialized.");
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Retrieves the address points matched to one building: the reverse of <see cref="GetBuildingsByAddressAsync"/>.
        /// <para>A building may hold any number of address points; a building with none answers an empty array.</para>
        /// </summary>
        /// <param name="reference">The <c>building_2d</c> reference of the building. Required.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the matches (an empty array when none), 400 for a missing reference, or 500 when the lookup could not run.</returns>
        [HttpGet("addresspointsbyreference", Name = $"{nameof(AddressPointController)}_{nameof(GetAddressPointsByReferenceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<GIS.Classes.AddressPointMatch>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAddressPointsByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string? reference, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AddressPointController), nameof(GetAddressPointsByReferenceAsync));
            Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                return BadRequest();
            }

            if (addressPointPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<GIS.Classes.AddressPointMatch>? addressPointMatches = await addressPointPostgreSQLConverter.GetAddressPointMatchesByReferencesAsync([reference], cancellationToken: cancellationToken);
            if (addressPointMatches is null)
            {
                return StatusCode(500, "Address points could not be read.");
            }

            if (addressPointMatches.Count == 0)
            {
                return Content(new JsonArray().ToJsonString(), "application/json");
            }

            string? json = Core.Convert.ToSystem_String(addressPointMatches);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Address point matches could not be serialized.");
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Stores the PRG address points of one county: assigns every point to the county part it lies in, matches it to the nearest <c>building_2d</c> footprint of that part and writes the match.
        /// <para>The county part is resolved per point <b>by geometry</b>, never by taking a single id on trust (DiGi.GIS.WebAPI#25): the given ids are first widened to every polygon part of the counties they name, so a caller sending one part of a multi-part county does not lose the points lying in its siblings. A point inside none of the parts is counted as outside and not written.</para>
        /// <para>Answers 401 for a wrong key and, distinctly logged, for a disabled <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint"/> flag; 503 with <c>Retry-After</c> for a transient database failure.</para>
        /// </summary>
        /// <param name="jsonArray">The JSON array of <see cref="GIS.Classes.AddressPoint"/> items to store.</param>
        /// <param name="countyIds">The county part ids the points belong to, one occurrence per part. Normally every polygon part of one county.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the <see cref="AddressPointUpdateResult"/>, 204 for an empty body, 400 for invalid input, 401, 500 or 503.</returns>
        [HttpPost("updateitemsbycountyids", Name = $"{nameof(AddressPointController)}_{nameof(UpdateItemsByCountyIdsAsync)}")]
        [ProducesResponseType(typeof(AddressPointUpdateResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsByCountyIdsAsync([FromBody] JsonArray? jsonArray, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AddressPointController), nameof(UpdateItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AddressPoint update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AddressPoint update not allowed: {Flag} is not set to true", nameof(GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint));
                return Unauthorized();
            }

            if (countyIds is null || countyIds.Length == 0 || countyIds.Any(x => x <= 0))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null, empty or non-positive");
                return BadRequest();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                Serilog.Modify.Log("No AddressPoints to update");
                return NoContent();
            }

            if (addressPointPostgreSQLConverter is null || building2DPostgreSQLConverter is null || administrativeAreal2DPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "A PostgreSQL converter is null");
                return BadRequest();
            }

            try
            {
                List<GIS.Classes.AddressPoint>? addressPoints = Core.Create.SerializableObjects<GIS.Classes.AddressPoint>(jsonArray);
                if (addressPoints is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AddressPoints could not be converted from json");
                    return BadRequest();
                }

                // Naming one part is not evidence the county has one part: widen to every part of the
                // counties named, so a point lying in a sibling part is filed there instead of dropped.
                HashSet<int>? countyIds_Candidate = await PostgreSQL.Query.SiblingCountyIdsAsync(administrativeAreal2DPostgreSQLConverter, countyIds, cancellationToken: cancellationToken);
                if (countyIds_Candidate is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County parts could not be resolved: the administrative_areal_2d lookup could not run (check the Main database configuration)");
                    return StatusCode(500, "County parts could not be resolved: the administrative_areal_2d lookup could not run.");
                }

                if (countyIds_Candidate.Count > new HashSet<int>(countyIds).Count)
                {
                    Serilog.Modify.Log("CountyIds widened to every part of their counties: {CountyIds}", string.Join(", ", countyIds_Candidate.OrderBy(x => x)));
                }

                Serilog.Modify.Log("Updating to database starting. AddressPoints count: {Count}", addressPoints.Count);

                AddressPointUpdateResult? addressPointUpdateResult = await PostgreSQL.Modify.UpdateAddressPointsAsync(addressPointPostgreSQLConverter, building2DPostgreSQLConverter, administrativeAreal2DPostgreSQLConverter, addressPoints, countyIds_Candidate, cancellationToken: cancellationToken);
                if (addressPointUpdateResult is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but the address points could not be written");
                    return StatusCode(500, "Address points could not be written.");
                }

                Serilog.Modify.Log("Updating to database ended. Inside: {Inside}, Nearest: {Nearest}, Unmatched: {Unmatched}, Outside: {Outside}, Skipped: {Skipped}, Written: {Written}",
                    addressPointUpdateResult.Inside, addressPointUpdateResult.Nearest, addressPointUpdateResult.Unmatched, addressPointUpdateResult.Outside, addressPointUpdateResult.Skipped, addressPointUpdateResult.Written);

                string? json = Core.Convert.ToSystem_String(addressPointUpdateResult);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return StatusCode(500, "Address point update result could not be serialized.");
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(AddressPointController), nameof(UpdateItemsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Unhandled error during AddressPointController.UpdateItemsByCountyIdsAsync");
                return StatusCode(500, exception.Message);
            }
        }
    }
}
