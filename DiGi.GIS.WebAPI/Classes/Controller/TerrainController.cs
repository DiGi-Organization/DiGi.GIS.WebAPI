using DiGi.Geometry.Planar;
using DiGi.Geometry.Planar.Classes;
using DiGi.Geometry.PointCloud.Core.Enums;
using DiGi.Geometry.PointCloud.Spatial;
using DiGi.Geometry.PointCloud.Spatial.Classes;
using DiGi.Geometry.Spatial.Classes;
using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Controller responsible for handling API requests related to terrain, reconstructing a ground surface mesh from the stored elevation points of the counties a request covers.
    /// <para>Every mesh returned here is a two-and-a-half dimensional height field: exactly one elevation per plan position. It models ground, and cannot express a vertical face, an overhang or a canopy.</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class TerrainController : DiGi.WebAPI.Classes.WebAPIController
    {
        /// <summary>
        /// The edge of one work tile of a coverage walk, counted in lattice steps.
        /// <para>The default the sampling task writes with, so a coverage tile lines up with a sampled tile and a shortfall is reported against the same batches the run worked in.</para>
        /// </summary>
        private const int tileSize = 128;

        private readonly TerrainPointPostgreSQLConverter terrainPointPostgreSQLConverter;
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainController"/> class.
        /// </summary>
        /// <param name="terrainPointPostgreSQLConverter">The converter used for reading terrain points from the PostgreSQL database.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used for resolving which counties an area covers.</param>
        public TerrainController(TerrainPointPostgreSQLConverter terrainPointPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.terrainPointPostgreSQLConverter = terrainPointPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <summary>
        /// Asynchronously retrieves the terrain surface inside a circle centred on the given plan coordinate.
        /// <para>The circle is honoured: the points outside it are excluded by the database, not trimmed afterwards, so no part of the returned mesh lies further from the centre than the radius.</para>
        /// <para>Either <paramref name="radius"/> or <paramref name="diameter"/> must be supplied; <paramref name="radius"/> wins when both are. The radius is capped by <see cref="Constants.Terrain.MaximumRadius"/>.</para>
        /// </summary>
        /// <param name="x">The X coordinate of the centre, in PL-1992 (EPSG:2180) metres, matching the coordinates the terrain points are stored in.</param>
        /// <param name="y">The Y coordinate of the centre, in PL-1992 (EPSG:2180) metres, matching the coordinates the terrain points are stored in.</param>
        /// <param name="radius">The search radius in metres. Optional when <paramref name="diameter"/> is supplied.</param>
        /// <param name="diameter">The search diameter in metres, used only when <paramref name="radius"/> is absent.</param>
        /// <param name="tolerance">An optional tolerance for the spatial query, in metres. If not provided or NaN, a default macro distance is used.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the <see cref="Mesh3D"/> as JSON, or an error status.</returns>
        [HttpGet("mesh3dbycircle", Name = $"{nameof(TerrainController)}_{nameof(GetMesh3DByCircleAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(Mesh3D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMesh3DByCircleAsync([BindRequired, FromQuery(Name = "x")] double x, [BindRequired, FromQuery(Name = "y")] double y, [Range(0.0, Constants.Terrain.MaximumRadius), FromQuery(Name = "radius")] double? radius, [Range(0.0, 2 * Constants.Terrain.MaximumRadius), FromQuery(Name = "diameter")] double? diameter, [FromQuery(Name = "tolerance")] double? tolerance, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(TerrainController), nameof(GetMesh3DByCircleAsync));
            Serilog.Modify.Log("Coordinates provided: X={X}, Y={Y}", x, y);

            if (!IsFinite(x) || !IsFinite(y))
            {
                return BadRequest();
            }

            double radius_Temp = double.NaN;
            if (radius.HasValue && !double.IsNaN(radius.Value))
            {
                radius_Temp = radius.Value;
            }
            else if (diameter.HasValue && !double.IsNaN(diameter.Value))
            {
                radius_Temp = diameter.Value / 2;
            }

            // A radius that is absent, negative, zero, not a number, infinite or beyond the cap all end
            // here as bad input. Left to reach the database the last four produce an empty result or a
            // national scan, and both used to be reported as nothing found.
            if (!IsFinite(radius_Temp) || radius_Temp <= 0 || radius_Temp > Constants.Terrain.MaximumRadius)
            {
                return BadRequest();
            }

            if (!TryGetTolerance(tolerance, out double tolerance_Temp))
            {
                return BadRequest();
            }

            Serilog.Modify.Log("Radius resolved: {Radius}, Tolerance: {Tolerance}", radius_Temp, tolerance_Temp);

            Circle2D circle2D = new(new Point2D(x, y), radius_Temp);

            BoundingBox2D? boundingBox2D = circle2D.GetBoundingBox();
            if (boundingBox2D is null)
            {
                return BadRequest();
            }

            PointCloud3D? pointCloud3D;
            try
            {
                if (!await TerrainPointTableExistsAsync())
                {
                    Serilog.Modify.Log("No terrain point table exists, so nothing has ever been sampled");
                    return NotFound();
                }

                HashSet<int>? countyIds = await CountyIdsAsync(boundingBox2D, tolerance_Temp, cancellationToken);
                if (countyIds is null)
                {
                    Serilog.Modify.Log("No county covers the requested area");
                    return NotFound();
                }

                pointCloud3D = await terrainPointPostgreSQLConverter.GetPointCloud3DByCircle2DAsync(circle2D, countyIds, tolerance_Temp, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetMesh3DByCircleAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            return Mesh3DResult(pointCloud3D);
        }

        /// <summary>
        /// Asynchronously retrieves the terrain surface inside an axis aligned bounding box given by two opposite corners.
        /// <para>Corner order does not matter. Each side of the box is capped at twice <see cref="Constants.Terrain.MaximumRadius"/>, so this endpoint and the circle admit the same largest area.</para>
        /// </summary>
        /// <param name="x_1">The X coordinate of the first corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="y_1">The Y coordinate of the first corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="x_2">The X coordinate of the second corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="y_2">The Y coordinate of the second corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="tolerance">An optional tolerance for the spatial query, in metres. If not provided or NaN, a default macro distance is used.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the <see cref="Mesh3D"/> as JSON, or an error status.</returns>
        [HttpGet("mesh3dbyboundingbox", Name = $"{nameof(TerrainController)}_{nameof(GetMesh3DByBoundingBoxAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(Mesh3D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMesh3DByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [FromQuery(Name = "tolerance")] double? tolerance, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(TerrainController), nameof(GetMesh3DByBoundingBoxAsync));
            Serilog.Modify.Log("BoundingBox provided: X_1={X_1}, Y_1={Y_1}, X_2={X_2}, Y_2={Y_2}", x_1, y_1, x_2, y_2);

            if (!IsFinite(x_1) || !IsFinite(y_1) || !IsFinite(x_2) || !IsFinite(y_2))
            {
                return BadRequest();
            }

            double extent_Maximum = 2 * Constants.Terrain.MaximumRadius;
            if (System.Math.Abs(x_2 - x_1) > extent_Maximum || System.Math.Abs(y_2 - y_1) > extent_Maximum)
            {
                return BadRequest();
            }

            if (!TryGetTolerance(tolerance, out double tolerance_Temp))
            {
                return BadRequest();
            }

            BoundingBox2D boundingBox2D = new(new Core.Classes.Range<double>(x_1, x_2), new Core.Classes.Range<double>(y_1, y_2));

            PointCloud3D? pointCloud3D;
            try
            {
                if (!await TerrainPointTableExistsAsync())
                {
                    Serilog.Modify.Log("No terrain point table exists, so nothing has ever been sampled");
                    return NotFound();
                }

                HashSet<int>? countyIds = await CountyIdsAsync(boundingBox2D, tolerance_Temp, cancellationToken);
                if (countyIds is null)
                {
                    Serilog.Modify.Log("No county covers the requested area");
                    return NotFound();
                }

                pointCloud3D = await terrainPointPostgreSQLConverter.GetPointCloud3DByBoundingBox2DAsync(boundingBox2D, countyIds, tolerance_Temp, cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetMesh3DByBoundingBoxAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            return Mesh3DResult(pointCloud3D);
        }

        /// <summary>
        /// Asynchronously retrieves the number of terrain points stored for one county partition.
        /// <para>The cheapest question that can be asked of the store. A county part that holds no points answers 0; only an identifier that is not a county part answers 404.</para>
        /// </summary>
        /// <param name="countyId">The identifier of the county partition to count.</param>
        /// <param name="estimated">Reads the planner's row estimate instead of counting the rows. Far faster on a partition of millions and accurate to a few percent, but it reflects the last time the partition was analysed rather than this moment. An unanalysed partition returns 204 NoContent.</param>
        /// <param name="analyze">A boolean value indicating whether to perform an ANALYZE operation before reading the estimate to ensure statistics are current.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the count, 0 for a county part that has no partition (it holds no rows), 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the identifier is not a county part.</returns>
        [HttpGet("countbycountyid", Name = $"{nameof(TerrainController)}_{nameof(GetCountByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountByCountyIdAsync([BindRequired, FromQuery(Name = "countyid")] int countyId, [FromQuery(Name = "estimated")] bool estimated = false, [FromQuery(Name = "analyze")] bool analyze = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId}", nameof(TerrainController), nameof(GetCountByCountyIdAsync), countyId);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            bool isCounty = false;
            long? count;
            try
            {
                count = estimated
                    ? await terrainPointPostgreSQLConverter.GetEstimatedCountAsync(countyId, analyze, commandTimeout, cancellationToken)
                    : await terrainPointPostgreSQLConverter.GetCountAsync(countyId, commandTimeout, cancellationToken);

                if (count is null || (!estimated && count < 0))
                {
                    isCounty = await administrativeAreal2DPostgreSQLConverter.IsCountyAsync(countyId, commandTimeout, cancellationToken);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetCountByCountyIdAsync));
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
                if (isCounty)
                {
                    Serilog.Modify.Log("County {CountyId} has no terrain point partition, so it holds no rows", countyId);
                    return Ok(0L);
                }

                Serilog.Modify.Log("County {CountyId} is not a county part", countyId);
                return NotFound();
            }

            if (estimated && count < 0)
            {
                Serilog.Modify.Log("County {CountyId} terrain point partition exists but has not been analysed", countyId);
                return NoContent();
            }

            return Ok(count.Value);
        }

        /// <summary>
        /// Asynchronously summarises what each of the named county partitions holds: how many points, over what extent, at what elevations, filed under how many subdivisions, and when they were written.
        /// <para>The account a sampling run leaves behind. The run keeps its tallies in memory and discards them when it ends, so this is what remains to read afterwards - and ordering the result by <see cref="TerrainPointCountyResult.CreatedAt_First"/> reconstructs how far a run got, because a run walks the counties in ascending identifier order.</para>
        /// <para>Naming no county summarises every partition. Counties holding no point are absent from the result rather than present with a zero.</para>
        /// </summary>
        /// <param name="countyIds">The identifiers of the county partitions to summarise, repeated once per county. Omit to summarise every one.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the summaries as JSON, or an error status.</returns>
        [HttpGet("summariesbycountyids", Name = $"{nameof(TerrainController)}_{nameof(GetSummariesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<TerrainPointCountyResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummariesByCountyIdsAsync([FromQuery(Name = "countyids")] List<int>? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {CountyCount} counties", nameof(TerrainController), nameof(GetSummariesByCountyIdsAsync), countyIds?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            List<TerrainPointCountyResult>? terrainPointCountyResults;
            try
            {
                terrainPointCountyResults = await terrainPointPostgreSQLConverter.GetSummariesByCountyIdsAsync(countyIds is null || countyIds.Count == 0 ? null : countyIds, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetSummariesByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (terrainPointCountyResults is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(terrainPointCountyResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            Serilog.Modify.Log("Number of TerrainPointCountyResults to be returned: {Count}", terrainPointCountyResults.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously reports how densely each of the named county partitions is sampled: the points it holds divided by the area of its subdivisions.
        /// <para>The cheap sweep. It costs one aggregate per partition and the outlines of the counties named, where deciding the same question node by node costs the generating and the looking up of the whole lattice - so this is what narrows a country down to the few counties worth <see cref="GetCoverageByCountyIdAsync"/>.</para>
        /// <para>Supplying <paramref name="gridSize"/> is what turns the density into a completeness. Without it the figure to read is the spacing, which needs no knowledge of what a run was asked for.</para>
        /// </summary>
        /// <param name="countyIds">The identifiers of the county partitions to measure, repeated once per county. At least one and at most <see cref="Constants.Terrain.MaximumDensityCountyCount"/>.</param>
        /// <param name="gridSize">The lattice spacing a sampling run used, in metres, when it is known. Strictly greater than zero when supplied.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the densities as JSON, or an error status.</returns>
        [HttpGet("densitiesbycountyids", Name = $"{nameof(TerrainController)}_{nameof(GetDensitiesByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<TerrainPointDensityResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDensitiesByCountyIdsAsync([BindRequired, FromQuery(Name = "countyids")] List<int>? countyIds, [Minimum(0, Exclusive = true), FromQuery(Name = "gridsize")] double? gridSize, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for {CountyCount} counties", nameof(TerrainController), nameof(GetDensitiesByCountyIdsAsync), countyIds?.Count ?? 0);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (countyIds is null || countyIds.Count == 0 || countyIds.Count > Constants.Terrain.MaximumDensityCountyCount)
            {
                return BadRequest();
            }

            if (gridSize.HasValue && (!IsFinite(gridSize.Value) || gridSize.Value <= 0))
            {
                return BadRequest();
            }

            List<TerrainPointDensityResult> terrainPointDensityResults = [];
            try
            {
                Dictionary<int, long>? counts_ByCountyId = await terrainPointPostgreSQLConverter.GetCountsByCountyIdsAsync(countyIds, commandTimeout, cancellationToken);
                if (counts_ByCountyId is null)
                {
                    return NotFound();
                }

                foreach (int countyId in countyIds)
                {
                    Dictionary<int, PolygonalFace2D>? polygonalFace2Ds_ById = await PolygonalFace2DsByIdAsync(countyId, cancellationToken);
                    if (polygonalFace2Ds_ById is null)
                    {
                        Serilog.Modify.Log("County {CountyId} has no subdivision outline, so there is no area to measure a density against", countyId);
                        continue;
                    }

                    double area = 0;
                    foreach (PolygonalFace2D polygonalFace2D in polygonalFace2Ds_ById.Values)
                    {
                        area += polygonalFace2D.GetArea();
                    }

                    // A county with no partition is absent from the counts and contributes a zero here, which is
                    // the answer wanted: a county a run never reached reports a density of nothing rather than
                    // dropping out of a sweep that was asked about it.
                    counts_ByCountyId.TryGetValue(countyId, out long count);

                    if (PostgreSQL.Create.TerrainPointDensityResult(countyId, count, area, gridSize) is TerrainPointDensityResult terrainPointDensityResult)
                    {
                        terrainPointDensityResults.Add(terrainPointDensityResult);
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetDensitiesByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            string? json = Core.Convert.ToSystem_String(terrainPointDensityResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            Serilog.Modify.Log("Number of TerrainPointDensityResults to be returned: {Count}", terrainPointDensityResults.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously compares what one county partition holds against what a sampling run on the given lattice should have put there.
        /// <para>The question a density cannot answer. A density says how much of a county is missing; this says which nodes, so a run that stepped over a batch can be sent back for exactly those.</para>
        /// <para>The expected nodes are derived from the same subdivision outlines and the same lattice the sampling run itself decides against, so the two agree by construction. Nodes of the county's bounding rectangle that fall outside its land are not expected and not counted.</para>
        /// </summary>
        /// <param name="countyId">The identifier of the county partition to measure.</param>
        /// <param name="gridSize">The lattice spacing, in metres. Not finer than <see cref="Constants.Terrain.MinimumGridSize"/>.</param>
        /// <param name="originX">The X coordinate the lattice is anchored at. Leave at zero unless a run used something else.</param>
        /// <param name="originY">The Y coordinate the lattice is anchored at. Leave at zero unless a run used something else.</param>
        /// <param name="tolerance">The distance a stored point may lie from a node and still be counted as that node, in metres. Capped at half a step.</param>
        /// <param name="limit">The largest number of missing coordinates returned. The count itself is reported in full regardless.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the <see cref="TerrainPointCoverageResult"/> as JSON, or an error status.</returns>
        [HttpGet("coveragebycountyid", Name = $"{nameof(TerrainController)}_{nameof(GetCoverageByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(TerrainPointCoverageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCoverageByCountyIdAsync([BindRequired, FromQuery(Name = "countyid")] int countyId, [BindRequired, Minimum(Constants.Terrain.MinimumGridSize), FromQuery(Name = "gridsize")] double gridSize, [FromQuery(Name = "originx")] double originX, [FromQuery(Name = "originy")] double originY, [FromQuery(Name = "tolerance")] double? tolerance, [Minimum(0), FromQuery(Name = "limit")] int limit = 1000, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId} at grid {GridSize}", nameof(TerrainController), nameof(GetCoverageByCountyIdAsync), countyId, gridSize);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (!TryGetLatticeParameters(gridSize, originX, originY, tolerance, limit, out Point2D? origin, out double tolerance_Temp) || origin is null)
            {
                return BadRequest();
            }

            TerrainPointCoverageResult? terrainPointCoverageResult;
            try
            {
                if (!await TerrainPointTableExistsAsync())
                {
                    Serilog.Modify.Log("No terrain point table exists, so nothing has ever been sampled");
                    return NotFound();
                }

                Dictionary<int, PolygonalFace2D>? polygonalFace2Ds_ById = await PolygonalFace2DsByIdAsync(countyId, cancellationToken);
                if (polygonalFace2Ds_ById is null)
                {
                    Serilog.Modify.Log("County {CountyId} has no subdivision outline, so there is nothing to measure a coverage against", countyId);
                    return NotFound();
                }

                terrainPointCoverageResult = await terrainPointPostgreSQLConverter.GetCoverageByCountyIdAsync(countyId, polygonalFace2Ds_ById, null, gridSize, origin, tolerance_Temp, limit, Constants.Terrain.MaximumNodeCount, tileSize, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetCoverageByCountyIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            // A null result is the node ceiling rather than an absent county: the county was found, and the
            // lattice asked for over its extent would have generated more nodes than a single request may.
            if (terrainPointCoverageResult is null)
            {
                Serilog.Modify.Log("County {CountyId} at grid {GridSize} exceeds the {MaximumNodeCount} node ceiling", countyId, gridSize, Constants.Terrain.MaximumNodeCount);
                return BadRequest();
            }

            string? json = Core.Convert.ToSystem_String(terrainPointCoverageResult);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            Serilog.Modify.Log("County {CountyId}: {ExpectedCount} expected, {StoredCount} stored, {MissingCount} missing", countyId, terrainPointCoverageResult.ExpectedCount, terrainPointCoverageResult.StoredCount, terrainPointCoverageResult.MissingCount);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the lattice nodes inside a rectangle that lie on a county's land and that the terrain point table holds no point for.
        /// <para>Where <see cref="GetCoverageByCountyIdAsync"/> answers for a whole county, this answers for an area - which is what a coverage reporting a shortfall is followed by. Every county the rectangle meets is measured, so a hole spanning a county boundary is reported once and whole.</para>
        /// </summary>
        /// <param name="x_1">The X coordinate of the first corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="y_1">The Y coordinate of the first corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="x_2">The X coordinate of the second corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="y_2">The Y coordinate of the second corner, in PL-1992 (EPSG:2180) metres.</param>
        /// <param name="gridSize">The lattice spacing, in metres. Not finer than <see cref="Constants.Terrain.MinimumGridSize"/>.</param>
        /// <param name="originX">The X coordinate the lattice is anchored at. Leave at zero unless a run used something else.</param>
        /// <param name="originY">The Y coordinate the lattice is anchored at. Leave at zero unless a run used something else.</param>
        /// <param name="tolerance">The distance a stored point may lie from a node and still be counted as that node, in metres. Capped at half a step.</param>
        /// <param name="limit">The largest number of missing coordinates returned.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the missing coordinates as JSON, or an error status.</returns>
        [HttpGet("gapsbyboundingbox", Name = $"{nameof(TerrainController)}_{nameof(GetGapsByBoundingBoxAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Point2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGapsByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [BindRequired, Minimum(Constants.Terrain.MinimumGridSize), FromQuery(Name = "gridsize")] double gridSize, [FromQuery(Name = "originx")] double originX, [FromQuery(Name = "originy")] double originY, [FromQuery(Name = "tolerance")] double? tolerance, [Minimum(0), FromQuery(Name = "limit")] int limit = 1000, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started at grid {GridSize}", nameof(TerrainController), nameof(GetGapsByBoundingBoxAsync), gridSize);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (!IsFinite(x_1) || !IsFinite(y_1) || !IsFinite(x_2) || !IsFinite(y_2))
            {
                return BadRequest();
            }

            if (Math.Abs(x_2 - x_1) > Constants.Terrain.MaximumGapExtent || Math.Abs(y_2 - y_1) > Constants.Terrain.MaximumGapExtent)
            {
                return BadRequest();
            }

            if (!TryGetLatticeParameters(gridSize, originX, originY, tolerance, limit, out Point2D? origin, out double tolerance_Temp) || origin is null)
            {
                return BadRequest();
            }

            BoundingBox2D boundingBox2D = new(new Core.Classes.Range<double>(x_1, x_2), new Core.Classes.Range<double>(y_1, y_2));

            List<Point2D> point2Ds_Missing = [];
            try
            {
                if (!await TerrainPointTableExistsAsync())
                {
                    Serilog.Modify.Log("No terrain point table exists, so nothing has ever been sampled");
                    return NotFound();
                }

                HashSet<int>? countyIds = await CountyIdsAsync(boundingBox2D, tolerance_Temp, cancellationToken);
                if (countyIds is null)
                {
                    Serilog.Modify.Log("No county covers the requested area");
                    return NotFound();
                }

                foreach (int countyId in countyIds)
                {
                    if (point2Ds_Missing.Count >= limit)
                    {
                        break;
                    }

                    Dictionary<int, PolygonalFace2D>? polygonalFace2Ds_ById = await PolygonalFace2DsByIdAsync(countyId, cancellationToken);
                    if (polygonalFace2Ds_ById is null)
                    {
                        continue;
                    }

                    TerrainPointCoverageResult? terrainPointCoverageResult = await terrainPointPostgreSQLConverter.GetCoverageByCountyIdAsync(countyId, polygonalFace2Ds_ById, boundingBox2D, gridSize, origin, tolerance_Temp, limit - point2Ds_Missing.Count, Constants.Terrain.MaximumNodeCount, tileSize, commandTimeout, cancellationToken);
                    if (terrainPointCoverageResult is null)
                    {
                        Serilog.Modify.Log("The requested area at grid {GridSize} exceeds the {MaximumNodeCount} node ceiling", gridSize, Constants.Terrain.MaximumNodeCount);
                        return BadRequest();
                    }

                    point2Ds_Missing.AddRange(terrainPointCoverageResult.Point2Ds_Missing);
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(TerrainController), nameof(GetGapsByBoundingBoxAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            string? json = Core.Convert.ToSystem_String(point2Ds_Missing);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            Serilog.Modify.Log("Number of missing nodes to be returned: {Count}", point2Ds_Missing.Count);
            return Content(json, "application/json");
        }

        /// <summary>
        /// Resolves which county partitions an area covers.
        /// <para>This has to happen here rather than inside the terrain converter. The terrain points live in the Storage database and the administrative geometry in Main, and a PostgreSQL connection cannot reach across databases - so the only place both are available is the host, where each converter carries its own connection. The write side has always worked this way; see <c>PostgreSQLTerrainPointCreateTableTask</c>.</para>
        /// </summary>
        /// <param name="boundingBox2D">The area to resolve.</param>
        /// <param name="tolerance">The distance the search area is expanded by, in metres.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>The identifiers of the counties the area meets, or <see langword="null"/> when it meets none.</returns>
        private async Task<HashSet<int>?> CountyIdsAsync(BoundingBox2D boundingBox2D, double tolerance, CancellationToken cancellationToken)
        {
            // Every part of every county reaching the box, not only the parts the box overlaps. A county
            // with disconnected territory is stored as one row per part and each part is a partition of
            // its own, so a point can be filed under a part whose polygon is nowhere near it; pruning to
            // the overlapping parts alone then answers nothing and reads as "nothing sampled here". This
            // is the same resolution the building reads use - see
            // https://github.com/DiGi-Organization/DiGi.GIS.PostgreSQL/issues/64.
            HashSet<int>? countyIds = await administrativeAreal2DPostgreSQLConverter.GetCountyIdsByBoundingBox2DAsync(boundingBox2D, tolerance, cancellationToken: cancellationToken);

            return countyIds is null || countyIds.Count == 0 ? null : countyIds;
        }

        /// <summary>
        /// Reconstructs the ground surface from the gathered terrain points and renders it as the JSON response body.
        /// <para>The three failure paths are reported separately, because "nothing stored here" and "too little stored here to triangulate" are answered by different fixes and both used to arrive as a bare 404.</para>
        /// </summary>
        /// <param name="pointCloud3D">The terrain points gathered for the requested area.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the <see cref="Mesh3D"/> as JSON, or a not found status.</returns>
        private IActionResult Mesh3DResult(PointCloud3D? pointCloud3D)
        {
            if (pointCloud3D is null || pointCloud3D.Count == 0)
            {
                Serilog.Modify.Log("No TerrainPoints stored for the requested area");
                return NotFound();
            }

            // A decimation grid finer than the source spacing walks every point and removes none, and the
            // stored lattice is already regular at 10 m or coarser, so there is nothing to decimate. With no
            // decimation the height selection never runs; Lowest is passed because it is what this path would
            // want - bare ground rather than a surface model of canopy and roofs - if a grid were ever set.
            // No fixed edge limit: a factor stops Delaunay bridging county edges and no-data gaps with a skirt
            // of long thin triangles, without opening a hole around a point the store happens to be missing.
            HeightFieldPointCloud3DMeshSolver heightFieldPointCloud3DMeshSolver = new(0, 0, PointCloudHeightSelection.Lowest, edgeLengthFactor: Constants.Terrain.EdgeLengthFactor);

            Mesh3D? mesh3D = pointCloud3D.Mesh3D(heightFieldPointCloud3DMeshSolver);
            if (mesh3D is null)
            {
                Serilog.Modify.Log("TerrainPoints found ({Count}) but none could be triangulated. A triangulation needs three points, so a radius below the lattice spacing lands here", pointCloud3D.Count);
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(mesh3D);
            if (string.IsNullOrWhiteSpace(json))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Mesh3D could not be serialized");
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Reports whether anything has ever been written to the terrain point store.
        /// <para>Asked once per request, before a walk that would otherwise send a query per tile against a table
        /// that does not exist. An undefined relation reaches a caller as a server fault, where the plain fact is
        /// that nothing has been sampled yet - which is an answer, and one a fresh deployment gives.</para>
        /// </summary>
        /// <returns><see langword="true"/> when the terrain point table exists; otherwise <see langword="false"/>.</returns>
        private async Task<bool> TerrainPointTableExistsAsync()
        {
            return await DiGi.PostgreSQL.Query.TableExistsAsync(terrainPointPostgreSQLConverter.ConnectionData, PostgreSQL.Constants.TableName.TerrainPoint);
        }

        /// <summary>
        /// Reads the outlines of a county's subdivisions, keyed by subdivision identifier.
        /// <para>Read from the administrative database rather than the terrain one, and derived through the same helper the sampling task uses, so an area measured here is the area a run would have sampled.</para>
        /// </summary>
        /// <param name="countyId">The identifier of the county whose subdivisions are wanted.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>The outlines keyed by subdivision identifier, or <see langword="null"/> when the county has none.</returns>
        private async Task<Dictionary<int, PolygonalFace2D>?> PolygonalFace2DsByIdAsync(int countyId, CancellationToken cancellationToken)
        {
            List<AdministrativeAreal2D>? administrativeAreal2Ds = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByAdministrativeArealType(AdministrativeArealType.Subdivision, countyId, cancellationToken: cancellationToken);
            if (administrativeAreal2Ds is null || administrativeAreal2Ds.Count == 0)
            {
                return null;
            }

            Dictionary<int, PolygonalFace2D> polygonalFace2Ds_ById = administrativeAreal2Ds.PolygonalFace2DsById();
            if (polygonalFace2Ds_ById.Count == 0)
            {
                return null;
            }

            return polygonalFace2Ds_ById;
        }

        /// <summary>
        /// Resolves and checks the lattice a coverage or gap request asked to be measured against.
        /// </summary>
        /// <param name="gridSize">The lattice spacing as bound from the query string.</param>
        /// <param name="originX">The X coordinate the lattice is anchored at.</param>
        /// <param name="originY">The Y coordinate the lattice is anchored at.</param>
        /// <param name="tolerance">The tolerance as bound from the query string.</param>
        /// <param name="limit">The largest number of coordinates the request asked to be returned.</param>
        /// <param name="origin">The resolved anchor of the lattice.</param>
        /// <param name="tolerance_Temp">The resolved tolerance, capped at half a step so that a point can never be taken for a node of the neighbouring cell.</param>
        /// <returns><see langword="true"/> when the lattice is usable; otherwise <see langword="false"/>.</returns>
        private static bool TryGetLatticeParameters(double gridSize, double originX, double originY, double? tolerance, int limit, out Point2D? origin, out double tolerance_Temp)
        {
            origin = null;
            tolerance_Temp = Core.Constants.Tolerance.MacroDistance;

            if (!IsFinite(gridSize) || gridSize < Constants.Terrain.MinimumGridSize)
            {
                return false;
            }

            if (!IsFinite(originX) || !IsFinite(originY) || limit < 0)
            {
                return false;
            }

            if (!TryGetTolerance(tolerance, out tolerance_Temp))
            {
                return false;
            }

            // The same cap the sampling task applies. Anything larger would let a point of one cell answer for the
            // node of the next, and a coverage would report stored what is in fact missing.
            if (tolerance_Temp > gridSize / 2)
            {
                tolerance_Temp = gridSize / 2;
            }

            origin = new Point2D(originX, originY);

            return true;
        }


        /// <summary>
        /// Resolves the tolerance a request asked for, falling back to the default when it was not supplied.
        /// </summary>
        /// <param name="tolerance">The tolerance as bound from the query string.</param>
        /// <param name="tolerance_Temp">The resolved tolerance, in metres.</param>
        /// <returns><see langword="true"/> when the tolerance is usable; otherwise <see langword="false"/>.</returns>
        private static bool TryGetTolerance(double? tolerance, out double tolerance_Temp)
        {
            tolerance_Temp = Core.Constants.Tolerance.MacroDistance;
            if (tolerance.HasValue && !double.IsNaN(tolerance.Value))
            {
                tolerance_Temp = tolerance.Value;
            }

            return IsFinite(tolerance_Temp) && tolerance_Temp >= 0;
        }

        /// <summary>
        /// Determines whether a bound query string value is a usable coordinate or distance.
        /// <para>Model binding accepts the literals NaN and Infinity for a double, so neither is a value the caller could only have reached by mistake.</para>
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> when the value is neither NaN nor infinite; otherwise <see langword="false"/>.</returns>
        private static bool IsFinite(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }
    }
}
