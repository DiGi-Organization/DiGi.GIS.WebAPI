using DiGi.GIS.Classes;
using DiGi.GIS.Enums;
using DiGi.GIS.PostgreSQL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Serves the building shape labelling campaign of the Building Shape Accuracy feature (DiGi.GIS#12, DiGi.GIS.WebAPI#80): the next unlabelled footprint of a fixed sample for the signed-in annotator, the annotator's progress, the label write, the export of the sample and its labels, and the key-authorized sample load.
    /// <para>Every annotator route sits behind the user session: the bearer token is resolved with <see cref="Query.GetUserEmail"/> and a missing or invalid one answers 401. The interpreter of a label is that session's e-mail and its timestamp is the server clock - never the body's. <c>nextsample</c> carries no solver shape, so the labelling stays blind; the solver shapes are served only by the <c>sampleitems</c> export. The writes are gated by <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingShapeLabel"/>: the label write answers 400 when it is disabled (a 401 there would read as an expired session and sign the annotator out), while the sample load, which additionally needs the <c>key</c> header, answers the key gate's 401 for both denials.</para>
    /// <para>A transient database failure is left to <see cref="TransientDatabaseFailureAttribute"/> (503 with <c>Retry-After</c>); every other failure is logged and answered 500.</para>
    /// <para>The query names (<c>samplekey</c>, <c>afterordinal</c>, <c>countyids</c>, <c>size</c>, <c>seed</c>) are a hand-matched wire contract consumed by DiGi.GIS.WebAPI.UI#86 - renaming one breaks the relay silently.</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingShapeController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly GISWebAPIConfigurationFileWatcher gISWebAPIConfigurationFileWatcher;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter;
        private readonly BuildingShapePostgreSQLConverter buildingShapePostgreSQLConverter;
        private readonly DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager;
        private readonly DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore;

        // This has to stay the ONLY public constructor: MVC activates controllers through
        // ActivatorUtilities.CreateFactory(type, Type.EmptyTypes), and a second public constructor turns every
        // action into an HTTP 500 before its body runs (DiGi.GIS.WebAPI#6).
        /// <summary> Initializes a new instance of the <see cref="BuildingShapeController"/> class. </summary>
        /// <param name="gISWebAPIConfigurationFileWatcher">The configuration file watcher for the GIS PostgreSQL Web API, holding the <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingShapeLabel"/> gate and the access key.</param>
        /// <param name="building2DPostgreSQLConverter">The converter for Building2D objects, used to serve the footprint of a sample item and to draw a sample (Main database).</param>
        /// <param name="buildingShapePostgreSQLConverter">The converter for the labelling samples and their labels (Main database).</param>
        /// <param name="securityKeyManager">The user extension&apos;s security key manager; <c>null</c> when the user extension is not loaded, in which case every user-token check denies.</param>
        /// <param name="tokenRevocationStore">The user extension&apos;s token revocation store; <c>null</c> when the user extension is not loaded, in which case every user-token check denies.</param>
        public BuildingShapeController(GISWebAPIConfigurationFileWatcher gISWebAPIConfigurationFileWatcher, Building2DPostgreSQLConverter building2DPostgreSQLConverter, BuildingShapePostgreSQLConverter buildingShapePostgreSQLConverter, DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager = null, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore = null)
        {
            this.gISWebAPIConfigurationFileWatcher = gISWebAPIConfigurationFileWatcher;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.buildingShapePostgreSQLConverter = buildingShapePostgreSQLConverter;
            this.securityKeyManager = securityKeyManager;
            this.tokenRevocationStore = tokenRevocationStore;
        }

        /// <summary>
        /// Returns the signed-in annotator's next unlabelled item of a sample, with its footprint, and without the solver shape.
        /// <para>Without <paramref name="afterOrdinal"/> the unlabelled item with the lowest ordinal is served; with it the search starts after that ordinal and wraps around, which is how Skip moves on. The body is <c>{ "SampleKey", "Ordinal", "CountyId", "Reference", "Building2D" }</c>, where <c>Building2D</c> is the DiGi JSON of the footprint (read with the reference fallback) or <c>null</c> when no building holds the reference any more - the item can still be labelled as undefined.</para>
        /// </summary>
        /// <param name="sampleKey">The identity of the sample.</param>
        /// <param name="afterOrdinal">The ordinal of the item last seen or skipped, or null to start from the beginning.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the item, 401 without a valid user token, 400 without a sample key, 404 when the annotator has labelled every item or the sample does not exist, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("nextsample", Name = $"{nameof(BuildingShapeController)}_{nameof(NextSampleAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> NextSampleAsync([FromQuery(Name = "samplekey")] string? sampleKey, [FromQuery(Name = "afterordinal")] int? afterOrdinal = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingShapeController), nameof(NextSampleAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: no valid user token", nameof(NextSampleAsync));
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(sampleKey))
            {
                return BadRequest("samplekey is required.");
            }

            try
            {
                BuildingShapeSampleItem? buildingShapeSampleItem = await buildingShapePostgreSQLConverter.GetNextSampleItemAsync(sampleKey, email, afterOrdinal, cancellationToken: cancellationToken);
                if (buildingShapeSampleItem is null)
                {
                    return NotFound();
                }

                JsonObject? jsonObject_Building2D = null;
                if (!string.IsNullOrWhiteSpace(buildingShapeSampleItem.Reference))
                {
                    PostgreSQL.Classes.Building2D? building2D = await building2DPostgreSQLConverter.GetBuilding2DByReferenceAsync(buildingShapeSampleItem.Reference, buildingShapeSampleItem.CountyId, fallbackByReference: true, cancellationToken: cancellationToken);
                    jsonObject_Building2D = building2D?.ToDiGi()?.ToJsonObject();
                }

                // The solver shape is deliberately left out: the annotator labels blind.
                JsonObject jsonObject = new()
                {
                    [nameof(BuildingShapeSampleItem.SampleKey)] = buildingShapeSampleItem.SampleKey,
                    [nameof(BuildingShapeSampleItem.Ordinal)] = buildingShapeSampleItem.Ordinal,
                    [nameof(BuildingShapeSampleItem.CountyId)] = buildingShapeSampleItem.CountyId,
                    [nameof(BuildingShapeSampleItem.Reference)] = buildingShapeSampleItem.Reference,
                    [nameof(GIS.Classes.Building2D)] = jsonObject_Building2D,
                };

                return Content(jsonObject.ToJsonString(), "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!TransientDatabaseFailureAttribute.IsTransient(exception))
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingShapeController), nameof(NextSampleAsync));
                return StatusCode(500, "Internal server error while reading the next sample item");
            }
        }

        /// <summary>
        /// Returns the signed-in annotator's progress through a sample as <c>{ "Labelled", "Total" }</c>.
        /// </summary>
        /// <param name="sampleKey">The identity of the sample.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the counts (both 0 for an unknown sample), 401 without a valid user token, 400 without a sample key, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("progress", Name = $"{nameof(BuildingShapeController)}_{nameof(ProgressAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProgressAsync([FromQuery(Name = "samplekey")] string? sampleKey, CancellationToken cancellationToken = default)
        {
            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: no valid user token", nameof(ProgressAsync));
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(sampleKey))
            {
                return BadRequest("samplekey is required.");
            }

            try
            {
                int? labelled = await buildingShapePostgreSQLConverter.GetLabelCountAsync(sampleKey, email, cancellationToken: cancellationToken);
                int? total = await buildingShapePostgreSQLConverter.GetSampleItemCountAsync(sampleKey, cancellationToken: cancellationToken);
                if (labelled is null || total is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Name}: the counts of sample {SampleKey} could not be read", nameof(ProgressAsync), sampleKey);
                    return StatusCode(500, "The labelling progress could not be read.");
                }

                JsonObject jsonObject = new()
                {
                    ["Labelled"] = labelled.Value,
                    ["Total"] = total.Value,
                };

                return Content(jsonObject.ToJsonString(), "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!TransientDatabaseFailureAttribute.IsTransient(exception))
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingShapeController), nameof(ProgressAsync));
                return StatusCode(500, "Internal server error while reading the labelling progress");
            }
        }

        /// <summary>
        /// Files (or revises) the signed-in annotator's own label of one sample item.
        /// <para>Gated by <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingShapeLabel"/> (a bare 400 when disabled), checked after the session and before the body. <c>Shape</c> must be a defined <see cref="BuildingShape"/> value; <see cref="BuildingShape.Undefined"/> (0) records "cannot classify". The interpreter is the session e-mail and the timestamp the server clock. One label is kept per item and interpreter: a re-label replaces the annotator's own row.</para>
        /// </summary>
        /// <param name="parameter">The label: sample key, county id, reference, shape and optional note.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 when the label was stored, 401 without a valid user token, 400 for a disabled flag, an invalid body or an item that is not in the sample, 503 on a transient database failure, or 500 when the write failed.</returns>
        [HttpPost("label", Name = $"{nameof(BuildingShapeController)}_{nameof(SetLabelAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetLabelAsync([FromBody] Parameter.BuildingShapeLabelParameter? parameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingShapeController), nameof(SetLabelAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: no valid user token", nameof(SetLabelAsync));
                return Unauthorized();
            }

            if (!gISWebAPIConfigurationFileWatcher.AllowUpdateBuildingShapeLabel)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: AllowUpdateBuildingShapeLabel is disabled", nameof(SetLabelAsync));
                return BadRequest();
            }

            if (parameter is null)
            {
                return BadRequest("Body is missing.");
            }

            if (string.IsNullOrWhiteSpace(parameter.SampleKey) || parameter.CountyId is not int countyId || string.IsNullOrWhiteSpace(parameter.Reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: SampleKey, CountyId and Reference are required", nameof(SetLabelAsync));
                return BadRequest("SampleKey, CountyId and Reference are required.");
            }

            if (parameter.Shape is not short shape || !Enum.IsDefined(typeof(BuildingShape), (int)shape))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: Shape {Shape} is not a building shape", nameof(SetLabelAsync), parameter.Shape?.ToString(CultureInfo.InvariantCulture) ?? "null");
                return BadRequest("Shape must be a defined building shape value (0 = cannot classify).");
            }

            try
            {
                // Interpreter and timestamp are the server's, never the body's.
                BuildingShapeLabel buildingShapeLabel = new(parameter.SampleKey, countyId, parameter.Reference, (BuildingShape)shape, email, string.IsNullOrWhiteSpace(parameter.Note) ? null : parameter.Note, DateTimeOffset.UtcNow);

                bool? stored = await buildingShapePostgreSQLConverter.UpsertLabelAsync(buildingShapeLabel, cancellationToken: cancellationToken);
                if (stored is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Name}: the label of {Reference} in sample {SampleKey} could not be stored", nameof(SetLabelAsync), parameter.Reference, parameter.SampleKey);
                    return StatusCode(500, "The building shape label could not be stored.");
                }

                if (stored == false)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: county {CountyId}, reference {Reference} is not an item of sample {SampleKey}", nameof(SetLabelAsync), countyId, parameter.Reference, parameter.SampleKey);
                    return BadRequest("The item is not part of the sample.");
                }

                Serilog.Modify.Log("{Name} filed {Shape} for {Reference} in sample {SampleKey} as {User}", nameof(SetLabelAsync), (BuildingShape)shape, parameter.Reference, parameter.SampleKey, email);

                return Ok();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!TransientDatabaseFailureAttribute.IsTransient(exception))
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingShapeController), nameof(SetLabelAsync));
                return StatusCode(500, "Internal server error during building shape label write");
            }
        }

        /// <summary>
        /// Returns every label filed for the items of a sample, by every annotator, ordered by item ordinal and interpreter, as DiGi <see cref="BuildingShapeLabel"/> JSON.
        /// </summary>
        /// <param name="sampleKey">The identity of the sample.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the labels (an empty list for an unknown or unlabelled sample), 401 without a valid user token, 400 without a sample key, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("labels", Name = $"{nameof(BuildingShapeController)}_{nameof(LabelsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LabelsAsync([FromQuery(Name = "samplekey")] string? sampleKey, CancellationToken cancellationToken = default)
        {
            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: no valid user token", nameof(LabelsAsync));
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(sampleKey))
            {
                return BadRequest("samplekey is required.");
            }

            try
            {
                List<BuildingShapeLabel>? buildingShapeLabels = await buildingShapePostgreSQLConverter.GetLabelsAsync(sampleKey, cancellationToken: cancellationToken);
                if (buildingShapeLabels is null)
                {
                    return StatusCode(500, "The building shape labels could not be read.");
                }

                return Content(Core.Convert.ToSystem_String(buildingShapeLabels) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!TransientDatabaseFailureAttribute.IsTransient(exception))
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingShapeController), nameof(LabelsAsync));
                return StatusCode(500, "Internal server error while reading the building shape labels");
            }
        }

        /// <summary>
        /// Returns every item of a sample in ordinal order, with its solver shape, as DiGi <see cref="BuildingShapeSampleItem"/> JSON - the export side of the accuracy report.
        /// <para>This is the only route serving the solver shapes; the labelling page calls it for the export, never while labelling.</para>
        /// </summary>
        /// <param name="sampleKey">The identity of the sample.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the items (an empty list for an unknown sample), 401 without a valid user token, 400 without a sample key, 503 on a transient database failure, or 500 when the read failed.</returns>
        [HttpGet("sampleitems", Name = $"{nameof(BuildingShapeController)}_{nameof(SampleItemsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SampleItemsAsync([FromQuery(Name = "samplekey")] string? sampleKey, CancellationToken cancellationToken = default)
        {
            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: no valid user token", nameof(SampleItemsAsync));
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(sampleKey))
            {
                return BadRequest("samplekey is required.");
            }

            try
            {
                List<BuildingShapeSampleItem>? buildingShapeSampleItems = await buildingShapePostgreSQLConverter.GetSampleItemsAsync(sampleKey, cancellationToken: cancellationToken);
                if (buildingShapeSampleItems is null)
                {
                    return StatusCode(500, "The sample items could not be read.");
                }

                return Content(Core.Convert.ToSystem_String(buildingShapeSampleItems) ?? "[]", "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!TransientDatabaseFailureAttribute.IsTransient(exception))
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingShapeController), nameof(SampleItemsAsync));
                return StatusCode(500, "Internal server error while reading the sample items");
            }
        }

        /// <summary>
        /// Draws a stratified labelling sample from the footprints of the given county parts and stores it under a new sample key.
        /// <para>Authorized by the <c>key</c> header and gated by <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingShapeLabel"/>; both denials answer the same bodyless 401 and are told apart only in the log. An existing key answers 409 before any footprint is read, so a sample is never overwritten. The draw solves every footprint of the counties with the default <see cref="BuildingShapeSolver"/>, which takes minutes for tens of thousands of footprints - call it with a long client timeout. The answer is <c>{ "SampleKey", "Count", "Shapes": { "&lt;shape&gt;": count } }</c>.</para>
        /// </summary>
        /// <param name="sampleKey">The identity of the new sample.</param>
        /// <param name="countyIds">The county part ids (not codes) to draw from.</param>
        /// <param name="size">The requested number of items (1 to 10 000).</param>
        /// <param name="seed">The seed of the draw.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the summary, 401 without a valid key or with the flag disabled, 400 for invalid parameters, 404 when the counties hold no classifiable footprint, 409 when the sample key exists, 503 on a transient database failure, or 500 when the draw or the write failed.</returns>
        [HttpPost("sample", Name = $"{nameof(BuildingShapeController)}_{nameof(CreateSampleAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSampleAsync([FromQuery(Name = "samplekey")] string? sampleKey, [FromQuery(Name = "countyids")] int[]? countyIds, [FromQuery(Name = "size")] int? size, [FromQuery(Name = "seed")] int? seed, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingShapeController), nameof(CreateSampleAsync));

            if (!gISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: no valid access key", nameof(CreateSampleAsync));
                return Unauthorized();
            }

            if (!gISWebAPIConfigurationFileWatcher.AllowUpdateBuildingShapeLabel)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Name} rejected: AllowUpdateBuildingShapeLabel is disabled", nameof(CreateSampleAsync));
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(sampleKey) || countyIds is null || countyIds.Length == 0 || size is not int size_Value || size_Value < 1 || size_Value > 10000 || seed is not int seed_Value)
            {
                return BadRequest("samplekey, countyids, size (1 to 10000) and seed are required.");
            }

            try
            {
                int? count_Existing = await buildingShapePostgreSQLConverter.GetSampleItemCountAsync(sampleKey, cancellationToken: cancellationToken);
                if (count_Existing is null)
                {
                    return StatusCode(500, "The sample could not be checked.");
                }

                if (count_Existing.Value > 0)
                {
                    return Conflict($"Sample {sampleKey} already exists.");
                }

                List<BuildingShapeSampleItem>? buildingShapeSampleItems = await PostgreSQL.Query.BuildingShapeSampleItemsAsync(building2DPostgreSQLConverter, sampleKey, countyIds, size_Value, seed_Value, cancellationToken: cancellationToken);
                if (buildingShapeSampleItems is null)
                {
                    return StatusCode(500, "The sample could not be drawn.");
                }

                if (buildingShapeSampleItems.Count == 0)
                {
                    return NotFound("The counties hold no classifiable footprint.");
                }

                bool? inserted = await buildingShapePostgreSQLConverter.InsertSampleAsync(buildingShapeSampleItems, cancellationToken: cancellationToken);
                if (inserted is null)
                {
                    return StatusCode(500, "The sample could not be stored.");
                }

                if (inserted == false)
                {
                    return Conflict($"Sample {sampleKey} already exists.");
                }

                JsonObject jsonObject_Shapes = [];
                foreach (IGrouping<BuildingShape, BuildingShapeSampleItem> grouping in buildingShapeSampleItems.GroupBy(x => x.SolverShape).OrderBy(x => x.Key))
                {
                    jsonObject_Shapes[Core.Query.Description(grouping.Key) ?? grouping.Key.ToString()] = grouping.Count();
                }

                JsonObject jsonObject = new()
                {
                    ["SampleKey"] = sampleKey,
                    ["Count"] = buildingShapeSampleItems.Count,
                    ["Shapes"] = jsonObject_Shapes,
                };

                Serilog.Modify.Log("{Name} stored sample {SampleKey} with {Count} item(s) from {CountyCount} county part(s)", nameof(CreateSampleAsync), sampleKey, buildingShapeSampleItems.Count, countyIds.Length);

                return Content(jsonObject.ToJsonString(), "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception) when (!TransientDatabaseFailureAttribute.IsTransient(exception))
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingShapeController), nameof(CreateSampleAsync));
                return StatusCode(500, "Internal server error while drawing the building shape sample");
            }
        }
    }
}
