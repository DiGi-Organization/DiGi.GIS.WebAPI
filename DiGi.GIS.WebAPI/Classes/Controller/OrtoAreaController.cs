using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Serves the full-interpretation test areas: the epoch list and the materialised imagery of an area, area creation with its server-drawn random cells, the polygons drawn in it, and the interpreter-stamped label rows (DiGi.GIS.WebAPI#74).
    /// <para>Every route sits behind the user session: the bearer token is resolved with <see cref="Query.GetUserEmail"/> and a missing or invalid one answers 401. The query parameter names (<c>x_1</c>, <c>y_1</c>, <c>x_2</c>, <c>y_2</c>, <c>year</c>) are a hand-matched wire contract consumed by DiGi.GIS.WebAPI.UI#84 - renaming one breaks the relay silently, because ASP.NET ignores an unknown parameter and answers with the unfiltered result.</para>
    /// <para>An area is named by <see cref="DiGi.GIS.Query.OrtoLocationKey(BoundingBox2D?)"/> of its box, so all six routes address the same area as long as the box's minimum corner matches; <c>areabyboundingbox</c> creates it on first sight, and every other route refuses a box whose area was never initialized with 400.</para>
    /// <para>The imagery of an epoch is fetched from the time-enabled WMS once per area and epoch and stored in <c>orto_area_image</c>: imagery (status 0) and the service's own "no imagery" answer (status 1) are both cached, while a transport failure (status 2) is not stored and answers 503 so the next request tries again. A blank (uniform) tile - the service's HTTP 200 answer for a flaky or missing epoch - is asked for again and, if it stays blank, is not stored either and answers 503; a stored status-0 row is re-checked before it is served, so a blank stored earlier is refetched rather than shown (DiGi.GIS.WebAPI#76).</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class OrtoAreaController : DiGi.WebAPI.Classes.WebAPIController
    {
        /// <summary>
        /// The image status of a stored row holding the fetched JPEG bytes.
        /// </summary>
        private const short imageStatus_Imagery = 0;

        /// <summary>
        /// The image status of a cached negative row: the WMS answered a service exception or other non-image content, so the epoch has no imagery here. Never refetched without deleting the row.
        /// </summary>
        private const short imageStatus_NoImagery = 1;

        /// <summary>
        /// The image status of a fetch that failed on the transport. This endpoint never stores such a row - the failure is not cached, so the next request retries.
        /// </summary>
        private const short imageStatus_FetchError = 2;

        // The host registers no IHttpClientFactory for this extension (Create.ServiceProvider builds one only for
        // its own outbound calls), so injecting a factory would fail controller activation and turn every action
        // into an HTTP 500 before its body runs. One static client owned by the controller class is used instead,
        // sized for a single WMS GetMap fetch per area and epoch.
        private static readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(60) };

        // The names a relayed polygon body may carry its point array under: the DiGi serializer's names and the
        // lower-cased forms a hand-built JSON body uses. The first name present decides the parse.
        private static readonly string[] polygonPointPropertyNames = ["point2Ds", "Point2Ds", "points", "Points"];

        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter;
        private readonly OrtoAreaImagePostgreSQLConverter ortoAreaImagePostgreSQLConverter;
        private readonly OpenBuildingPostgreSQLConverter? openBuildingPostgreSQLConverter;
        private readonly OrtoAreaLabelPostgreSQLConverter ortoAreaLabelPostgreSQLConverter;
        private readonly OrtoAreaObjectPostgreSQLConverter ortoAreaObjectPostgreSQLConverter;
        private readonly OrtoAreaPostgreSQLConverter ortoAreaPostgreSQLConverter;
        private readonly OrtoSheetPostgreSQLConverter ortoSheetPostgreSQLConverter;
        private readonly DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager;
        private readonly DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore;

        static OrtoAreaController()
        {
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            string applicationInfo = string.Format(CultureInfo.CurrentCulture, "{0}/{1}", assemblyName.Name ?? "DiGi.GIS.WebAPI", assemblyName.Version?.ToString() ?? "1.0.0.0");
            httpClient.DefaultRequestHeaders.Add("User-Agent", $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) {applicationInfo}");
        }

        // This has to stay the ONLY public constructor. MVC activates controllers through
        // ActivatorUtilities.CreateFactory(type, Type.EmptyTypes), which matches every public
        // constructor when no explicit argument types are supplied and then throws
        // "Multiple constructors accepting all given argument types have been found in type ...".
        // A second, shorter convenience constructor therefore turns every endpoint on this
        // controller into an HTTP 500 before the action body ever runs (issue #6). Test code that
        // wants a partial controller passes the extra converters explicitly instead.
        /// <summary> Initializes a new instance of the <see cref="OrtoAreaController"/> class. </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher for the GIS PostgreSQL Web API, holding the <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateOrtoAreaLabel"/> gate of the label write.</param>
        /// <param name="building2DPostgreSQLConverter">The converter for Building2D objects, used to resolve the footprint box a label's observation dates are read against (Main database).</param>
        /// <param name="ortoAreaImagePostgreSQLConverter">The converter for the materialised area imagery of one epoch (Storage database).</param>
        /// <param name="ortoAreaLabelPostgreSQLConverter">The converter for the interpretation label rows of an area (Main database).</param>
        /// <param name="ortoAreaObjectPostgreSQLConverter">The converter for the drawn polygon rows of an area (Main database).</param>
        /// <param name="ortoAreaPostgreSQLConverter">The converter for the test-area registry with its sampled cells (Main database).</param>
        /// <param name="ortoSheetPostgreSQLConverter">The converter for the orthophoto sheet index, used for the epoch list and the observation dates (Storage database).</param>
        /// <param name="securityKeyManager">The user extension&apos;s security key manager; <c>null</c> when the user extension is not loaded, in which case every user-token check denies.</param>
        /// <param name="tokenRevocationStore">The user extension&apos;s token revocation store; <c>null</c> when the user extension is not loaded, in which case every user-token check denies.</param>
        /// <param name="openBuildingPostgreSQLConverter">The converter reading the open building footprint tables for <c>validationsummary</c>; <c>null</c> when not registered, in which case a request naming datasets answers 400.</param>
        public OrtoAreaController(
            GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher,
            Building2DPostgreSQLConverter building2DPostgreSQLConverter,
            OrtoAreaImagePostgreSQLConverter ortoAreaImagePostgreSQLConverter,
            OrtoAreaLabelPostgreSQLConverter ortoAreaLabelPostgreSQLConverter,
            OrtoAreaObjectPostgreSQLConverter ortoAreaObjectPostgreSQLConverter,
            OrtoAreaPostgreSQLConverter ortoAreaPostgreSQLConverter,
            OrtoSheetPostgreSQLConverter ortoSheetPostgreSQLConverter,
            DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager = null,
            DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore = null,
            OpenBuildingPostgreSQLConverter? openBuildingPostgreSQLConverter = null)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.ortoAreaImagePostgreSQLConverter = ortoAreaImagePostgreSQLConverter;
            this.ortoAreaLabelPostgreSQLConverter = ortoAreaLabelPostgreSQLConverter;
            this.ortoAreaObjectPostgreSQLConverter = ortoAreaObjectPostgreSQLConverter;
            this.ortoAreaPostgreSQLConverter = ortoAreaPostgreSQLConverter;
            this.ortoSheetPostgreSQLConverter = ortoSheetPostgreSQLConverter;
            this.securityKeyManager = securityKeyManager;
            this.tokenRevocationStore = tokenRevocationStore;
            this.openBuildingPostgreSQLConverter = openBuildingPostgreSQLConverter;
        }

        /// <summary>
        /// Lists the imagery epochs of an area: the distinct years of the <c>orto_sheet</c> rows whose footprints intersect the bounding box, ascending.
        /// <para>The answer comes from the sheet index itself rather than from the availability matrix: an epoch a card can be built for is what the caller chooses from, and every sheet of the box contributes its year whatever its usability. A box no sheet covers answers 200 with an empty list - no coverage is a result, not a failure.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the distinct years ascending (an empty list when no sheet covers), 401 without a valid user token, 400 for an invalid bounding box, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("availableyearsbyboundingbox", Name = $"{nameof(OrtoAreaController)}_{nameof(AvailableYearsByBoundingBoxAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<short>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AvailableYearsByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(AvailableYearsByBoundingBoxAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AvailableYearsByBoundingBox rejected: no valid user token");
                return Unauthorized();
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(x_1, y_1, x_2, y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AvailableYearsByBoundingBox rejected: bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            List<GIS.Classes.OrtoSheet>? ortoSheets;
            try
            {
                ortoSheets = await ortoSheetPostgreSQLConverter.GetOrtoSheetsByBoundingBoxAsync(boundingBox2D, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(AvailableYearsByBoundingBoxAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(AvailableYearsByBoundingBoxAsync));
                return StatusCode(500, "Internal server error during orthophoto sheet read");
            }

            if (ortoSheets is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AvailableYearsByBoundingBox: the orthophoto sheet read returned no result; answering an empty epoch list");
                return Ok(new List<short>());
            }

            List<short> years = [.. ortoSheets.Select(x => (short)x.Year).Distinct().OrderBy(x => x)];

            Serilog.Modify.Log("AvailableYearsByBoundingBox answered {Count} epoch(s) for the box X_1={X_1}, Y_1={Y_1}, X_2={X_2}, Y_2={Y_2}", years.Count, x_1, y_1, x_2, y_2);

            return Ok(years);
        }

        /// <summary>
        /// Serves the materialised orthophoto imagery of an area for one epoch, fetching and storing it on first sight.
        /// <para>On a cache miss the WMS <c>GetMap</c> URL is built with <see cref="DiGi.GIS.Query.Url_OrtoImage(BoundingBox2D?, string?, string?, string?, int, int)"/> against <see cref="DiGi.GIS.Constants.WebService.StandardResolutionTime"/> (<c>TIME=year</c>, EPSG:2180 easting first). The response is classified by content, not by status: image bytes become a stored status-0 row, a service exception or other non-image body a stored status-1 negative row (never refetched without deleting the row), and a transport failure is not stored at all - it answers 503 and the next request retries. One fetch per area and epoch is the result, proven by the single-fetch fact.</para>
        /// <para>The tile size is <c>width = clamp(ceil(box.Width &#215; MaximumImagePixelSize &#247; MaximumAreaSide), MinimumImagePixelSize, MaximumImagePixelSize)</c> with the height following the box aspect ratio capped at the same maximum, which fixes the fetch near one metre per pixel. The scale denominator is verified against <see cref="DiGi.GIS.Constants.WebService.OrtoMaxScaleDenominator"/> before the fetch as defence in depth - under the maximum-area-side guard the formula keeps the scale near 3 500, far below the limit, so the 400 is unreachable in practice.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="year">The imagery epoch to serve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the image bytes, 401 without a valid user token, 400 for an invalid bounding box, 404 when the epoch has no imagery (cached negative row), 503 when the WMS fetch failed on the transport, the WMS kept answering a blank tile, or the database is transiently unavailable, or 500 when the read or store failed.</returns>
        [HttpGet("imagebyboundingbox", Name = $"{nameof(OrtoAreaController)}_{nameof(ImageByBoundingBoxAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        // No action-level [Produces]: it restricts every ObjectResult of the action to image/jpeg, so the string-bodied
        // 400/500/503 answers below had no formatter and left as 406 (DiGi.GIS.WebAPI#38). The photo content type is documented on the 200.
        [ProducesResponseType(typeof(FileContentResult), 200, "image/jpeg")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImageByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [BindRequired, FromQuery(Name = "year")] short year, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(ImageByBoundingBoxAsync));
            Serilog.Modify.Log("BoundingBox provided: X_1={X_1}, Y_1={Y_1}, X_2={X_2}, Y_2={Y_2}, Year={Year}", x_1, y_1, x_2, y_2, year);

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox rejected: no valid user token");
                return Unauthorized();
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(x_1, y_1, x_2, y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox rejected: bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            string? areaKey = boundingBox2D.OrtoLocationKey();
            if (string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox rejected: the area key could not be derived from the bounding box");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            try
            {
                // The stored row decides without asking the WMS: status 1 is the negative cache and never refetched,
                // status 2 is never stored by this endpoint, so a status-2 row found here can only be foreign data
                // and still reads as unavailable.
                OrtoAreaImage? ortoAreaImage = await ortoAreaImagePostgreSQLConverter.GetOrtoAreaImageAsync(areaKey, year, cancellationToken: cancellationToken);

                if (ortoAreaImage is not null)
                {
                    // A stored status-0 row is re-checked before it is served: a blank tile stored before
                    // DiGi.GIS.WebAPI#76 (or by any other writer) is never shown as imagery - it falls through to a
                    // refetch, which replaces it once the service answers with content. Real imagery fails the
                    // check at its first non-flat block, so the cost on the hit path is negligible.
                    if (ortoAreaImage.Status == imageStatus_Imagery && ortoAreaImage.Bytes is not null && ortoAreaImage.Bytes.IsOrtoJpegBlank())
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox: the stored row for area {AreaKey}, epoch {Year} is a blank tile; refetching", areaKey, year);
                    }
                    else if (ortoAreaImage.Status == imageStatus_Imagery && ortoAreaImage.Bytes is not null)
                    {
                        Serilog.Modify.Log("ImageByBoundingBox served the stored row for area {AreaKey}, epoch {Year}", areaKey, year);
                        return File(ortoAreaImage.Bytes, "image/jpeg");
                    }
                    else if (ortoAreaImage.Status == imageStatus_NoImagery)
                    {
                        Serilog.Modify.Log("ImageByBoundingBox: cached negative row for area {AreaKey}, epoch {Year}", areaKey, year);
                        return NotFound();
                    }
                    else if (ortoAreaImage.Status == imageStatus_FetchError)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox: a fetch-error row exists for area {AreaKey}, epoch {Year}; answering 503 without touching the WMS", areaKey, year);
                        return StatusCode(503, "Orthophoto imagery for this epoch previously failed to fetch; retry shortly");
                    }
                    else
                    {
                        // A status-0 row without bytes is malformed; it falls through to a refetch and is replaced.
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox: stored row for area {AreaKey}, epoch {Year} carries no bytes; refetching", areaKey, year);
                    }
                }

                int width = (int)Math.Clamp(Math.Ceiling(boundingBox2D.Width * Constants.OrtoArea.MaximumImagePixelSize / Constants.OrtoArea.MaximumAreaSide), Constants.OrtoArea.MinimumImagePixelSize, Constants.OrtoArea.MaximumImagePixelSize);
                int height = boundingBox2D.Width > 0 ? (int)Math.Clamp(Math.Round(width * boundingBox2D.Height / boundingBox2D.Width), 1, Constants.OrtoArea.MaximumImagePixelSize) : 1;

                double? scaleDenominator = boundingBox2D.OrtoImageScaleDenominator(width);
                if (scaleDenominator is null || scaleDenominator.Value > GIS.Constants.WebService.OrtoMaxScaleDenominator)
                {
                    // Unreachable under the maximum-area-side guard: the pixel formula keeps the fetch near one metre
                    // per pixel, far below the service limit. Kept as defence in depth so a retuned guard can never
                    // send a white-image request (HTTP 200, no exception) that reads as "no imagery" and would be
                    // cached as such.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox rejected: tile scale denominator {ScaleDenominator} exceeds the service limit for area {AreaKey}", scaleDenominator ?? double.NaN, areaKey);
                    return BadRequest("The requested tile is coarser than the orthophoto service limit; use a smaller bounding box.");
                }

                string? url = boundingBox2D.Url_OrtoImage(GIS.Constants.WebService.StandardResolutionTime, GIS.Constants.WebService.StandardResolutionTimeLayer, year.ToString(CultureInfo.InvariantCulture), width, height);
                if (string.IsNullOrWhiteSpace(url))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "ImageByBoundingBox: the WMS URL could not be built for area {AreaKey}, epoch {Year}", areaKey, year);
                    return StatusCode(500, "The orthophoto request could not be built.");
                }

                byte[]? bytes = await httpClient.ResponseBytesAsync(url, Constants.OrtoArea.ImageFetchRetryCount, Constants.OrtoArea.ImageFetchRetryDelay, cancellationToken: cancellationToken);

                if (bytes is null)
                {
                    // Status 2: a transport failure after the retries. Deliberately NOT stored - caching it would
                    // freeze one network hiccup into a permanent "no imagery", so the row stays absent and the
                    // next request fetches again.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox: the WMS fetch failed for area {AreaKey}, epoch {Year}; nothing stored", areaKey, year);
                    return StatusCode(503, "Orthophoto service temporarily unavailable; retry shortly");
                }

                // Step 2 of exception -> blank -> hash (DiGi.GIS.WebAPI#76). The WMS answers a flaky or missing epoch
                // with HTTP 200 and a valid uniform JPEG, which the signature check alone stored as imagery for good.
                // A blank is asked for again; one that stays blank is NOT stored, like a transport failure: blanks
                // are flaky (the same tile later returns content), so caching one as imagery or as a negative row
                // would freeze a service hiccup into a permanent gap for every interpreter.
                for (int attempt = 0; attempt < Constants.OrtoArea.ImageBlankRefetchCount && bytes is not null && bytes.IsOrtoImage() && bytes.IsOrtoJpegBlank(); attempt++)
                {
                    Serilog.Modify.Log("ImageByBoundingBox: the WMS answered a blank tile for area {AreaKey}, epoch {Year}; asking again", areaKey, year);
                    await Task.Delay(Constants.OrtoArea.ImageBlankRefetchDelay, cancellationToken);
                    bytes = await httpClient.ResponseBytesAsync(url, Constants.OrtoArea.ImageFetchRetryCount, Constants.OrtoArea.ImageFetchRetryDelay, cancellationToken: cancellationToken);
                }

                if (bytes is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox: the WMS refetch failed for area {AreaKey}, epoch {Year}; nothing stored", areaKey, year);
                    return StatusCode(503, "Orthophoto service temporarily unavailable; retry shortly");
                }

                if (bytes.IsOrtoImage() && bytes.IsOrtoJpegBlank())
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ImageByBoundingBox: the WMS still answers a blank tile for area {AreaKey}, epoch {Year}; nothing stored", areaKey, year);
                    return StatusCode(503, "The orthophoto service answered a blank image for this epoch; retry later");
                }

                short status = bytes.IsOrtoImage() ? imageStatus_Imagery : imageStatus_NoImagery;

                OrtoAreaImage ortoAreaImage_New = new(areaKey, boundingBox2D, year, width, height, status, url, DateTimeOffset.UtcNow, status == imageStatus_Imagery ? bytes : null);
                long? imageStored = await ortoAreaImagePostgreSQLConverter.UpdateAsync(ortoAreaImage_New, commandTimeout: 30, cancellationToken: cancellationToken);
                if (imageStored is null or 0)
                {
                    // The bytes are in hand but the row did not land; serving them anyway would drop the one-fetch-
                    // per-area-and-epoch guarantee for every later request.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "ImageByBoundingBox: the fetched row for area {AreaKey}, epoch {Year} could not be stored", areaKey, year);
                    return StatusCode(500, "The orthophoto imagery could not be stored.");
                }

                if (status == imageStatus_NoImagery)
                {
                    Serilog.Modify.Log("ImageByBoundingBox stored a negative row for area {AreaKey}, epoch {Year}", areaKey, year);
                    return NotFound();
                }

                Serilog.Modify.Log("ImageByBoundingBox fetched and stored imagery for area {AreaKey}, epoch {Year} ({Width}x{Height} px)", areaKey, year, width, height);

                return File(bytes, "image/jpeg");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(ImageByBoundingBoxAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(ImageByBoundingBoxAsync));
                return StatusCode(500, "Internal server error during orthophoto image read");
            }
        }

        /// <summary>
        /// Returns the test area of a bounding box, creating it on first sight: the area with its server-drawn random cells, its drawn polygons, and every interpreter's label rows.
        /// <para>The area is created when no row answers its invariant location key. Its <c>Seed</c> is the first eight bytes of the SHA-256 of the key read as a little-endian <see cref="long"/> - stable across processes, unlike <c>string.GetHashCode()</c> - and the cells are sampled without replacement from the grid aligned to multiples of <see cref="Constants.OrtoArea.CellSize"/> over the box, so repeated calls (and any other interpreter's session) see identical cells. The effective count is <c>clamp(round(total &#215; Constants.OrtoArea.SamplingFraction), Constants.OrtoArea.CellCountMinimum, Constants.OrtoArea.CellCountMaximum)</c>, never more than the grid holds.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with an <see cref="OrtoAreaItems"/> composite, 401 without a valid user token, 400 for an invalid bounding box, 503 on a transient database failure, or 500 when the create or read failed.</returns>
        [HttpGet("areabyboundingbox", Name = $"{nameof(OrtoAreaController)}_{nameof(AreaByBoundingBoxAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(OrtoAreaItems), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AreaByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(AreaByBoundingBoxAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AreaByBoundingBox rejected: no valid user token");
                return Unauthorized();
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(x_1, y_1, x_2, y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AreaByBoundingBox rejected: bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            string? areaKey = boundingBox2D.OrtoLocationKey();
            if (string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AreaByBoundingBox rejected: the area key could not be derived from the bounding box");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            long AreaSeed(string key)
            {
                // Stable across processes: the first 8 bytes of the SHA-256 of the key, read little-endian on every
                // platform. string.GetHashCode is randomised per process, so it could never reproduce another
                // session's cells.
                byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
                return BinaryPrimitives.ReadInt64LittleEndian(hash);
            }

            List<OrtoAreaCell> SampleCells(BoundingBox2D box, double cellSize, long seed)
            {
                // Grid aligned to multiples of the cell size - the floor/ceiling index idiom of
                // Create.OrtoAvailabilityMatrix - so one box always yields one grid.
                long indexX_Min = (long)Math.Floor(box.Min.X / cellSize);
                long indexX_Max = (long)Math.Ceiling(box.Max.X / cellSize);
                long indexY_Min = (long)Math.Floor(box.Min.Y / cellSize);
                long indexY_Max = (long)Math.Ceiling(box.Max.Y / cellSize);

                long columns = indexX_Max - indexX_Min;
                long rows = indexY_Max - indexY_Min;
                long total = columns * rows;
                if (total <= 0)
                {
                    // A box degenerate to a single grid point holds no cell; the area is still created, with none.
                    return [];
                }

                int count = (int)Math.Round(total * Constants.OrtoArea.SamplingFraction);
                count = Math.Clamp(count, Constants.OrtoArea.CellCountMinimum, Constants.OrtoArea.CellCountMaximum);
                count = (int)Math.Min(count, total);

                // Every cell starts as its linear index in the order the grid is walked (X major, Y minor - the order
                // of the nested loops above Create.OrtoAvailabilityMatrix); count of them are then drawn without
                // replacement by a partial Fisher-Yates shuffle seeded with the area key, so the sample never repeats
                // a cell and the same seed always draws the same cells in the same creation order.
                int[] indexes = new int[total];
                for (int i = 0; i < indexes.Length; i++)
                {
                    indexes[i] = i;
                }

                // Random's seeded constructor takes an int, so the stored 64-bit seed is narrowed deterministically
                // (low 32 bits): one area key always produces the same sequence on every process and platform.
                Random random = new(unchecked((int)seed));
                for (int i = 0; i < count; i++)
                {
                    int j = random.Next(i, (int)total);
                    (indexes[i], indexes[j]) = (indexes[j], indexes[i]);
                }

                List<OrtoAreaCell> ortoAreaCells = [];
                for (int i = 0; i < count; i++)
                {
                    long indexX = indexX_Min + (indexes[i] / rows);
                    long indexY = indexY_Min + (indexes[i] % rows);
                    double x = indexX * cellSize;
                    double y = indexY * cellSize;
                    ortoAreaCells.Add(new OrtoAreaCell(i, new BoundingBox2D(new Point2D(x, y), new Point2D(x + cellSize, y + cellSize))));
                }

                return ortoAreaCells;
            }

            OrtoArea? ortoArea;
            List<OrtoAreaObject>? ortoAreaObjects;
            List<OrtoAreaLabel>? ortoAreaLabels;
            try
            {
                ortoArea = await ortoAreaPostgreSQLConverter.GetOrtoAreaAsync(areaKey, cancellationToken: cancellationToken);

                if (ortoArea is null)
                {
                    long seed = AreaSeed(areaKey);
                    List<OrtoAreaCell> ortoAreaCells = SampleCells(boundingBox2D, Constants.OrtoArea.CellSize, seed);
                    ortoArea = new OrtoArea(areaKey, boundingBox2D, null, null, Constants.OrtoArea.CellSize, ortoAreaCells.Count, seed, ortoAreaCells);

                    long? areaStored = await ortoAreaPostgreSQLConverter.UpdateAsync(ortoArea, commandTimeout: 30, cancellationToken: cancellationToken);
                    if (areaStored is null or 0)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AreaByBoundingBox: the new area {AreaKey} could not be stored", areaKey);
                        return StatusCode(500, "The test area could not be created.");
                    }

                    Serilog.Modify.Log("AreaByBoundingBox created area {AreaKey} with {CellCount} sampled cell(s), seed {Seed}", areaKey, ortoArea.Cells.Count, seed);
                }

                ortoAreaObjects = await ortoAreaObjectPostgreSQLConverter.GetOrtoAreaObjectsAsync(areaKey, cancellationToken: cancellationToken);
                ortoAreaLabels = await ortoAreaLabelPostgreSQLConverter.GetOrtoAreaLabelsAsync(areaKey, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(AreaByBoundingBoxAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(AreaByBoundingBoxAsync));
                return StatusCode(500, "Internal server error during test area read");
            }

            if (ortoAreaObjects is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AreaByBoundingBox: the drawn object read returned no result for area {AreaKey}", areaKey);
            }

            if (ortoAreaLabels is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AreaByBoundingBox: the label read returned no result for area {AreaKey}", areaKey);
            }

            OrtoAreaItems ortoAreaItems = new()
            {
                Area = ortoArea,
                Objects = ortoAreaObjects ?? [],
                Labels = ortoAreaLabels ?? []
            };

            return Ok(ortoAreaItems);
        }

        /// <summary>
        /// Stores one polygon drawn in a test area, assigning its object identifier and the sampled cell it falls in.
        /// <para>The identity is the caller's own: the bearer token resolves the <c>CreatedBy</c> email, never the body. <c>CellIndex</c> is the index of the stored sampled cell whose box contains the polygon's centroid, tested inclusive at the minimum and exclusive at the maximum corner so a centroid on a shared grid edge is attributed to exactly one cell; it is null when the centroid falls outside every sampled cell. The area must already exist (<c>areabyboundingbox</c> initializes it), so a box no area answers for is 400.</para>
        /// </summary>
        /// <param name="parameter">The drawn polygon and the area bounding box it belongs to.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the created <see cref="OrtoAreaObject"/>, 401 without a valid user token, 400 for a missing body, an invalid bounding box, a degenerate polygon (fewer than three vertices) or an uninitialized area, 503 on a transient database failure, or 500 when the read or write failed.</returns>
        [HttpPost("objects", Name = $"{nameof(OrtoAreaController)}_{nameof(CreateObjectAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(OrtoAreaObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateObjectAsync([FromBody] Parameter.OrtoAreaObjectParameter? parameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(CreateObjectAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: no valid user token");
                return Unauthorized();
            }

            if (parameter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: body is missing");
                return BadRequest("Body is missing.");
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(parameter.X_1, parameter.Y_1, parameter.X_2, parameter.Y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: bounding box is missing, not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            string? areaKey = boundingBox2D.OrtoLocationKey();
            if (string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: the area key could not be derived from the bounding box");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            Polygon2D? polygon2D = CreatePolygon(parameter.Polygon);
            if (polygon2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: polygon is missing or unreadable");
                return BadRequest("Polygon is missing or unreadable.");
            }

            List<Point2D>? point2Ds = polygon2D.GetPoints();
            if (point2Ds is null || point2Ds.Count < 3)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: polygon holds {Count} vertex/vertices, fewer than 3", point2Ds?.Count ?? 0);
                return BadRequest("Polygon must hold at least 3 vertices.");
            }

            try
            {
                OrtoArea? ortoArea = await ortoAreaPostgreSQLConverter.GetOrtoAreaAsync(areaKey, cancellationToken: cancellationToken);
                if (ortoArea is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "CreateObject rejected: area {AreaKey} is not initialized", areaKey);
                    return BadRequest("The area does not exist yet; call areabyboundingbox to initialize it first.");
                }

                Point2D? centroid = polygon2D.GetCentroid();

                // Inclusive at the minimum corner, exclusive at the maximum: a centroid exactly on a shared grid
                // edge belongs to exactly one cell (the lower one), so two sampled cells never claim it. A centroid
                // cannot be resolved (practically impossible past the three-vertex check) leaves the cell null,
                // which the row already represents.
                int? cellIndex = null;
                if (centroid is not null)
                {
                    foreach (OrtoAreaCell ortoAreaCell in ortoArea.Cells)
                    {
                        BoundingBox2D? cellBoundingBox2D = ortoAreaCell.BoundingBox2D;
                        if (cellBoundingBox2D is null)
                        {
                            continue;
                        }

                        if (centroid.X >= cellBoundingBox2D.Min.X && centroid.X < cellBoundingBox2D.Max.X && centroid.Y >= cellBoundingBox2D.Min.Y && centroid.Y < cellBoundingBox2D.Max.Y)
                        {
                            cellIndex = ortoAreaCell.Index;
                            break;
                        }
                    }
                }

                OrtoAreaObject ortoAreaObject = new(Guid.NewGuid().ToString(), areaKey, cellIndex, polygon2D, email, DateTimeOffset.UtcNow);

                long? objectStored = await ortoAreaObjectPostgreSQLConverter.UpdateAsync([ortoAreaObject], cancellationToken: cancellationToken);
                if (objectStored is null or 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CreateObject: the drawn object for area {AreaKey} could not be stored", areaKey);
                    return StatusCode(500, "The drawn object could not be stored.");
                }

                Serilog.Modify.Log("CreateObject stored object {ObjectId} in area {AreaKey}, cell {CellIndex}, by {User}", ortoAreaObject.ObjectId, areaKey, cellIndex?.ToString(CultureInfo.InvariantCulture) ?? "none", email);

                return Ok(ortoAreaObject);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(CreateObjectAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(CreateObjectAsync));
                return StatusCode(500, "Internal server error during drawn object write");
            }
        }

        /// <summary>
        /// Removes one polygon drawn in a test area, together with the caller's own labels on it.
        /// <para>Drawn polygons are shared and their labels feed the agreement between interpreters, so only the interpreter who drew the polygon may remove it, and only while no other interpreter has labelled it (<see cref="OrtoAreaObjectPostgreSQLConverter.RemoveAsync(string?, string?, string?, int, CancellationToken)"/>). The identity is the caller's own: the bearer token resolves the email, never the query. The area must already exist, so a box no area answers for is 400.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="objectId">The server-assigned identifier of the polygon to remove.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 when the polygon was removed, 401 without a valid user token, 400 for an invalid bounding box, a missing objectId or an uninitialized area, 403 when another interpreter drew the polygon, 404 when the area holds no such polygon, 409 when another interpreter has labelled it, 503 on a transient database failure, or 500 when the removal failed.</returns>
        [HttpDelete("objects", Name = $"{nameof(OrtoAreaController)}_{nameof(RemoveObjectAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveObjectAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [FromQuery(Name = "objectid")] string? objectId, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(RemoveObjectAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveObject rejected: no valid user token");
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(objectId))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveObject rejected: objectid is missing");
                return BadRequest("objectid is required.");
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(x_1, y_1, x_2, y_2);
            string? areaKey = boundingBox2D?.OrtoLocationKey();
            if (boundingBox2D is null || string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveObject rejected: bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            try
            {
                OrtoArea? ortoArea = await ortoAreaPostgreSQLConverter.GetOrtoAreaAsync(areaKey, cancellationToken: cancellationToken);
                if (ortoArea is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "RemoveObject rejected: area {AreaKey} is not initialized", areaKey);
                    return BadRequest("The area does not exist yet; call areabyboundingbox to initialize it first.");
                }

                OrtoAreaObjectRemoval? removal = await ortoAreaObjectPostgreSQLConverter.RemoveAsync(areaKey, objectId, email, cancellationToken: cancellationToken);
                Serilog.Modify.Log("RemoveObject: object {ObjectId} in area {AreaKey} by {User}: {Outcome}", objectId, areaKey, email, removal?.ToString() ?? "null");

                return removal switch
                {
                    OrtoAreaObjectRemoval.Removed => Ok(),
                    OrtoAreaObjectRemoval.NotFound => NotFound("The area holds no drawn polygon under this objectid."),
                    OrtoAreaObjectRemoval.NotCreator => StatusCode(StatusCodes.Status403Forbidden, "Only the interpreter who drew the polygon can remove it."),
                    OrtoAreaObjectRemoval.LabelledByOthers => Conflict("Another interpreter has labelled this polygon, so it cannot be removed."),
                    _ => StatusCode(500, "The drawn polygon could not be removed.")
                };
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(RemoveObjectAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(RemoveObjectAsync));
                return StatusCode(500, "Internal server error during drawn object removal");
            }
        }

        /// <summary>
        /// Lists every label row of the area of a bounding box, all interpreters, as JSON.
        /// <para>Reading is deliberately inclusive: what the other interpreters marked is what this workflow is read against, while only <c>gis/ortoarea/label</c> writes - always under the caller's own identity. The area must already exist, so a box no area answers for is 400 rather than a quietly empty list.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the label rows (an empty list when none were filed), 401 without a valid user token, 400 for an invalid bounding box or an uninitialized area, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("labelsbyboundingbox", Name = $"{nameof(OrtoAreaController)}_{nameof(LabelsByBoundingBoxAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<OrtoAreaLabel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LabelsByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(LabelsByBoundingBoxAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "LabelsByBoundingBox rejected: no valid user token");
                return Unauthorized();
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(x_1, y_1, x_2, y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "LabelsByBoundingBox rejected: bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            string? areaKey = boundingBox2D.OrtoLocationKey();
            if (string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "LabelsByBoundingBox rejected: the area key could not be derived from the bounding box");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            List<OrtoAreaLabel>? ortoAreaLabels;
            try
            {
                OrtoArea? ortoArea = await ortoAreaPostgreSQLConverter.GetOrtoAreaAsync(areaKey, cancellationToken: cancellationToken);
                if (ortoArea is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "LabelsByBoundingBox rejected: area {AreaKey} is not initialized", areaKey);
                    return BadRequest("The area does not exist yet; call areabyboundingbox to initialize it first.");
                }

                ortoAreaLabels = await ortoAreaLabelPostgreSQLConverter.GetOrtoAreaLabelsAsync(areaKey, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(LabelsByBoundingBoxAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(LabelsByBoundingBoxAsync));
                return StatusCode(500, "Internal server error during label read");
            }

            if (ortoAreaLabels is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "LabelsByBoundingBox: the label read returned no result for area {AreaKey}", areaKey);
                return Ok(new List<OrtoAreaLabel>());
            }

            Serilog.Modify.Log("LabelsByBoundingBox answered {Count} label row(s) for area {AreaKey}", ortoAreaLabels.Count, areaKey);

            return Ok(ortoAreaLabels);
        }

        /// <summary>
        /// Compares the interpreted reference sample of the area of a bounding box with BDOT10k and, optionally, with open building datasets (paper 01, sections 4.4 and 5.2).
        /// <para>Labels of one epoch are resolved per object across interpreters (disagreement resolves to indeterminate). The result gives BDOT10k commission (footprints seen as absent), omission (drawn objects seen as present) and the indeterminate share; for each dataset named, how many reference objects it detects and how its footprints in the area split into matched, matching only absent or indeterminate objects, and unmatched. See <see cref="PostgreSQL.Query.OrtoAreaValidationAsync"/> for the matching rule.</para>
        /// <para>Answers counts only - no interpreter identity - but sits behind the user session like every route of this controller.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_1">The northing of the first corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="x_2">The easting of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="y_2">The northing of the second corner of the bounding box, in EPSG:2180 metres.</param>
        /// <param name="epochYear">The imagery epoch to evaluate. Omit for the latest epoch labelled in the area.</param>
        /// <param name="datasetKeys">The open building datasets to compare (<c>eubucco</c>, <c>overture</c>, <c>osm</c>, <c>dbsm</c>, <c>microsoft</c>), repeated once per dataset. Omit to evaluate BDOT10k only.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 120 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the comparison, 401 without a valid user token, 400 for an invalid bounding box, an unknown dataset key or an uninitialized area, 404 when a named dataset's table does not exist yet, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("validationsummary", Name = $"{nameof(OrtoAreaController)}_{nameof(ValidationSummaryAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(OrtoAreaValidationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidationSummaryAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [FromQuery(Name = "epochyear")] short? epochYear, [FromQuery(Name = "datasetkeys")] List<string>? datasetKeys, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 120, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {DatasetCount} datasets", nameof(OrtoAreaController), nameof(ValidationSummaryAsync), datasetKeys?.Count ?? 0);

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ValidationSummary rejected: no valid user token");
                return Unauthorized();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ValidationSummary rejected: CommandTimeout cannot be negative");
                return BadRequest();
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(x_1, y_1, x_2, y_2);
            string? areaKey = boundingBox2D?.OrtoLocationKey();
            if (boundingBox2D is null || string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ValidationSummary rejected: bounding box is not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            List<string> datasetKeys_List = datasetKeys is null ? [] : [.. datasetKeys.Where(x => !string.IsNullOrWhiteSpace(x))];
            foreach (string datasetKey in datasetKeys_List)
            {
                string? tableName = PostgreSQL.Query.OpenBuildingTableName(datasetKey);
                if (tableName is null || string.Equals(tableName, PostgreSQL.Constants.TableName.GHS_OBAT_Building, StringComparison.Ordinal))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ValidationSummary rejected: dataset key {DatasetKey} does not name a footprint table", datasetKey);
                    return BadRequest($"Dataset key '{datasetKey}' does not name a footprint table.");
                }
            }

            if (datasetKeys_List.Count != 0 && openBuildingPostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "ValidationSummary rejected: OpenBuildingPostgreSQLConverter is not registered");
                return BadRequest("Open building datasets are not available on this host.");
            }

            OrtoAreaValidationResult? ortoAreaValidationResult;
            try
            {
                OrtoArea? ortoArea = await ortoAreaPostgreSQLConverter.GetOrtoAreaAsync(areaKey, commandTimeout, cancellationToken);
                if (ortoArea is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ValidationSummary rejected: area {AreaKey} is not initialized", areaKey);
                    return BadRequest("The area does not exist yet; call areabyboundingbox to initialize it first.");
                }

                ortoAreaValidationResult = await PostgreSQL.Query.OrtoAreaValidationAsync(ortoAreaPostgreSQLConverter, ortoAreaLabelPostgreSQLConverter, ortoAreaObjectPostgreSQLConverter, building2DPostgreSQLConverter, openBuildingPostgreSQLConverter, areaKey, epochYear, datasetKeys_List, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(ValidationSummaryAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(ValidationSummaryAsync));
                return StatusCode(500, "Internal server error during validation read");
            }

            if (ortoAreaValidationResult is null)
            {
                // The area exists and every key was validated, so a null here is a dataset table not created yet.
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "ValidationSummary: a named dataset table does not exist for area {AreaKey}", areaKey);
                return NotFound("A named dataset has not been imported yet.");
            }

            string? json = Core.Convert.ToSystem_String(ortoAreaValidationResult);
            if (string.IsNullOrWhiteSpace(json))
            {
                return StatusCode(500, "Internal server error during serialization");
            }

            Serilog.Modify.Log("ValidationSummary answered epoch {EpochYear} for area {AreaKey}", ortoAreaValidationResult.EpochYear?.ToString(CultureInfo.InvariantCulture) ?? string.Empty, areaKey);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Files (or revises) the caller's own interpretation label of one epoch of one object in a test area.
        /// <para>Gated by <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateOrtoAreaLabel"/> (400 when disabled), checked after the session and before the body, each failure logged distinctly so the cause of a shared 400 is recoverable. The object identity is exactly one of <c>reference</c> (a footprint; <c>countyId</c> is an optional narrowing hint, resolved server-side because the footprint payload carries no county id) or <c>objectId</c>, and <c>state</c> must be 0 (present), 1 (absent) or 2 (indeterminate).</para>
        /// <para><c>Interpreter</c> and <c>UpdatedAt</c> are stamped server-side from the bearer token and the clock - never taken from the body. The observation dates are resolved server-side as well: a footprint's box comes from its <c>building_2d</c> row (404 when no building holds the reference), a drawn object's box from the polygon stored under its <c>objectId</c> (404 when the area holds no such object), and the min/max date are the distinct UTC date components of the <c>orto_sheet</c> rows covering that box whose <c>Date.Year</c> equals <c>epochYear</c>; an epoch no covering sheet dates answers both null. Upsert is per (area, object identity, epoch, interpreter): two interpreters keep their own rows.</para>
        /// </summary>
        /// <param name="parameter">The label to file: the area bounding box, exactly one object identity (a footprint reference with an optional county hint, or a drawn objectId), the epoch year, the state, and the optional note.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 when the label was stored, 401 without a valid user token, 400 for a disabled flag, an invalid body, or an uninitialized area, 404 when no building holds the footprint reference or the area holds no drawn object under the objectId, 503 on a transient database failure, or 500 when the write failed.</returns>
        [HttpPost("label", Name = $"{nameof(OrtoAreaController)}_{nameof(SetLabelAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetLabelAsync([FromBody] Parameter.OrtoAreaLabelParameter? parameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(OrtoAreaController), nameof(SetLabelAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: no valid user token");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateOrtoAreaLabel)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: AllowUpdateOrtoAreaLabel is disabled");
                return BadRequest();
            }

            if (parameter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: body is missing");
                return BadRequest("Body is missing.");
            }

            BoundingBox2D? boundingBox2D = CreateBoundingBox2D(parameter.X_1, parameter.Y_1, parameter.X_2, parameter.Y_2);
            if (boundingBox2D is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: bounding box is missing, not finite, not ordered, or exceeds the maximum area side");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            if (parameter.State is not short state || (state != 0 && state != 1 && state != 2))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: State {State} is not one of 0, 1, 2", parameter.State?.ToString(CultureInfo.InvariantCulture) ?? "null");
                return BadRequest("State must be 0 (present), 1 (absent) or 2 (indeterminate).");
            }

            if (parameter.EpochYear is not short epochYear)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: EpochYear is missing");
                return BadRequest("EpochYear is required.");
            }

            // The identity is Reference XOR ObjectId. CountyId is NOT part of the identity: the footprint payload
            // (GIS wire shape of building2D/itemsbyboundingbox) carries no county id, so the client legitimately
            // sends the reference alone; CountyId, when present, only narrows the lookup below.
            bool hasReference = !string.IsNullOrWhiteSpace(parameter.Reference);
            bool hasObjectId = !string.IsNullOrWhiteSpace(parameter.ObjectId);
            if (hasReference == hasObjectId)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: the object identity must be exactly one of Reference or ObjectId");
                return BadRequest("Exactly one object identity is required: Reference, or ObjectId.");
            }

            bool footprintIdentity = hasReference;
            bool drawnIdentity = hasObjectId;

            int? countyId = footprintIdentity ? parameter.CountyId : null;
            string? reference = footprintIdentity ? parameter.Reference : null;
            string? objectId = drawnIdentity ? parameter.ObjectId : null;

            string? areaKey = boundingBox2D.OrtoLocationKey();
            if (string.IsNullOrWhiteSpace(areaKey))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: the area key could not be derived from the bounding box");
                return BadRequest("Bounding box must be finite, ordered (min <= max) and no side longer than the maximum area side.");
            }

            try
            {
                OrtoArea? ortoArea = await ortoAreaPostgreSQLConverter.GetOrtoAreaAsync(areaKey, cancellationToken: cancellationToken);
                if (ortoArea is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: area {AreaKey} is not initialized", areaKey);
                    return BadRequest("The area does not exist yet; call areabyboundingbox to initialize it first.");
                }

                BoundingBox2D? objectBoundingBox2D;
                if (footprintIdentity)
                {
                    string reference_Footprint = parameter.Reference!; // non-null: hasReference required it
                    // CountyId is a hint, not the key: an absent or wrong part falls back to the reference exactly as
                    // fallbackByReference does everywhere else, and the part the read resolved is what files the row,
                    // so the stored identity can never disagree with the geometry the observation dates came from.
                    Building2D? building2D = await building2DPostgreSQLConverter.GetBuilding2DByReferenceAsync(reference_Footprint, countyId, true, cancellationToken: cancellationToken);
                    if (building2D is null)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: no building holds reference {Reference}", reference_Footprint);
                        return NotFound();
                    }

                    countyId = building2D.CountyId;
                    objectBoundingBox2D = building2D.BoundingBox2D;
                }
                else
                {
                    // The drawn object's own stored polygon resolves the dates - never a polygon from the body: what
                    // the label sees is what was filed under the objectId, and the label POST carries no polygon.
                    List<OrtoAreaObject>? ortoAreaObjects = await ortoAreaObjectPostgreSQLConverter.GetOrtoAreaObjectsAsync(areaKey, cancellationToken: cancellationToken);
                    OrtoAreaObject? ortoAreaObject = string.IsNullOrWhiteSpace(objectId) ? null : ortoAreaObjects?.Find(x => objectId == x.ObjectId);
                    if (ortoAreaObject is null)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetLabel rejected: area {AreaKey} holds no drawn object {ObjectId}", areaKey, objectId ?? "null");
                        return NotFound();
                    }

                    objectBoundingBox2D = ortoAreaObject.Polygon2D?.GetBoundingBox();
                }

                // Observation dates from the Storage side only: the sheets whose footprint box intersects the object's
                // box and whose own Date.Year equals the epoch. The min/max are the distinct UTC date components of
                // those dates, so one sheet date of an epoch is one candidate; an epoch no covering sheet dates
                // answers both null rather than a guess.
                DateOnly? observationDateMin = null;
                DateOnly? observationDateMax = null;

                if (objectBoundingBox2D is not null)
                {
                    List<GIS.Classes.OrtoSheet>? ortoSheets = await ortoSheetPostgreSQLConverter.GetOrtoSheetsByBoundingBoxAsync(objectBoundingBox2D, cancellationToken: cancellationToken);

                    HashSet<DateOnly> observationDates = [];
                    if (ortoSheets is not null)
                    {
                        foreach (GIS.Classes.OrtoSheet ortoSheet in ortoSheets)
                        {
                            if (ortoSheet.Date is DateTimeOffset date && date.Year == epochYear)
                            {
                                observationDates.Add(DateOnly.FromDateTime(date.UtcDateTime));
                            }
                        }
                    }

                    if (observationDates.Count != 0)
                    {
                        observationDateMin = observationDates.Min();
                        observationDateMax = observationDates.Max();
                    }
                }

                // Interpreter and timestamp are the server's, never the body's - the bearer token is what files the
                // row under an identity, and UpdatedAt is what makes the row's age trustworthy.
                OrtoAreaLabel ortoAreaLabel = new(areaKey, countyId, reference, objectId, epochYear, state, observationDateMin, observationDateMax, email, parameter.Note, DateTimeOffset.UtcNow);

                long? labelStored = await ortoAreaLabelPostgreSQLConverter.UpdateAsync([ortoAreaLabel], cancellationToken: cancellationToken);
                if (labelStored is null or 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "SetLabel: the label for area {AreaKey}, epoch {Year} could not be stored", areaKey, epochYear);
                    return StatusCode(500, "The interpretation label could not be stored.");
                }

                Serilog.Modify.Log("SetLabel filed state {State} for area {AreaKey}, epoch {Year} as {User} (observation dates {Min}/{Max})", state, areaKey, epochYear, email, observationDateMin?.ToString(CultureInfo.InvariantCulture) ?? "none", observationDateMax?.ToString(CultureInfo.InvariantCulture) ?? "none");

                return Ok();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(OrtoAreaController), nameof(SetLabelAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(OrtoAreaController), nameof(SetLabelAsync));
                return StatusCode(500, "Internal server error during interpretation label write");
            }
        }

        /// <summary>
        /// Validates four bounding-box corners and builds their box, applied to every <c>gis/ortoarea</c> route.
        /// <para>Refuses non-finite values, a box whose minimum corner is not below its maximum, and any side longer than <see cref="Constants.OrtoArea.MaximumAreaSide"/>; each refusal is answered 400 by the caller. A box degenerate to a point or a line is accepted - it simply holds no grid cell and the WMS answers it.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner, or <c>null</c> when omitted.</param>
        /// <param name="y_1">The northing of the first corner, or <c>null</c> when omitted.</param>
        /// <param name="x_2">The easting of the second corner, or <c>null</c> when omitted.</param>
        /// <param name="y_2">The northing of the second corner, or <c>null</c> when omitted.</param>
        /// <returns>The bounding box, or null when a corner is missing or non-finite, the corners are not ordered, or a side exceeds the maximum.</returns>
        private static BoundingBox2D? CreateBoundingBox2D(double? x_1, double? y_1, double? x_2, double? y_2)
        {
            if (x_1 is not double x_1_Value || y_1 is not double y_1_Value || x_2 is not double x_2_Value || y_2 is not double y_2_Value)
            {
                return null;
            }

            if (!double.IsFinite(x_1_Value) || !double.IsFinite(y_1_Value) || !double.IsFinite(x_2_Value) || !double.IsFinite(y_2_Value))
            {
                return null;
            }

            if (x_1_Value > x_2_Value || y_1_Value > y_2_Value)
            {
                return null;
            }

            if (x_2_Value - x_1_Value > Constants.OrtoArea.MaximumAreaSide || y_2_Value - y_1_Value > Constants.OrtoArea.MaximumAreaSide)
            {
                return null;
            }

            return new BoundingBox2D(new Point2D(x_1_Value, y_1_Value), new Point2D(x_2_Value, y_2_Value));
        }

        /// <summary>
        /// Resolves a serialized polygon body into a <see cref="Polygon2D"/>.
        /// <para>Accepts the shapes a relayed body may carry: a bare array of points, an object holding its point array under <c>point2Ds</c>/<c>points</c> (the DiGi serializer's names and their lower-cased forms), or a serialized DiGi <c>Polygon2D</c>. Points are pairs <c>[x, y]</c> or objects with <c>x</c>/<c>y</c> (either case); one unreadable element disqualifies the whole polygon rather than silently dropping it.</para>
        /// </summary>
        /// <param name="jsonNode">The serialized polygon, or null.</param>
        /// <returns>The polygon with the parsed points, or null when the body is missing or no shape could be read. Vertex-count validation is the caller's.</returns>
        private static Polygon2D? CreatePolygon(JsonNode? jsonNode)
        {
            if (jsonNode is JsonArray pointArray)
            {
                List<Point2D>? point2Ds = ParsePoint2Ds(pointArray);
                return point2Ds is null ? null : new Polygon2D(point2Ds);
            }

            if (jsonNode is not JsonObject jsonObject)
            {
                return null;
            }

            foreach (string propertyName in polygonPointPropertyNames)
            {
                if (jsonObject[propertyName] is JsonArray pointsArray)
                {
                    List<Point2D>? point2Ds = ParsePoint2Ds(pointsArray);
                    return point2Ds is null ? null : new Polygon2D(point2Ds);
                }
            }

            return Core.Create.SerializableObject<Polygon2D>(jsonObject);

            static List<Point2D>? ParsePoint2Ds(JsonArray jsonArray)
            {
                List<Point2D> point2Ds = [];
                foreach (JsonNode? jsonNode_Temp in jsonArray)
                {
                    double x;
                    double y;

                    if (jsonNode_Temp is JsonArray pair)
                    {
                        if (pair.Count < 2 || !TryNumber(pair[0], out x) || !TryNumber(pair[1], out y))
                        {
                            return null;
                        }
                    }
                    else if (jsonNode_Temp is JsonObject object_Temp)
                    {
                        if (!TryNumber(object_Temp["x"] ?? object_Temp["X"], out x) || !TryNumber(object_Temp["y"] ?? object_Temp["Y"], out y))
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                    point2Ds.Add(new Point2D(x, y));
                }

                return point2Ds;
            }

            static bool TryNumber(JsonNode? jsonNode, out double value)
            {
                if (jsonNode is JsonValue jsonValue && jsonValue.TryGetValue<double>(out double value_Temp))
                {
                    value = value_Temp;
                    return true;
                }

                value = default;
                return false;
            }
        }
    }
}
