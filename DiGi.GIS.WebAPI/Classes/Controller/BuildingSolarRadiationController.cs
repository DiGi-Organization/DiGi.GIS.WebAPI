using DiGi.GIS.PostgreSQL.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Serves the stored solar radiation of buildings (<c>building_solar_radiation</c>): a read open to every visitor, and a write that only a signed-in user can make.
    /// <para>A stored result is keyed by county part, building model id, neighbour radius and solver version. Its weather members describe the stored summary, not the key, so a client compares them with its own weather source and rebuilds the summary from the stored shading factors when they differ.</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingSolarRadiationController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter;
        private readonly BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter;
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;
        private readonly DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager;
        private readonly DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingSolarRadiationController"/> class. This is the controller's only public constructor: MVC activates it through <c>ActivatorUtilities</c>, and a second one would answer every action with 500 (<c>Coding - WebAPI Contracts.md</c> §5).
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration watcher holding <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateSolarRadiationData"/>.</param>
        /// <param name="buildingSolarRadiationPostgreSQLConverter">The converter of the <c>building_solar_radiation</c> table (Storage database).</param>
        /// <param name="building2DPostgreSQLConverter">The converter of the <c>building_2d</c> table (Main database), which decides the county part a write is filed under.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter of the <c>administrative_areal_2d</c> table (Main database), which widens that decision to the sibling parts of the county.</param>
        /// <param name="securityKeyManager">The user extension&apos;s security key manager; <c>null</c> when the user extension is not loaded, in which case every write is refused with 401.</param>
        /// <param name="tokenRevocationStore">The user extension&apos;s token revocation store; <c>null</c> when the user extension is not loaded, in which case every write is refused with 401.</param>
        public BuildingSolarRadiationController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager = null, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore = null)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.buildingSolarRadiationPostgreSQLConverter = buildingSolarRadiationPostgreSQLConverter;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
            this.securityKeyManager = securityKeyManager;
            this.tokenRevocationStore = tokenRevocationStore;
        }

        /// <summary>
        /// Reads the stored solar radiation of a building model.
        /// <para>Open to every visitor, like the building model read the panel already uses. Every parameter is bound as nullable, and a missing one answers 400 rather than meaning <c>default</c> (<c>Coding - WebAPI Contracts.md</c> §2). The row is looked for under <paramref name="countyId"/> and then under every county part, because a building of a multi-part county may be filed under a sibling part.</para>
        /// </summary>
        /// <param name="id">The id of the building model.</param>
        /// <param name="countyId">The county part the building is filed under.</param>
        /// <param name="radius">The neighbour radius, in m, exactly as stored (the producer rounds it to 0.1 m); it is not rounded here.</param>
        /// <param name="solverVersion">The solver version.</param>
        /// <param name="includeShadingFactors">True to include every surface's shading factors (8 760 bytes each, base64); omitted or false leaves them null, which is all a client needs when the stored summary matches its weather source.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the <see cref="GIS.Classes.BuildingSolarRadiationResult"/>, 204 when nothing is stored for the key, 400 for a missing or invalid parameter, 503 for a transient database failure, or 500.</returns>
        [HttpGet("solarradiationbybuildingmodelid", Name = $"{nameof(BuildingSolarRadiationController)}_{nameof(GetSolarRadiationByBuildingModelIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(GIS.Classes.BuildingSolarRadiationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSolarRadiationByBuildingModelIdAsync([FromQuery(Name = "id")] long? id, [FromQuery(Name = "countyid")] int? countyId, [FromQuery(Name = "radius")] double? radius, [FromQuery(Name = "solverversion")] int? solverVersion, [FromQuery(Name = "shadingfactors")] bool? includeShadingFactors, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for building model {Id} (county {CountyId})", nameof(BuildingSolarRadiationController), nameof(GetSolarRadiationByBuildingModelIdAsync), id?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "-", countyId?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "-");

            if (id is null || id.Value <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SolarRadiationByBuildingModelId rejected: id is missing or not positive");
                return BadRequest();
            }

            if (countyId is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SolarRadiationByBuildingModelId rejected: countyid is missing");
                return BadRequest();
            }

            if (radius is null || !double.IsFinite(radius.Value) || radius.Value <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SolarRadiationByBuildingModelId rejected: radius is missing or not a positive number");
                return BadRequest();
            }

            if (solverVersion is null || solverVersion.Value <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SolarRadiationByBuildingModelId rejected: solverversion is missing or not positive");
                return BadRequest();
            }

            GIS.Classes.BuildingSolarRadiationResult? buildingSolarRadiationResult;
            try
            {
                buildingSolarRadiationResult = await buildingSolarRadiationPostgreSQLConverter.GetAsync(countyId.Value, id.Value, radius.Value, solverVersion.Value, includeShadingFactors ?? false, true, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingSolarRadiationController), nameof(GetSolarRadiationByBuildingModelIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingSolarRadiationController), nameof(GetSolarRadiationByBuildingModelIdAsync));
                return StatusCode(500, "Internal server error during stored solar radiation read");
            }

            if (buildingSolarRadiationResult is null)
            {
                return NoContent();
            }

            string? json = Core.Convert.ToSystem_String(buildingSolarRadiationResult);
            if (string.IsNullOrWhiteSpace(json))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "SolarRadiationByBuildingModelId: the stored result of building model {Id} could not be serialized", id);
                return StatusCode(500, "Stored solar radiation could not be serialized");
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Summarises what each of the named county parts holds per neighbour radius and solver version: the rows stored, the residential buildings covered against all residential buildings, the first and last write, and the size of the part's partition.
        /// <para>The production measure of a batch solve (DiGi.GIS.PostgreSQL#137): the solve keeps its counts in memory, and production is read only through this API, never through a configuration file (<c>Coding - PostgreSQL.md</c> §6). Counts and sizes only - no building data leaves. Open to every visitor, like the other <c>summariesbycountyids</c> reads.</para>
        /// <para>Naming no county part summarises every part holding results. A part holding none is absent rather than present with zeros, and a missing table answers an empty list.</para>
        /// </summary>
        /// <param name="countyIds">The identifiers of the county parts to summarise, repeated once per part (<c>countyids=55417&amp;countyids=53477</c>). Omit to summarise every part.</param>
        /// <param name="solverVersion">The solver version to summarise; omit to summarise every version, one entry per version.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with a JSON list of <see cref="BuildingSolarRadiationCountyResult"/> (empty when nothing is stored), 400 for a negative timeout, 503 for a transient database failure, or 500.</returns>
        [HttpGet("summariesbycountyids", Name = $"{nameof(BuildingSolarRadiationController)}_{nameof(GetSummariesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<BuildingSolarRadiationCountyResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummariesByCountyIdsAsync([FromQuery(Name = "countyids")] List<int>? countyIds, [FromQuery(Name = "solverversion")] int? solverVersion = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {CountyCount} county parts", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync), countyIds?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: commandtimeout cannot be negative", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync));
                return BadRequest();
            }

            List<BuildingSolarRadiationCountyResult>? buildingSolarRadiationCountyResults;
            try
            {
                buildingSolarRadiationCountyResults = await buildingSolarRadiationPostgreSQLConverter.GetSummariesByCountyIdsAsync(countyIds is null || countyIds.Count == 0 ? null : countyIds, solverVersion, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            // Null is a failure to reach the database, never "nothing stored" - an empty list says that (Coding - WebAPI Contracts.md).
            if (buildingSolarRadiationCountyResults is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name} failed: no connection to the Storage database", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync));
                return StatusCode(500, "Solar radiation summaries could not be read");
            }

            if (buildingSolarRadiationCountyResults.Count == 0)
            {
                return Content("[]", "application/json");
            }

            string? json = Core.Convert.ToSystem_String(buildingSolarRadiationCountyResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name}: {Count} summaries could not be serialized", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync), buildingSolarRadiationCountyResults.Count);
                return StatusCode(500, "Solar radiation summaries could not be serialized");
            }

            Serilog.Modify.Log("{Type}:{Name} returns {Count} summaries", nameof(BuildingSolarRadiationController), nameof(GetSummariesByCountyIdsAsync), buildingSolarRadiationCountyResults.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Stores the solar radiation of a building on behalf of the signed-in visitor, replacing the result stored for the same key.
        /// <para>The bearer token identifies the user, as for <c>yearbuiltdata/setuseryearbuilt</c>, and <see cref="GISWebAPIConfigurationFileWatcher.AllowUpdateSolarRadiationData"/> gates the write; each refusal is logged distinctly so the cause of a 4xx is recoverable where the status is shared. The server stamps <see cref="GIS.Classes.BuildingSolarRadiationResult.UserEmail"/> from the token, ignoring any value in the body, and sets <see cref="GIS.Classes.BuildingSolarRadiationResult.CreatedAt"/> to now.</para>
        /// <para>The body is the DiGi JSON of a <see cref="GIS.Classes.BuildingSolarRadiationResult"/> (<c>ToJsonObject()</c>, with <c>_type</c>), bound as a <see cref="JsonObject"/> so the host's System.Text.Json formatter never sees the DiGi layout. The county part is resolved from the body's reference through <c>building_2d</c>, widened to the sibling parts of the body's <c>CountyId</c>; the body's part is only the candidate (<c>Coding - GIS Administrative Data.md</c>).</para>
        /// </summary>
        /// <param name="jsonObject">The result to store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the id of the stored row; 401 without a valid user token; 400 for a disabled flag or an invalid body; 404 when no part of the county holds the reference; 503 for a transient database failure; or 500 when the write failed.</returns>
        [HttpPost("setsolarradiation", Name = $"{nameof(BuildingSolarRadiationController)}_{nameof(SetSolarRadiationAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetSolarRadiationAsync([FromBody] JsonObject? jsonObject, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingSolarRadiationController), nameof(SetSolarRadiationAsync));

            string? email = Query.GetUserEmail(securityKeyManager, tokenRevocationStore, HttpContext.Request.Headers.Authorization);
            if (email is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetSolarRadiation rejected: no valid user token");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateSolarRadiationData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetSolarRadiation rejected: AllowUpdateSolarRadiationData is disabled");
                return BadRequest();
            }

            if (jsonObject is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetSolarRadiation rejected: body is missing");
                return BadRequest();
            }

            GIS.Classes.BuildingSolarRadiationResult? buildingSolarRadiationResult;
            try
            {
                buildingSolarRadiationResult = Core.Create.SerializableObject<GIS.Classes.BuildingSolarRadiationResult>(jsonObject);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "SetSolarRadiation rejected: body could not be read as a BuildingSolarRadiationResult");
                return BadRequest();
            }

            if (buildingSolarRadiationResult is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetSolarRadiation rejected: body is not a BuildingSolarRadiationResult");
                return BadRequest();
            }

            string? rejection = Rejection(buildingSolarRadiationResult);
            if (rejection is not null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetSolarRadiation rejected: {Rejection}", rejection);
                return BadRequest();
            }

            string reference = buildingSolarRadiationResult.Reference!;

            try
            {
                Dictionary<string, int>? countyIds_ByReference = await PostgreSQL.Query.CountyIdsByReferencesWithSiblingFallbackAsync(building2DPostgreSQLConverter, administrativeAreal2DPostgreSQLConverter, [reference], [buildingSolarRadiationResult.CountyId], cancellationToken: cancellationToken);
                if (countyIds_ByReference is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "SetSolarRadiation: the county part could not be resolved, the building_2d lookup could not run (check the Main database configuration)");
                    return StatusCode(500, "County part could not be resolved: the building_2d lookup could not run.");
                }

                if (!countyIds_ByReference.TryGetValue(reference, out int countyId))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "SetSolarRadiation: no part of the county of {CountyId} holds reference {Reference}", buildingSolarRadiationResult.CountyId, reference);
                    return NotFound();
                }

                // The user and the time come from the server, never from the body.
                GIS.Classes.BuildingSolarRadiationResult buildingSolarRadiationResult_Stamped = new(
                    reference,
                    countyId,
                    buildingSolarRadiationResult.BuildingModelId,
                    buildingSolarRadiationResult.Radius,
                    buildingSolarRadiationResult.SolverVersion,
                    buildingSolarRadiationResult.WeatherSourceType,
                    buildingSolarRadiationResult.WeatherSourceId,
                    buildingSolarRadiationResult.WeatherPeriodStart,
                    buildingSolarRadiationResult.WeatherPeriodEnd,
                    email,
                    DateTimeOffset.UtcNow,
                    buildingSolarRadiationResult.SurfaceSolarRadiations);

                long? id = await buildingSolarRadiationPostgreSQLConverter.UpdateAsync(countyId, buildingSolarRadiationResult_Stamped, cancellationToken: cancellationToken);
                if (id is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "SetSolarRadiation: write for building model {Id} of part {CountyId} failed", buildingSolarRadiationResult.BuildingModelId, countyId);
                    return StatusCode(500, "Stored solar radiation write failed.");
                }

                Serilog.Modify.Log("SetSolarRadiation stored building model {Id} (reference {Reference}, part {CountyId}, radius {Radius} m, solver version {SolverVersion}) as row {RowId} for {User}", buildingSolarRadiationResult.BuildingModelId, reference, countyId, buildingSolarRadiationResult.Radius, buildingSolarRadiationResult.SolverVersion, id.Value, email);

                return Ok(id.Value);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingSolarRadiationController), nameof(SetSolarRadiationAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingSolarRadiationController), nameof(SetSolarRadiationAsync));
                return StatusCode(500, "Internal server error during stored solar radiation write");
            }
        }

        // The first reason the body cannot be stored, or null when it can: the members the key and the stored row need,
        // a defined weather source, a period in order, and every surface with finite values and a whole reference year
        // of shading factors.
        private static string? Rejection(GIS.Classes.BuildingSolarRadiationResult buildingSolarRadiationResult)
        {
            if (string.IsNullOrWhiteSpace(buildingSolarRadiationResult.Reference))
            {
                return "Reference is missing";
            }

            if (buildingSolarRadiationResult.CountyId <= 0)
            {
                return "CountyId is missing";
            }

            if (buildingSolarRadiationResult.BuildingModelId <= 0)
            {
                return "BuildingModelId is missing";
            }

            if (!double.IsFinite(buildingSolarRadiationResult.Radius) || buildingSolarRadiationResult.Radius <= 0)
            {
                return "Radius is missing or not a positive number";
            }

            if (buildingSolarRadiationResult.SolverVersion <= 0)
            {
                return "SolverVersion is missing";
            }

            if (buildingSolarRadiationResult.WeatherSourceType == GIS.Enums.WeatherSourceType.Undefined || !Enum.IsDefined(buildingSolarRadiationResult.WeatherSourceType))
            {
                return $"WeatherSourceType {(int)buildingSolarRadiationResult.WeatherSourceType} is not a defined weather source";
            }

            if (string.IsNullOrWhiteSpace(buildingSolarRadiationResult.WeatherSourceId))
            {
                return "WeatherSourceId is missing";
            }

            if (buildingSolarRadiationResult.WeatherPeriodStart is DateTimeOffset weatherPeriodStart && buildingSolarRadiationResult.WeatherPeriodEnd is DateTimeOffset weatherPeriodEnd && weatherPeriodStart > weatherPeriodEnd)
            {
                return "WeatherPeriodStart is after WeatherPeriodEnd";
            }

            List<GIS.Classes.SurfaceSolarRadiation>? surfaceSolarRadiations = buildingSolarRadiationResult.SurfaceSolarRadiations;
            if (surfaceSolarRadiations is null || surfaceSolarRadiations.Count == 0)
            {
                return "SurfaceSolarRadiations is missing or empty";
            }

            for (int i = 0; i < surfaceSolarRadiations.Count; i++)
            {
                GIS.Classes.SurfaceSolarRadiation surfaceSolarRadiation = surfaceSolarRadiations[i];

                byte[]? shadingFactors = surfaceSolarRadiation.ShadingFactors;
                if (shadingFactors is null || shadingFactors.Length != Constants.SolarRadiation.HourCount)
                {
                    return $"surface {i} carries {shadingFactors?.Length ?? 0} shading factors instead of {Constants.SolarRadiation.HourCount}";
                }

                Geometry.Spatial.Classes.Vector3D? normal = surfaceSolarRadiation.Normal;
                double[] values = [surfaceSolarRadiation.Area, surfaceSolarRadiation.SkyVisibility, surfaceSolarRadiation.GroundVisibility, surfaceSolarRadiation.Irradiation, surfaceSolarRadiation.Beam, surfaceSolarRadiation.Diffuse, surfaceSolarRadiation.Ground, surfaceSolarRadiation.IrradiationUnshaded, surfaceSolarRadiation.Energy, normal?.X ?? 0, normal?.Y ?? 0, normal?.Z ?? 0];
                foreach (double value in values)
                {
                    if (!double.IsFinite(value))
                    {
                        return $"surface {i} carries a non-finite number";
                    }
                }
            }

            return null;
        }
    }
}
