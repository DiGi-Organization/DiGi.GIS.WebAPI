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
    }
}