using DiGi.Geometry.Planar.Classes;
using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.WebAPI.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Web API controller for administrative area 2D operations, providing endpoints to retrieve, filter, and update administrative area data.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class AdministrativeAreal2DController : WebAPIController
    {
        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;
        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        /// <summary> Initializes a new instance of the <see cref="AdministrativeAreal2DController"/> class. </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The configuration file watcher for the GIS PostgreSQL Web API.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used for administrative area 2D PostgreSQL operations.</param>
        public AdministrativeAreal2DController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
        }

        /// <summary> Gets an administrative area reference by its code and type. </summary>
        /// <param name="code">The unique code of the administrative area.</param>
        /// <param name="administrativeArealType">The optional type of the administrative area to filter the search. Omit to resolve the code without a type filter.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("administrativeareal2Dreferencebycode", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferenceByCodeAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(AdministrativeAreal2DReference), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferenceByCodeAsync([BindRequired, FromQuery(Name = "code")] string? code, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferenceByCodeAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid code or administrative areal type provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            AdministrativeAreal2DReference? administrativeAreal2DReference = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByCodeAsync(code, administrativeArealType, cancellationToken: cancellationToken);
            string? json = Core.Convert.ToSystem_String(administrativeAreal2DReference);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves an administrative area reference by its identifier. </summary>
        /// <param name="id">The unique identifier of the administrative area reference to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("administrativeareal2Dreferencebyid", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferenceByIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(AdministrativeAreal2DReference), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferenceByIdAsync([BindRequired, Minimum(1), FromQuery(Name = "id")] int id, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferenceByIdAsync));
            Serilog.Modify.Log("Id provided: {Id}", id);

            if (id <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid id provided: {Id}", id);
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            AdministrativeAreal2DReference? administrativeAreal2DReference = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByIdAsync(id, cancellationToken: cancellationToken);

            string? json = Core.Convert.ToSystem_String(administrativeAreal2DReference);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves the administrative area reference path by its identifier. </summary>
        /// <param name="id">The unique identifier of the administrative area reference path to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("administrativeareal2Dreferencepathbyid", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencePathByIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(AdministrativeAreal2DReferencePath), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencePathByIdAsync([BindRequired, Minimum(1), FromQuery(Name = "id")] int id, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencePathByIdAsync));
            Serilog.Modify.Log("Id provided: {Id}", id);

            if (id <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid id provided: {Id}", id);
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            AdministrativeAreal2DReferencePath? administrativeAreal2DReferencePath = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencePathAsync(id, cancellationToken: cancellationToken);
            string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferencePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area reference paths by name as a query string. </summary>
        /// <remarks> Case-insensitive substring match on the administrative area name. Equivalent to <c>POST administrativeareal2Dreferencepathsbynameparameter</c> without the <c>_type</c> discriminator. </remarks>
        /// <param name="text">The search text used to find matching administrative area reference paths. Required.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("referencepathsbyname", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetReferencePathsByNameAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReferencePath>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReferencePathsByNameAsync([BindRequired, FromQuery(Name = "text")] string text, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetReferencePathsByNameAsync));
            Serilog.Modify.Log("Text provided: {Text}", text ?? string.Empty);

            if (string.IsNullOrWhiteSpace(text))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Empty or whitespace text provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<AdministrativeAreal2DReferencePath>? administrativeAreal2DReferencePaths = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencePathsByNameAsync(text, cancellationToken: cancellationToken);

                if (administrativeAreal2DReferencePaths is null)
                {
                    return NotFound();
                }

                string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferencePaths);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Error in GetReferencePathsByNameAsync");
                return StatusCode(500, "An error occurred while processing the geospatial data.");
            }
        }

        /// <summary> Retrieves administrative area references by name as a query string. </summary>
        /// <remarks> Case-insensitive substring match on the administrative area name. Equivalent to <c>POST administrativeareal2Dreferencesbynameparameter</c> without the <c>_type</c> discriminator. </remarks>
        /// <param name="text">The search text used to find matching administrative area references. Required.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("referencesbyname", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetReferencesByNameAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReferencesByNameAsync([BindRequired, FromQuery(Name = "text")] string text, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetReferencesByNameAsync));
            Serilog.Modify.Log("Text provided: {Text}", text ?? string.Empty);

            if (string.IsNullOrWhiteSpace(text))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Empty or whitespace text provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByNameAsync(text, cancellationToken: cancellationToken);

                if (administrativeAreal2DReferences is null)
                {
                    return NotFound();
                }

                string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferences);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Error in GetReferencesByNameAsync");
                return StatusCode(500, "An error occurred while processing the geospatial data.");
            }
        }

        /// <summary> Retrieves administrative area reference paths by name. </summary>
        /// <remarks> Use <c>GET referencepathsbyname?text=...</c> for a simpler query-string form without the <c>_type</c> body discriminator. </remarks>
        /// <param name="text">The search text used to find matching administrative area reference paths.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("administrativeareal2Dreferencepathsbyname", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencePathsByNameAsync)}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReferencePath>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencePathsByNameAsync([FromBody] string text, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencePathsByNameAsync));

            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<AdministrativeAreal2DReferencePath>? administrativeAreal2DReferencePaths = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencePathsByNameAsync(text, cancellationToken: cancellationToken);

                if (administrativeAreal2DReferencePaths is null)
                {
                    return NotFound();
                }

                string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferencePaths);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Error in GetAdministrativeAreal2DReferencePathsByNameAsync");
                return StatusCode(500, "An error occurred while processing the geospatial data.");
            }
        }

        /// <summary> Retrieves administrative area reference paths by name parameter. </summary>
        /// <param name="administrativeAreal2DReferencePathsByNameParameter">The parameter containing the search term for querying administrative areas by name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("administrativeareal2Dreferencepathsbynameparameter", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencePathsByNameParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReferencePath>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencePathsByNameParameterAsync([FromBody] AdministrativeAreal2DReferencePathsByNameParameter administrativeAreal2DReferencePathsByNameParameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencePathsByNameParameterAsync));

            if (string.IsNullOrWhiteSpace(administrativeAreal2DReferencePathsByNameParameter?.Text))
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<AdministrativeAreal2DReferencePath>? administrativeAreal2DReferencePaths = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencePathsByNameAsync(administrativeAreal2DReferencePathsByNameParameter.Text, cancellationToken: cancellationToken);

                if (administrativeAreal2DReferencePaths is null)
                {
                    return NotFound();
                }

                string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferencePaths);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Error in GetAdministrativeAreal2DReferencePathsByNameParameterAsync");
                return StatusCode(500, "An error occurred while processing the geospatial data.");
            }
        }

        /// <summary> Retrieves administrative area references by name parameter. </summary>
        /// <remarks> Use <c>GET referencesbyname?text=...</c> for a simpler query-string form without the <c>_type</c> body discriminator. </remarks>
        /// <param name="administrativeAreal2DReferencesByNameParameter">The parameter containing the search term for querying administrative area references by name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("administrativeareal2Dreferencesbynameparameter", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencesByNameParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencesByNameParameterAsync([FromBody] AdministrativeAreal2DReferencesByNameParameter administrativeAreal2DReferencesByNameParameter, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencesByNameParameterAsync));

            if (string.IsNullOrWhiteSpace(administrativeAreal2DReferencesByNameParameter?.Text))
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            try
            {
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByNameAsync(administrativeAreal2DReferencesByNameParameter.Text, cancellationToken: cancellationToken);

                if (administrativeAreal2DReferences is null)
                {
                    return NotFound();
                }

                string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferences);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (Exception exception) when (exception is not NpgsqlException { IsTransient: true })
            {
                Serilog.Modify.Log(exception, "Error in GetAdministrativeAreal2DReferencesByNameParameterAsync");
                return StatusCode(500, "An error occurred while processing the geospatial data.");
            }
        }

        /// <summary> Retrieves all administrative area references filtered by administrative area type. </summary>
        /// <param name="administrativeArealType">The administrative area type used to filter the references. Bound as nullable so an omitted parameter can be rejected: a non-nullable binding would silently take <see cref="AdministrativeArealType.Country"/>, because that is <c>default</c> of the enum while <see cref="AdministrativeArealType.Undefined"/> is -1.</param>
        /// <param name="parentId">The optional parent identifier used for filtering.</param>
        /// <param name="uniqueCode">An optional flag indicating whether to filter by unique code.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("administrativeareal2Dreferencesbyadministrativearealtype", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync([BindRequired, FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, [Minimum(1), FromQuery(Name = "parentId")] int? parentId, [FromQuery(Name = "uniquecode")] bool? uniqueCode, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync));
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);
            Serilog.Modify.Log("ParentId provided: {ParentId}", parentId?.ToString() ?? string.Empty);

            if (administrativeArealType is null || administrativeArealType.Value == AdministrativeArealType.Undefined || (parentId.HasValue && parentId.Value <= 0))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid AdministrativeArealType or ParentId provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(administrativeArealType.Value, parentId, uniqueCode ?? false, 30, cancellationToken: cancellationToken);
            string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferences);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area references by their code. </summary>
        /// <param name="code">The unique identifier or code used to retrieve the administrative area references.</param>
        /// <param name="administrativeArealType">An optional filter specifying the type of administrative area.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("administrativeareal2Dreferencesbycode", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencesByCodeAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencesByCodeAsync([BindRequired, FromQuery(Name = "code")] string code, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencesByCodeAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid code or administrative areal type provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = null;

            if (administrativeArealType != null)
            {
                administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByCodeAsync(code, administrativeArealType.Value, cancellationToken: cancellationToken);
            }
            else
            {
                administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByCodeAsync(code, cancellationToken: cancellationToken);
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferences);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            Serilog.Modify.Log("Number of AdministrativeAreal2DReferences to be returned: {Count}", administrativeAreal2DReferences!.Count);
            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area references by a list of identifiers. </summary>
        /// <param name="ids">The list of unique identifiers of the administrative areas to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("administrativeareal2Dreferencesbyids", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetAdministrativeAreal2DReferencesByIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<AdministrativeAreal2DReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdministrativeAreal2DReferencesByIdsAsync([FromBody] List<int> ids, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetAdministrativeAreal2DReferencesByIdsAsync));
            Serilog.Modify.Log("Ids count: {Count}", ids?.Count ?? 0);

            if (ids is null || ids.Count == 0)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByIdsAsync(ids, cancellationToken: cancellationToken);
            string? json = Core.Convert.ToSystem_String(administrativeAreal2DReferences);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Asynchronously retrieves the 2D bounding box enclosing country administrative areas. </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by the called method to indicate that the operation should be canceled.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("boundingbox2D", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetBoundingBox2DAsync)}")]
        [ProducesResponseType(typeof(BoundingBox2D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBoundingBox2DAsync(CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetBoundingBox2DAsync));

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            BoundingBox2D? boundingBox2D = await administrativeAreal2DPostgreSQLConverter.GetBoundingBox2DAsync(cancellationToken: cancellationToken);

            string? json = Core.Convert.ToSystem_String(boundingBox2D);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves all available administrative area codes. </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("codes", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetCodesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCodesAsync()
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetCodesAsync));

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            HashSet<string>? codes = await administrativeAreal2DPostgreSQLConverter.GetCodesAsync();
            if (codes is null || codes.Count == 0)
            {
                return NotFound();
            }

            JsonArray jsonArray = [.. codes];

            string? json = jsonArray.ToJsonString();
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves the count of administrative areas. </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("count", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetCountAsync)}")]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCountAsync(CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetCountAsync));

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            long count = await administrativeAreal2DPostgreSQLConverter.GetCountAsync(cancellationToken: cancellationToken);
            if (count < 0)
            {
                return NotFound();
            }

            return Ok(count);
        }

        /// <summary> Retrieves the identifier for a given code. </summary>
        /// <param name="code">The unique code of the administrative area.</param>
        /// <param name="administrativeArealType">The optional type of the administrative area to filter the search.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("idbycode", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetIdByCodeAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetIdByCodeAsync([BindRequired, FromQuery(Name = "code")] string code, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetIdByCodeAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            int? id = await administrativeAreal2DPostgreSQLConverter.GetIdByCodeAsync(code, administrativeArealType);
            if (id is null || !id.HasValue)
            {
                return NotFound();
            }

            return Ok(id.Value);
        }

        /// <summary> Retrieves all identifiers for a given code. </summary>
        /// <param name="code">The code of the administrative area.</param>
        /// <param name="administrativeArealType">The optional type of the administrative area to filter the search.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("idsbycode", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetIdsByCodeAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(HashSet<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetIdsByCodeAsync([BindRequired, FromQuery(Name = "code")] string code, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetIdsByCodeAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            HashSet<int>? ids = await administrativeAreal2DPostgreSQLConverter.GetIdsByCodeAsync(code, administrativeArealType, cancellationToken: cancellationToken);
            if (ids is null || ids.Count == 0)
            {
                return NotFound();
            }

            return Ok(ids);
        }

        /// <summary> Retrieves all identifiers for a given administrative area type. </summary>
        /// <param name="administrativeArealType">The administrative area type. Bound as nullable so an omitted parameter can be rejected: a non-nullable binding would silently take <see cref="AdministrativeArealType.Country"/>, because that is <c>default</c> of the enum while <see cref="AdministrativeArealType.Undefined"/> is -1.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("idsbyadministrativearealtype", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetIdsByAdministrativeArealTypeAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(HashSet<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetIdsByAdministrativeArealTypeAsync([BindRequired, FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetIdsByAdministrativeArealTypeAsync));
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (administrativeArealType is null || administrativeArealType.Value == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            HashSet<int>? ids = await administrativeAreal2DPostgreSQLConverter.GetIdsAsync(administrativeArealType.Value, cancellationToken: cancellationToken);
            if (ids is null || ids.Count == 0)
            {
                return NotFound();
            }

            return Ok(ids);
        }

        /// <summary> Retrieves an administrative area item by its code and optional type. </summary>
        /// <param name="code">The unique code of the administrative area to retrieve.</param>
        /// <param name="administrativeArealType">The optional type of the administrative area to filter the search.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itembycode", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemByCodeAsync)}")]
        [ProducesResponseType(typeof(GIS.Classes.AdministrativeAreal2D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemByCodeAsync([BindRequired, FromQuery(Name = "code")] string code, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemByCodeAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid code or administrative areal type provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            AdministrativeAreal2D? administrativeAreal2D_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DByCodeAsync(code, administrativeArealType, cancellationToken: cancellationToken);
            if (administrativeAreal2D_PostgreSQL is null)
            {
                return NotFound();
            }

            GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
            string? json = Core.Convert.ToSystem_String(administrativeAreal2D);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Asynchronously retrieves an administrative area item by its unique identifier. </summary>
        /// <param name="id">The integer identifier of the administrative area item to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itembyid", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemByIdAsync)}")]
        [ProducesResponseType(typeof(GIS.Classes.AdministrativeAreal2D), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemByIdAsync([BindRequired, Minimum(1), FromQuery(Name = "id")] int id, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemByIdAsync));
            Serilog.Modify.Log("Id provided: {Id}", id);

            if (id <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid id provided: {Id}", id);
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            AdministrativeAreal2D? administrativeAreal2D_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DByIdAsync(id);
            if (administrativeAreal2D_PostgreSQL is null)
            {
                return NotFound();
            }

            GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
            string? json = Core.Convert.ToSystem_String(administrativeAreal2D);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves all administrative area items filtered by administrative area type. </summary>
        /// <param name="administrativeArealType">The administrative area type used to filter the results. Bound as nullable so an omitted parameter can be rejected: a non-nullable binding would silently take <see cref="AdministrativeArealType.Country"/>, because that is <c>default</c> of the enum while <see cref="AdministrativeArealType.Undefined"/> is -1.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbyadministrativearealtype", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByAdministrativeArealTypeAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByAdministrativeArealTypeAsync([BindRequired, FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByAdministrativeArealTypeAsync));
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (administrativeArealType is null || administrativeArealType.Value == AdministrativeArealType.Undefined)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Invalid AdministrativeArealType provided");
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByAdministrativeArealTypeAsync(administrativeArealType.Value);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                Serilog.Modify.Log("No content found");
                return NotFound();
            }

            Serilog.Modify.Log("Content found: {Count} items", administrativeAreal2Ds_PostgreSQL.Count);

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                Serilog.Modify.Log("No content found");
                return NotFound();
            }

            Serilog.Modify.Log("{Count} items converted to GIS", administrativeAreal2Ds.Count);

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area items within a specified bounding box. </summary>
        /// <param name="x_1">The X-coordinate of the first corner of the bounding box.</param>
        /// <param name="y_1">The Y-coordinate of the first corner of the bounding box.</param>
        /// <param name="x_2">The X-coordinate of the second corner of the bounding box.</param>
        /// <param name="y_2">The Y-coordinate of the second corner of the bounding box.</param>
        /// <param name="tolerance">An optional tolerance value for the spatial query. If not provided, a default macro distance is used.</param>
        /// <param name="administrativeArealType">An optional filter to restrict results to a specific type of administrative area.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbyboundingbox", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByBoundingBoxAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByBoundingBoxAsync([BindRequired, FromQuery(Name = "x_1")] double x_1, [BindRequired, FromQuery(Name = "y_1")] double y_1, [BindRequired, FromQuery(Name = "x_2")] double x_2, [BindRequired, FromQuery(Name = "y_2")] double y_2, [FromQuery(Name = "tolerance")] double? tolerance, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByBoundingBoxAsync));
            Serilog.Modify.Log("BoundingBox provided: X_1={X_1}, Y_1={Y_1}, X_2={X_2}, Y_2={Y_2}", x_1, y_1, x_2, y_2);
            Serilog.Modify.Log("Tolerance provided: {Tolerance}", tolerance?.ToString() ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (double.IsNaN(x_1) || double.IsNaN(y_1) || double.IsNaN(x_2) || double.IsNaN(y_2) || (tolerance.HasValue && (double.IsNaN(tolerance.Value) || tolerance.Value < 0)) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            if (tolerance is null || double.IsNaN(tolerance.Value))
            {
                tolerance = Core.Constants.Tolerance.MacroDistance;
            }

            List<AdministrativeArealType>? administrativeArealTypes = null;
            if (administrativeArealType != null)
            {
                administrativeArealTypes = [administrativeArealType.Value];
            }

            BoundingBox2D boundingBox2D = new(new Core.Classes.Range<double>(x_1, x_2), new Core.Classes.Range<double>(y_1, y_2));
            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByBoundingBox2DAsync(boundingBox2D, administrativeArealTypes, tolerance.Value, cancellationToken: cancellationToken);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area items within a specified circle. </summary>
        /// <param name="x">The X-coordinate of the center point of the search circle.</param>
        /// <param name="y">The Y-coordinate of the center point of the search circle.</param>
        /// <param name="radius">The radius of the search circle, in metres. Capped at <see cref="Constants.Terrain.MaximumRadius"/>.</param>
        /// <param name="diameter">The diameter of the search circle, in metres, used only when <paramref name="radius"/> is absent. Capped at twice <see cref="Constants.Terrain.MaximumRadius"/>.</param>
        /// <param name="tolerance">The tolerance value for the spatial query.</param>
        /// <param name="administrativeArealType">The type of administrative area to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="IActionResult" /> containing a list of administrative area items if found, or an error response.</returns>
        [HttpGet("itemsbycircle", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByCircleAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByCircleAsync([BindRequired, FromQuery(Name = "x")] double x, [BindRequired, FromQuery(Name = "y")] double y, [Range(0.0, Constants.Terrain.MaximumRadius), FromQuery(Name = "radius")] double? radius, [Range(0.0, 2 * Constants.Terrain.MaximumRadius), FromQuery(Name = "diameter")] double? diameter, [FromQuery(Name = "tolerance")] double? tolerance, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByCircleAsync));
            Serilog.Modify.Log("Coordinates provided: X={X}, Y={Y}", x, y);
            Serilog.Modify.Log("Radius provided: {Radius}, Diameter provided: {Diameter}", radius?.ToString() ?? string.Empty, diameter?.ToString() ?? string.Empty);
            Serilog.Modify.Log("Tolerance provided: {Tolerance}", tolerance?.ToString() ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (double.IsNaN(x) || double.IsNaN(y) || (tolerance.HasValue && (double.IsNaN(tolerance.Value) || tolerance.Value < 0)) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
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

            List<AdministrativeArealType>? administrativeArealTypes = null;
            if (administrativeArealType != null)
            {
                administrativeArealTypes = [administrativeArealType.Value];
            }

            Point2D point2D = new(x, y);
            Circle2D circle2D = new(point2D, radius_Temp);
            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByCircle2DAsync(circle2D, administrativeArealTypes, tolerance.Value);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area items filtered by code. </summary>
        /// <param name="code">The code used to filter the administrative area items.</param>
        /// <param name="administrativeArealType">The optional type of administrative area to filter by.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>An <see cref="IActionResult" /> containing a list of matching administrative area items, or an error response if the code is invalid or no items are found.</returns>
        [HttpGet("itemsbycode", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByCodeAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByCodeAsync([BindRequired, FromQuery(Name = "code")] string code, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByCodeAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByCodeAsync(code, administrativeArealType, cancellationToken: cancellationToken);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area items filtered by multiple codes. </summary>
        /// <param name="codes">The list of codes used to filter the administrative area items.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("itemsbycodes", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByCodesAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByCodesAsync([FromBody] List<string> codes, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByCodesAsync));
            Serilog.Modify.Log("Codes count: {Count}", codes?.Count ?? 0);

            if (codes == null || codes.Count == 0)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByCodesAsync(codes);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area items filtered by a list of identifiers. </summary>
        /// <param name="ids">The list of identifiers used to retrieve the administrative area items.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("itemsbyids", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByIdsAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByIdsAsync([FromBody] List<int> ids, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByIdsAsync));
            Serilog.Modify.Log("Ids count: {Count}", ids?.Count ?? 0);

            if (ids is null || ids.Count == 0)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByIdsAsync(ids);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves administrative area items at or near a specified point. </summary>
        /// <param name="x">The X-coordinate of the search point.</param>
        /// <param name="y">The Y-coordinate of the search point.</param>
        /// <param name="tolerance">The optional tolerance distance to use when searching for items near the specified point. If null, a default macro distance is used.</param>
        /// <param name="administrativeArealType">The optional type filter for the administrative area items to be retrieved.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("itemsbypoint", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetItemsByPointAsync)}")]
        [ProducesResponseType(typeof(List<GIS.Classes.AdministrativeAreal2D>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetItemsByPointAsync([BindRequired, FromQuery(Name = "x")] double x, [BindRequired, FromQuery(Name = "y")] double y, [FromQuery(Name = "tolerance")] double? tolerance, [FromQuery(Name = "administrativearealtype")] AdministrativeArealType? administrativeArealType, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetItemsByPointAsync));
            Serilog.Modify.Log("Coordinates provided: X={X}, Y={Y}", x, y);
            Serilog.Modify.Log("Tolerance provided: {Tolerance}", tolerance?.ToString() ?? string.Empty);
            Serilog.Modify.Log("AdministrativeArealType provided: {AdministrativeArealType}", administrativeArealType?.ToString() ?? string.Empty);

            if (double.IsNaN(x) || double.IsNaN(y) || (tolerance.HasValue && (double.IsNaN(tolerance.Value) || tolerance.Value < 0)) || administrativeArealType == AdministrativeArealType.Undefined)
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            if (tolerance is null || double.IsNaN(tolerance.Value))
            {
                tolerance = Core.Constants.Tolerance.MacroDistance;
            }

            List<AdministrativeArealType>? administrativeArealTypes = null;
            if (administrativeArealType != null)
            {
                administrativeArealTypes = [administrativeArealType.Value];
            }

            Point2D point2D = new(x, y);
            List<AdministrativeAreal2D>? administrativeAreal2Ds_PostgreSQL = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DsByPoint2DAsync(point2D, administrativeArealTypes, tolerance.Value);
            if (administrativeAreal2Ds_PostgreSQL is null || administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NotFound();
            }

            List<GIS.Classes.AdministrativeAreal2D> administrativeAreal2Ds = [];
            foreach (AdministrativeAreal2D administrativeAreal2D_PostgreSQL in administrativeAreal2Ds_PostgreSQL)
            {
                GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = administrativeAreal2D_PostgreSQL.ToDiGi();
                if (administrativeAreal2D is null)
                {
                    continue;
                }

                administrativeAreal2Ds.Add(administrativeAreal2D);
            }

            if (administrativeAreal2Ds.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(administrativeAreal2Ds);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Retrieves subcodes for a given code. </summary>
        /// <param name="code">The administrative area code used to retrieve the associated subcodes.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("subcodes", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(GetSubCodesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSubCodesAsync([BindRequired, FromQuery(Name = "code")] string code, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(GetSubCodesAsync));
            Serilog.Modify.Log("Code provided: {Code}", code ?? string.Empty);

            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            HashSet<string>? subcodes = await administrativeAreal2DPostgreSQLConverter.GetSubCodesAsync(code, cancellationToken: cancellationToken);
            if (subcodes is null || subcodes.Count == 0)
            {
                return NotFound();
            }

            JsonArray jsonArray = [.. subcodes];

            string? json = jsonArray.ToJsonString();
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary> Updates a single administrative area item. </summary>
        /// <param name="jsonObject">The <see cref="JsonObject" /> containing the data used to update the administrative area item.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("updateitem", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(UpdateItemAsync)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemAsync([FromBody] JsonObject? jsonObject, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(UpdateItemAsync));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AdministrativeAreal2D update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateAdministrativeAreal2D)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AdministrativeAreal2D update not allowed");
                return Unauthorized();
            }

            if (jsonObject is null)
            {
                return NoContent();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            GIS.Classes.AdministrativeAreal2D? administrativeAreal2D = Core.Create.SerializableObject<GIS.Classes.AdministrativeAreal2D>(jsonObject);
            if (administrativeAreal2D is null)
            {
                return BadRequest();
            }

            AdministrativeAreal2D? administrativeAreal2D_PostgreSQL = administrativeAreal2D.ToPostgreSQL();
            if (administrativeAreal2D_PostgreSQL is null)
            {
                return BadRequest();
            }

            HashSet<int>? ids = await administrativeAreal2DPostgreSQLConverter.UpdateAsync([administrativeAreal2D_PostgreSQL]);
            if (ids is null || ids.Count == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary> Updates multiple administrative area items. </summary>
        /// <param name="jsonArray">The JSON array containing the administrative area items to be updated.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("updateitems", Name = $"{nameof(AdministrativeAreal2DController)}_{nameof(UpdateItemsAsync)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItemsAsync([FromBody] JsonArray? jsonArray, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(AdministrativeAreal2DController), nameof(UpdateItemsAsync));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AdministrativeAreal2D update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateAdministrativeAreal2D)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "AdministrativeAreal2D update not allowed");
                return Unauthorized();
            }

            if (jsonArray is null || jsonArray.Count == 0)
            {
                return NoContent();
            }

            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return BadRequest();
            }

            List<GIS.Classes.AdministrativeAreal2D>? administrativeAreal2Ds = Core.Create.SerializableObjects<GIS.Classes.AdministrativeAreal2D>(jsonArray);
            if (administrativeAreal2Ds is null)
            {
                return BadRequest();
            }

            List<AdministrativeAreal2D> administrativeAreal2Ds_PostgreSQL = [];
            foreach (GIS.Classes.AdministrativeAreal2D administrativeAreal2D in administrativeAreal2Ds)
            {
                AdministrativeAreal2D? administrativeAreal2D_PostgreSQL = administrativeAreal2D.ToPostgreSQL();
                if (administrativeAreal2D_PostgreSQL is null)
                {
                    continue;
                }

                administrativeAreal2Ds_PostgreSQL.Add(administrativeAreal2D_PostgreSQL);
            }

            if (administrativeAreal2Ds_PostgreSQL.Count == 0)
            {
                return NoContent();
            }

            HashSet<int>? ids = await administrativeAreal2DPostgreSQLConverter.UpdateAsync(administrativeAreal2Ds_PostgreSQL);
            if (ids is null || ids.Count == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
