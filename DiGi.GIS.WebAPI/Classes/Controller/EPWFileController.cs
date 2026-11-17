using DiGi.EPW.Classes;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.WebAPI.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Controller responsible for handling API requests related to EPW files retrieved from or updated in a PostgreSQL database.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class EPWFileController : WebAPIController
    {
        private readonly EPWFilePostgreSQLConverter ePWFilePostgreSQLConverter;
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EPWFileController"/> class.
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher used to monitor changes to the GIS PostgreSQL Web API settings.</param>
        /// <param name="ePWFilePostgreSQLConverter">The converter used for handling EPW file operations within the PostgreSQL database.</param>
        public EPWFileController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, EPWFilePostgreSQLConverter ePWFilePostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.ePWFilePostgreSQLConverter = ePWFilePostgreSQLConverter;
        }

        /// <summary>
        /// Asynchronously updates or inserts a collection of EPW files.
        /// </summary>
        /// <param name="jsonArray">The JSON array containing the EPW files to update or insert.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("updateitems", Name = $"{nameof(EPWFileController)}_{nameof(UpdateItemsAsync)}")]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsAsync([FromBody] JsonArray? jsonArray, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(EPWFileController), nameof(UpdateItemsAsync));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "EPWFile update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateEPWFile)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "EPWFile update not allowed");
                return Unauthorized();
            }

            if (jsonArray is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "EPWFiles cannot be null");
                return BadRequest();
            }

            List<EPWFile>? ePWFiles = Core.Create.SerializableObjects<EPWFile>(jsonArray);
            if (ePWFiles is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "EPWFiles could not be deserialized");
                return BadRequest();
            }

            int count_Before = ePWFiles.Count;
            if (count_Before == 0)
            {
                Serilog.Modify.Log("No EPWFiles to update");
                return NoContent();
            }

            Serilog.Modify.Log("Updating to database starting");

            List<int> ids = [];
            try
            {
                ids = await ePWFilePostgreSQLConverter.InsertAsync(ePWFiles, cancellationToken: cancellationToken);
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Database could not be updated");
                return StatusCode(500, "Internal server error during database update");
            }

            // The identifiers travel in the body, but no caller reads them: DiGi.WebAPI.Modify.PostAsync
            // forces PostOptions.RequestResult to false for this shape of POST, so success is decided by
            // the status code alone. An empty list behind a 200 is therefore as invisible as a bare Ok, and
            // the EPWFiles reached this point, so nothing updated is a failure rather than a quiet no-op.
            if (ids.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database ended but no EPWFiles have been updated");
                return StatusCode(500, "Database update returned no modified EPWFile IDs.");
            }

            Serilog.Modify.Log("Updating to database ended. Updated EPWFiles: {After}/{Before}", ids.Count, count_Before);

            return Ok(ids);
        }

        /// <summary>
        /// Asynchronously retrieves the closest EPWFile to the given coordinate (x, y).
        /// <para>The response header <c>DiGi-EPWFile-Id</c> (<see cref="Constants.Header.EPWFileId"/>) carries the id of the <c>epw_file</c> row the body holds, as an invariant integer, so a client can name the weather source of a stored solar radiation summary. The body is unchanged.</para>
        /// </summary>
        /// <param name="x">The X coordinate (longitude).</param>
        /// <param name="y">The Y coordinate (latitude).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("item", Name = $"{nameof(EPWFileController)}_{nameof(GetEPWFileAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(EPWFile), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEPWFileAsync([BindRequired, FromQuery(Name = "x")] double x, [BindRequired, FromQuery(Name = "y")] double y, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(EPWFileController), nameof(GetEPWFileAsync));
            Serilog.Modify.Log("Coordinates provided: X={X}, Y={Y}", x, y);

            if (ePWFilePostgreSQLConverter is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "EPWFilePostgreSQLConverter is null");
                return BadRequest();
            }

            try
            {
                (int Id, EPWFile EPWFile)? ePWFileWithId = await ePWFilePostgreSQLConverter.GetEPWFileWithIdAsync(x, y, cancellationToken: cancellationToken);
                if (ePWFileWithId is null)
                {
                    Serilog.Modify.Log("No EPWFile found for coordinates: X={X}, Y={Y}", x, y);
                    return NotFound();
                }

                string? json = ePWFileWithId.Value.EPWFile.ToJsonObject()?.ToJsonString();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                HttpContext.Response.Headers[Constants.Header.EPWFileId] = ePWFileWithId.Value.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(EPWFileController), nameof(GetEPWFileAsync));
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
        /// Asynchronously retrieves the EPW file of one <c>epw_file</c> row, addressed by its id.
        /// <para>The by-id counterpart of <see cref="GetEPWFileAsync"/>: the same body and the same <c>DiGi-EPWFile-Id</c> header (<see cref="Constants.Header.EPWFileId"/>). A client that picked a station from <see cref="GetEPWFileLocationsAsync"/> reads each station it uses once, by id, instead of resolving the location of every building (DiGi.GIS.WebAPI#93). The id is bound nullable, so an omitted one is refused rather than read as 0 (<c>Coding - WebAPI Contracts.md</c> §2).</para>
        /// </summary>
        /// <param name="id">The id of the <c>epw_file</c> row.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the <see cref="EPWFile"/>; 400 for a missing or non-positive id or a negative timeout; 404 when no row has the id or the table does not exist; 503 for a transient database failure; or 500.</returns>
        [HttpGet("itembyid", Name = $"{nameof(EPWFileController)}_{nameof(GetEPWFileByIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(EPWFile), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEPWFileByIdAsync([FromQuery(Name = "id")] int? id, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for row {Id}", nameof(EPWFileController), nameof(GetEPWFileByIdAsync), id?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "-");

            if (id is null || id.Value <= 0 || commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: id is missing or not positive, or commandtimeout {CommandTimeout} is negative", nameof(EPWFileController), nameof(GetEPWFileByIdAsync), commandTimeout);
                return BadRequest();
            }

            // The converter answers null both for a missing row and for no connection; only the first is a 404 (Coding - WebAPI Contracts.md §3).
            if (ePWFilePostgreSQLConverter.ConnectionData is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name} failed: the EPW file converter has no connection data", nameof(EPWFileController), nameof(GetEPWFileByIdAsync));
                return StatusCode(500, "EPW file could not be read");
            }

            EPWFile? ePWFile;
            try
            {
                ePWFile = await ePWFilePostgreSQLConverter.GetEPWFileByIdAsync(id.Value, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (PostgresException postgresException) when (postgresException.SqlState == PostgresErrorCodes.UndefinedTable)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name}: table {TableName} does not exist", nameof(EPWFileController), nameof(GetEPWFileByIdAsync), EPWFilePostgreSQLConverter.TableName);
                return NotFound();
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(EPWFileController), nameof(GetEPWFileByIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed for row {Id}", nameof(EPWFileController), nameof(GetEPWFileByIdAsync), id.Value);
                return StatusCode(500, "Internal server error during database query");
            }

            if (ePWFile is null)
            {
                Serilog.Modify.Log("{Type}:{Name}: no row {Id}", nameof(EPWFileController), nameof(GetEPWFileByIdAsync), id.Value);
                return NotFound();
            }

            string? json = ePWFile.ToJsonObject()?.ToJsonString();
            if (string.IsNullOrWhiteSpace(json))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name}: the file of row {Id} could not be serialized", nameof(EPWFileController), nameof(GetEPWFileByIdAsync), id.Value);
                return StatusCode(500, "EPW file could not be serialized");
            }

            HttpContext.Response.Headers[Constants.Header.EPWFileId] = id.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the id and location of every EPW station, without the weather files.
        /// <para>A client positioning many buildings picks each one's station in memory from this list - the smallest planar distance, as <see cref="GetEPWFileAsync"/> and the server-side solar solve pick it (<c>Query.NearestEPWFileId</c>) - and then reads only the stations it uses with <see cref="GetEPWFileByIdAsync"/> (DiGi.GIS.WebAPI#93). Open to every visitor, like the by-location read.</para>
        /// </summary>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with the JSON list of <see cref="EPWFileLocationResult"/> in id order (empty when no station is stored); 400 for a negative timeout; 404 when the table does not exist; 503 for a transient database failure; or 500.</returns>
        [HttpGet("locations", Name = $"{nameof(EPWFileController)}_{nameof(GetEPWFileLocationsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<EPWFileLocationResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEPWFileLocationsAsync([Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} rejected: commandtimeout cannot be negative", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync));
                return BadRequest();
            }

            List<(int Id, double X, double Y)>? ePWFileLocations;
            try
            {
                ePWFileLocations = await ePWFilePostgreSQLConverter.GetEPWFileLocationsAsync(commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (PostgresException postgresException) when (postgresException.SqlState == PostgresErrorCodes.UndefinedTable)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name}: table {TableName} does not exist", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync), EPWFilePostgreSQLConverter.TableName);
                return NotFound();
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            // Null is a failure to reach the database, never "no station" - an empty list says that (Coding - WebAPI Contracts.md).
            if (ePWFileLocations is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name} failed: no connection to the database", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync));
                return StatusCode(500, "EPW station locations could not be read");
            }

            if (ePWFileLocations.Count == 0)
            {
                return Content("[]", "application/json");
            }

            List<EPWFileLocationResult> ePWFileLocationResults = [];
            foreach ((int Id, double X, double Y) ePWFileLocation in ePWFileLocations)
            {
                ePWFileLocationResults.Add(new EPWFileLocationResult(ePWFileLocation.Id, ePWFileLocation.X, ePWFileLocation.Y));
            }

            string? json = Core.Convert.ToSystem_String(ePWFileLocationResults);
            if (string.IsNullOrWhiteSpace(json))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "{Type}:{Name}: {Count} station locations could not be serialized", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync), ePWFileLocationResults.Count);
                return StatusCode(500, "EPW station locations could not be serialized");
            }

            Serilog.Modify.Log("{Type}:{Name} returns {Count} station locations", nameof(EPWFileController), nameof(GetEPWFileLocationsAsync), ePWFileLocationResults.Count);
            return Content(json, "application/json");
        }
    }
}