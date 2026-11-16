using DiGi.Core.IO.Table.Classes;
using DiGi.GIS.IO;
using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.PostgreSQL.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Controller responsible for handling API requests related to building data retrieved from a PostgreSQL database.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingDataController : DiGi.WebAPI.Classes.WebAPIController
    {
        /// <summary>
        /// The largest number of references one request may ask for.
        /// <para>The whole collection travels into a single statement, so an unbounded list is an unbounded statement. A caller with more than this to ask about should page rather than widen the request.</para>
        /// </summary>
        private const int referenceCount_Maximum = 10000;

        private readonly GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher;

        private readonly BuildingDataPostgreSQLConverter buildingDataPostgreSQLConverter;

        private readonly Building2DPostgreSQLConverter building2DPostgreSQLConverter;

        private readonly AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter;

        private readonly AddressPointPostgreSQLConverter addressPointPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the BuildingDataController class.
        /// <para>All converters and the configuration watcher are taken on the one constructor, because the coverage read compares two tables that sit in different databases, <c>byaddress</c> reads a third, and write operations require authorization. A second constructor is not an option: a controller with more than one public constructor fails activation and answers 500 on every one of its endpoints.</para>
        /// </summary>
        /// <param name="GISWebAPIConfigurationFileWatcher">The <see cref="Classes.GISWebAPIConfigurationFileWatcher" /> used to verify authorization and permissions for write operations.</param>
        /// <param name="buildingDataPostgreSQLConverter">The <see cref="BuildingDataPostgreSQLConverter" /> used to handle building data operations and database conversions.</param>
        /// <param name="building2DPostgreSQLConverter">The <see cref="Building2DPostgreSQLConverter" /> used to read the buildings a county holds, which is the other half of the coverage comparison.</param>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The <see cref="AdministrativeAreal2DPostgreSQLConverter" /> used to widen a row no named part holds to every part of the named county.</param>
        /// <param name="addressPointPostgreSQLConverter">The <see cref="AddressPointPostgreSQLConverter" /> used by <c>byaddress</c> to search the address points the building data is read for.</param>
        public BuildingDataController(GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, BuildingDataPostgreSQLConverter buildingDataPostgreSQLConverter, Building2DPostgreSQLConverter building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, AddressPointPostgreSQLConverter addressPointPostgreSQLConverter)
        {
            this.GISWebAPIConfigurationFileWatcher = GISWebAPIConfigurationFileWatcher;
            this.buildingDataPostgreSQLConverter = buildingDataPostgreSQLConverter;
            this.building2DPostgreSQLConverter = building2DPostgreSQLConverter;
            this.administrativeAreal2DPostgreSQLConverter = administrativeAreal2DPostgreSQLConverter;
            this.addressPointPostgreSQLConverter = addressPointPostgreSQLConverter;
        }

        /// <summary>
        /// Resolves the optional <c>AdministrativeAreal2DId</c> of a filtered read to the one county partition and filter group the read runs on (DiGi-Organization/DiGi.GIS.WebAPI#83).
        /// <para>Without an area the caller's county and filter group pass through unchanged. With one, <see cref="Query.AdministrativeAreal2DScopesAsync"/> resolves it - a Subdivision through its polygon, a County as its own partition, a higher level through its Subdivision children - and the read must land on exactly one county partition: a table, aggregate or histogram cannot be merged across partitions, so an area spanning several answers 400 and points at <c>countbyadministrativeareal2did</c>, which sums across them.</para>
        /// </summary>
        /// <param name="countyId">The county partition the caller named; must be null when an area is named.</param>
        /// <param name="administrativeAreal2DId">The administrative area the caller named, or null.</param>
        /// <param name="filterGroup">The caller's filter group.</param>
        /// <param name="commandTimeout">The timeout in seconds for each area read.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The result carries either the answer to give instead of reading (400, 404 or 204), or the county partition and filter group to read with.</returns>
        private async Task<(IActionResult? ActionResult, int? CountyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup)> AdministrativeAreal2DScopeAsync(int? countyId, int? administrativeAreal2DId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup, int commandTimeout, CancellationToken cancellationToken)
        {
            if (administrativeAreal2DId is null)
            {
                return (null, countyId, filterGroup);
            }

            if (countyId is not null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyId and AdministrativeAreal2DId are mutually exclusive");
                return (BadRequest("CountyId and AdministrativeAreal2DId are mutually exclusive; send one of them."), null, null);
            }

            if (administrativeAreal2DId.Value <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AdministrativeAreal2DId is not positive");
                return (BadRequest("AdministrativeAreal2DId must be positive."), null, null);
            }

            (Enums.AdministrativeAreal2DCountStatus Status, List<AdministrativeAreal2DScope> AdministrativeAreal2DScopes) scopes = await Query.AdministrativeAreal2DScopesAsync(
                administrativeAreal2DId.Value,
                filterGroup,
                (id, token) => administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByIdAsync(id, commandTimeout, token),
                (id, token) => administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.Subdivision, id, commandTimeout: commandTimeout, cancellationToken: token),
                (id, token) => building2DPostgreSQLConverter.GetBuilding2DReferencesBySubdivisionIdAsync(id, commandTimeout, token),
                Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount,
                Constants.BuildingData.MaximumAdministrativeAreal2DReferenceCount,
                cancellationToken);

            switch (scopes.Status)
            {
                case Enums.AdministrativeAreal2DCountStatus.Counted:
                    if (scopes.AdministrativeAreal2DScopes.Count == 0)
                    {
                        Serilog.Modify.Log("Area resolved to no county partition; answering NoContent");
                        return (NoContent(), null, null);
                    }

                    if (scopes.AdministrativeAreal2DScopes.Count > 1)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Area spans {Count} county partitions", scopes.AdministrativeAreal2DScopes.Count);
                        return (BadRequest($"The administrative area spans {scopes.AdministrativeAreal2DScopes.Count} county partitions; this read is confined to one. Narrow the area, or use countbyadministrativeareal2did, which sums across partitions."), null, null);
                    }

                    AdministrativeAreal2DScope administrativeAreal2DScope = scopes.AdministrativeAreal2DScopes[0];
                    return (null, administrativeAreal2DScope.CountyId, administrativeAreal2DScope.FilterGroup);

                case Enums.AdministrativeAreal2DCountStatus.NoSubdivisions:
                case Enums.AdministrativeAreal2DCountStatus.NoBuildings:
                    Serilog.Modify.Log("Area resolution found nothing to read by; answering NoContent");
                    return (NoContent(), null, null);

                case Enums.AdministrativeAreal2DCountStatus.TooManyCounties:
                    return (BadRequest($"The administrative area spans more than {Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount} counties; narrow the area."), null, null);

                case Enums.AdministrativeAreal2DCountStatus.TooManyReferences:
                    return (BadRequest($"The administrative area holds more than {Constants.BuildingData.MaximumAdministrativeAreal2DReferenceCount} buildings; narrow the area."), null, null);

                default:
                    Serilog.Modify.Log("Area not found, or no connection; answering NotFound");
                    return (NotFound(), null, null);
            }
        }

        /// <summary>
        /// Resolves the caller's <c>columns</c> query value to the projection a read runs with, or to the answer to give instead, before the read opens a connection (DiGi-Organization/DiGi.GIS.WebAPI#85).
        /// <para>Each element is split on commas, trimmed and de-duplicated, so a repeated key (<c>?columns=storeys&amp;columns=floor_area</c>) and one comma-separated value (<c>?columns=storeys,floor_area</c>) name the same projection.</para>
        /// <para><paramref name="columns"/> null means the caller named no projection: <paramref name="columnUniqueIds_Default"/> is then used, and a default column the stored table no longer holds is dropped with a warning, because a constant of this API must not be able to fail a request. A projection the caller did supply is answered strictly: an id the stored table does not hold, or an entry that names no id at all, is refused with 400 naming it, and a stored column list that could not be read is refused with 500. A projection is never checked silently and never silently narrowed.</para>
        /// </summary>
        /// <param name="columns">The value the caller sent, or null when none was sent.</param>
        /// <param name="columnUniqueIds_Default">The projection to read with when the caller sent none; null means every stored column.</param>
        /// <param name="commandTimeout">The timeout in seconds for the column list read. A value of 0 disables the timeout.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task whose result carries the answer to give instead (400 or 500), or the projection to read with - null meaning every stored column.</returns>
        private async Task<(IActionResult? ActionResult, List<string>? ColumnUniqueIds)> ColumnUniqueIdsAsync(List<string>? columns, List<string>? columnUniqueIds_Default, int commandTimeout, CancellationToken cancellationToken)
        {
            bool suppliedByCaller = columns is not null;
            List<string>? columnUniqueIds_Source = columns ?? columnUniqueIds_Default;
            if (columnUniqueIds_Source is null)
            {
                return (null, null);
            }

            List<string> columnUniqueIds = [];
            foreach (string? column in columnUniqueIds_Source)
            {
                foreach (string part in (column ?? string.Empty).Split(','))
                {
                    string part_Trimmed = part.Trim();
                    if (part_Trimmed.Length == 0)
                    {
                        if (suppliedByCaller)
                        {
                            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "A columns entry names no column id");
                            return (Problem(detail: "Every columns entry must name at least one column id; an empty id is refused.", statusCode: 400), null);
                        }

                        continue;
                    }

                    if (!columnUniqueIds.Contains(part_Trimmed, StringComparer.Ordinal))
                    {
                        columnUniqueIds.Add(part_Trimmed);
                    }
                }
            }

            if (columnUniqueIds.Count == 0)
            {
                if (suppliedByCaller)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Columns were sent but name no column id");
                    return (Problem(detail: "columns was sent but names no column id.", statusCode: 400), null);
                }

                return (null, null);
            }

            List<Column>? columns_Stored = await buildingDataPostgreSQLConverter.GetColumnsByUniqueIdsAsync(columnUniqueIds, commandTimeout, cancellationToken);
            if (columns_Stored is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The stored column list could not be read");
                return (Problem(detail: "The stored column list could not be read.", statusCode: 500), null);
            }

            HashSet<string> storedColumnUniqueIds = [];
            foreach (Column column_Stored in columns_Stored)
            {
                if (DiGi.PostgreSQL.Table.Query.UniqueId(column_Stored) is string uniqueId && !string.IsNullOrWhiteSpace(uniqueId))
                {
                    storedColumnUniqueIds.Add(uniqueId);
                }
            }

            List<string> columnUniqueIds_Unknown = [.. columnUniqueIds.Where(x => !storedColumnUniqueIds.Contains(x))];
            if (columnUniqueIds_Unknown.Count == 0)
            {
                return (null, columnUniqueIds);
            }

            if (suppliedByCaller)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The stored table does not hold the column(s): {Columns}", string.Join(", ", columnUniqueIds_Unknown));
                return (Problem(detail: $"The stored table does not hold: {string.Join(", ", columnUniqueIds_Unknown)}.", statusCode: 400), null);
            }

            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Default projection column(s) {Columns} are not stored and were dropped", string.Join(", ", columnUniqueIds_Unknown));
            columnUniqueIds.RemoveAll(x => columnUniqueIds_Unknown.Contains(x, StringComparer.Ordinal));
            if (columnUniqueIds.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "None of the default projection columns is stored");
                return (Problem(detail: "None of the default projection columns is stored.", statusCode: 500), null);
            }

            return (null, columnUniqueIds);
        }

        /// <summary>
        /// Builds the projected building data of one row: one key per column of the table, named by the stored column unique id and holding the row's value, or null where the row holds none.
        /// </summary>
        /// <param name="table">The projected table the row belongs to.</param>
        /// <param name="row">The row to convert.</param>
        /// <returns>The row as a <see cref="JsonObject"/>, or null when no table was read.</returns>
        private static JsonObject? ToJsonObject(Table? table, Row? row)
        {
            if (table is null || row is null)
            {
                return null;
            }

            JsonObject jsonObject = [];
            foreach (Column column in table.Columns)
            {
                if (DiGi.PostgreSQL.Table.Query.UniqueId(column) is not string uniqueId || string.IsNullOrWhiteSpace(uniqueId))
                {
                    continue;
                }

                jsonObject[uniqueId] = Core.Create.JsonNode(row[column.Index]);
            }

            return jsonObject;
        }

        /// <summary>
        /// Answers an address search with the projected building data of the matched buildings in one call: for each matched address point, the match itself and the stored columns the caller names (DiGi-Organization/DiGi.GIS.WebAPI#85).
        /// <para>Ranking and limits are those of <c>addresspoint/buildingsbyaddress</c> - similarity first, then an exact house number before a prefix match, from <see cref="Constants.BuildingData.ByAddressLimit_Minimum"/> to <see cref="Constants.BuildingData.ByAddressLimit_Maximum"/> matches - and the answer keeps that rank order. Every match carries its county part, so the building data is read one county partition at a time: the reads run one after another, never in parallel, because concurrent requests exhaust the Npgsql pool, and a question that spans two counties costs two scoped reads instead of one cross-partition scan.</para>
        /// <para><paramref name="columns"/> takes stored column unique ids as repeated query keys or as one comma-separated value and is validated like any other projection: an id the stored table does not hold answers 400 naming it, an entry naming no id answers 400, and a stored column list that could not be read answers 500. Omitted, it defaults to <see cref="Constants.BuildingData.ByAddressDefaultColumnUniqueIds"/>; a default column the stored table no longer holds is dropped with a warning rather than refused.</para>
        /// <para>Every item carries <c>reference</c>, and <c>county_id</c> as well because the read is always confined to one county part, so <c>BuildingData</c> repeats what its match already carries. A key whose value is null means the stored row holds no value for that column; a building with no building data row answers every projected key as null, and the answer does not separate the two.</para>
        /// </summary>
        /// <param name="text">The street or locality name to search for, as typed. Required; a text with no letter or digit is refused with 400.</param>
        /// <param name="houseNumber">The house number to narrow the result to, or null for any.</param>
        /// <param name="countyId">The county part id to narrow the address search to, or null for every county. Whichever it is, each match's own part is what its building data is read from.</param>
        /// <param name="limit">The maximum number of address matches, from 1 to 100. Out-of-range values are refused, not clamped.</param>
        /// <param name="columns">The projected building data columns, as repeated query keys or as one comma-separated value; null for the default projection.</param>
        /// <param name="commandTimeout">The timeout in seconds for each command - the address search and every building data read. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <returns>A task that represents the asynchronous operation. 200 with one item per matched address point in rank order (an empty array when none), 400 for a missing or empty text, a text that normalises to nothing, an out-of-range limit or county id, a negative timeout, an unknown column or an entry naming no column, 404 when a building data read could not run, 503 for a transient database failure, or 500.</returns>
        [HttpGet("byaddress", Name = $"{nameof(BuildingDataController)}_{nameof(GetByAddressAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<BuildingDataByAddressItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The text is missing or normalises to nothing, the limit or county id is out of range, the command timeout is negative, or the projection names a column the stored table does not hold.")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByAddressAsync([BindRequired, FromQuery(Name = "text")] string? text, [FromQuery(Name = "housenumber")] string? houseNumber, [Minimum(1), FromQuery(Name = "countyid")] int? countyId, [Range(Constants.BuildingData.ByAddressLimit_Minimum, Constants.BuildingData.ByAddressLimit_Maximum), FromQuery(Name = "limit")] int? limit = 20, [FromQuery(Name = "columns")] List<string>? columns = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetByAddressAsync));
            Serilog.Modify.Log("Text provided: {Text}, HouseNumber provided: {HouseNumber}, CountyId provided: {CountyId}, Limit provided: {Limit}", text ?? string.Empty, houseNumber ?? string.Empty, countyId?.ToString(CultureInfo.CurrentCulture) ?? string.Empty, limit?.ToString(CultureInfo.CurrentCulture) ?? string.Empty);

            if (string.IsNullOrWhiteSpace(text) || PostgreSQL.Query.SearchText(text) is null)
            {
                return BadRequest();
            }

            if (limit is null || limit.Value < Constants.BuildingData.ByAddressLimit_Minimum || limit.Value > Constants.BuildingData.ByAddressLimit_Maximum)
            {
                return BadRequest();
            }

            if (countyId is not null && countyId.Value <= 0)
            {
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                return BadRequest();
            }

            try
            {
                (IActionResult? actionResult_Columns, List<string>? columnUniqueIds) = await ColumnUniqueIdsAsync(columns, Constants.BuildingData.ByAddressDefaultColumnUniqueIds, commandTimeout, cancellationToken);
                if (actionResult_Columns is not null)
                {
                    return actionResult_Columns;
                }

                List<GIS.Classes.AddressPointMatch>? addressPointMatches = await addressPointPostgreSQLConverter.GetAddressPointMatchesByAddressAsync(text, houseNumber, countyId, limit.Value, commandTimeout, cancellationToken);
                if (addressPointMatches is null)
                {
                    // The text was proved to normalise above, so null here means no connection could be built - never a client error.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Address points could not be searched: no connection could be built");
                    return StatusCode(500, "Address points could not be searched.");
                }

                List<GIS.Classes.AddressPointMatch> addressPointMatches_Matched = [.. addressPointMatches.Where(x => !string.IsNullOrWhiteSpace(x?.Reference))];
                if (addressPointMatches_Matched.Count == 0)
                {
                    return Content(new JsonArray().ToJsonString(), "application/json");
                }

                Dictionary<string, JsonObject?> buildingData_ByReference = new(StringComparer.Ordinal);
                foreach (IGrouping<int?, GIS.Classes.AddressPointMatch> grouping in addressPointMatches_Matched.GroupBy(x => x.CountyId))
                {
                    List<string> references = [];
                    foreach (GIS.Classes.AddressPointMatch addressPointMatch in grouping)
                    {
                        if (addressPointMatch.Reference is string reference && !references.Contains(reference, StringComparer.Ordinal))
                        {
                            references.Add(reference);
                        }
                    }

                    // One county part at a time, one read after another: the match carries the part (DiGi-GIS.WebAPI#85 item 1), so the read is confined to that partition instead of scanning every partition the way countyidsbyreference has to.
                    Table? table = await buildingDataPostgreSQLConverter.PullAsync(references, grouping.Key, columnUniqueIds, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                    if (table is null)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Building data could not be read for county part {CountyId}", grouping.Key?.ToString(CultureInfo.CurrentCulture) ?? string.Empty);
                        return NotFound();
                    }

                    Column? column_Reference = null;
                    foreach (Column column in table.Columns)
                    {
                        if (string.Equals(DiGi.PostgreSQL.Table.Query.UniqueId(column), DiGi.GIS.IO.Constants.Column.Reference.UniqueId(), StringComparison.Ordinal))
                        {
                            column_Reference = column;
                            break;
                        }
                    }

                    if (column_Reference is null)
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The projected table holds no reference column");
                        return StatusCode(500, "The projected building data holds no reference column.");
                    }

                    foreach (Row row in table.Rows)
                    {
                        if (row[column_Reference.Index] is string reference_Row && reference_Row.Length != 0 && !buildingData_ByReference.ContainsKey(reference_Row))
                        {
                            buildingData_ByReference[reference_Row] = ToJsonObject(table, row);
                        }
                    }
                }

                List<BuildingDataByAddressItem> buildingDataByAddressItems = [];
                foreach (GIS.Classes.AddressPointMatch addressPointMatch in addressPointMatches_Matched)
                {
                    JsonObject? buildingData = null;
                    if (addressPointMatch.Reference is string reference_Match && buildingData_ByReference.TryGetValue(reference_Match, out JsonObject? buildingData_Temp))
                    {
                        buildingData = buildingData_Temp;
                    }

                    buildingDataByAddressItems.Add(new BuildingDataByAddressItem(addressPointMatch, buildingData));
                }

                string? json = Core.Convert.ToSystem_String(buildingDataByAddressItems);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return StatusCode(500, "The answer could not be serialized.");
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetByAddressAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetByAddressAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves all available building data column categories.
        /// </summary>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("categories", Name = $"{nameof(BuildingDataController)}_{nameof(GetCategoriesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesAsync([Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetCategoriesAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                HashSet<string>? categories = await buildingDataPostgreSQLConverter.GetCategoriesAsync(commandTimeout, cancellationToken);
                if (categories is null || categories.Count == 0)
                {
                    return NotFound();
                }

                JsonArray jsonArray = [.. categories];

                string? json = jsonArray.ToJsonString();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetCategoriesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetCategoriesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves all column references, optionally filtered by the specified categories.
        /// </summary>
        /// <param name="categories">An optional list of category names to filter the column references by.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning a list of column references.</returns>
        [HttpGet("columnreferences", Name = $"{nameof(BuildingDataController)}_{nameof(GetColumnReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<DiGi.PostgreSQL.Table.Classes.ColumnReference>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetColumnReferencesAsync([FromQuery(Name = "categories")] List<string>? categories = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetColumnReferencesAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                List<DiGi.PostgreSQL.Table.Classes.ColumnReference>? columnReferences = await buildingDataPostgreSQLConverter.GetColumnReferencesByCategoriesAsync(categories, commandTimeout, cancellationToken);
                if (columnReferences is null || columnReferences.Count == 0)
                {
                    return NotFound();
                }

                string? json = Core.Convert.ToSystem_String(columnReferences);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetColumnReferencesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetColumnReferencesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves all available column definitions for building data.
        /// </summary>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("columns", Name = $"{nameof(BuildingDataController)}_{nameof(GetColumnsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<DiGi.PostgreSQL.Table.Classes.Column>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetColumnsAsync([Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetColumnsAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                List<Column>? columns = await buildingDataPostgreSQLConverter.GetColumnsByCategoriesAsync(commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (columns is null || columns.Count == 0)
                {
                    return NotFound();
                }

                List<DiGi.PostgreSQL.Table.Classes.Column>? columns_PostgreSQL = DiGi.PostgreSQL.Table.Convert.ToDiGi(columns);

                string? json = Core.Convert.ToSystem_String(columns_PostgreSQL);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetColumnsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetColumnsAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves all columns filtered by the specified categories.
        /// </summary>
        /// <param name="categories">An optional list of category names to filter the columns by. If null, the filtering behavior is determined by the underlying data source.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("columnsbycategories", Name = $"{nameof(BuildingDataController)}_{nameof(GetColumnsByCategoriesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<DiGi.PostgreSQL.Table.Classes.Column>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetColumnsByCategoriesAsync([FromBody] List<string>? categories = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetColumnsByCategoriesAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                List<Column>? columns = await buildingDataPostgreSQLConverter.GetColumnsByCategoriesAsync(categories, commandTimeout, cancellationToken);
                if (columns is null || columns.Count == 0)
                {
                    return NotFound();
                }

                List<DiGi.PostgreSQL.Table.Classes.Column>? columns_PostgreSQL = DiGi.PostgreSQL.Table.Convert.ToDiGi(columns);

                string? json = Core.Convert.ToSystem_String(columns_PostgreSQL);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetColumnsByCategoriesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetColumnsByCategoriesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Retrieves all columns with given categories by columns by categories parameter (which contains categories).
        /// </summary>
        /// <param name="columnsByCategoriesParameter"> The parameter containing the categories for querying columns. </param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>Column <see cref="DiGi.PostgreSQL.Table.Classes.Column"/></returns>
        [HttpPost("columnsbycategoriesparameter", Name = $"{nameof(BuildingDataController)}_{nameof(GetColumnsByCategoriesParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<DiGi.PostgreSQL.Table.Classes.Column>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetColumnsByCategoriesParameterAsync([FromBody] ColumnsByCategoriesParameter columnsByCategoriesParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetColumnsByCategoriesParameterAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                List<Column>? columns = await buildingDataPostgreSQLConverter.GetColumnsByCategoriesAsync(columnsByCategoriesParameter.Categories, commandTimeout, cancellationToken);
                if (columns is null || columns.Count == 0)
                {
                    return NotFound();
                }

                List<DiGi.PostgreSQL.Table.Classes.Column>? columns_PostgreSQL = DiGi.PostgreSQL.Table.Convert.ToDiGi(columns);

                string? json = Core.Convert.ToSystem_String(columns_PostgreSQL);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetColumnsByCategoriesParameterAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetColumnsByCategoriesParameterAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Retrieves the unique identifiers for columns, optionally filtered by the specified categories.
        /// </summary>
        /// <param name="categories">An optional list of category names used to filter the column references.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("columnuniqueids", Name = $"{nameof(BuildingDataController)}_{nameof(GetColumnUniqueIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetColumnUniqueIdsAsync([FromBody] List<string>? categories = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetColumnUniqueIdsAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                List<DiGi.PostgreSQL.Table.Classes.ColumnReference>? columnReferences = await buildingDataPostgreSQLConverter.GetColumnReferencesByCategoriesAsync(categories, commandTimeout, cancellationToken);
                if (columnReferences is null || columnReferences.Count == 0)
                {
                    return NotFound();
                }

                List<string> columnUniqueIds = [];
                foreach (DiGi.PostgreSQL.Table.Classes.ColumnReference columnReference in columnReferences)
                {
                    if (columnReference.UniqueId is string columnUniqueId && !string.IsNullOrWhiteSpace(columnUniqueId))
                    {
                        columnUniqueIds.Add(columnUniqueId);
                    }
                }

                JsonArray jsonArray = [.. columnUniqueIds];

                string? json = jsonArray.ToJsonString();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetColumnUniqueIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetColumnUniqueIdsAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously retrieves the number of building data rows stored for one county.
        /// <para>The cheapest question that can be asked of the table. A county part that holds no rows answers 0, the same as <c>coveragebycountyid</c>; only an identifier that is not a county part answers 404.</para>
        /// </summary>
        /// <param name="countyId">The identifier of the county to count.</param>
        /// <param name="estimated">Reads the planner's row estimate instead of counting the rows. Far faster on a partition of millions and accurate to a few percent, but it reflects the last time the partition was analysed rather than this moment. An unanalysed partition returns 204 NoContent.</param>
        /// <param name="analyze">A boolean value indicating whether to perform an ANALYZE operation before reading the estimate to ensure statistics are current.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the count, 0 for a county part that has no partition (it holds no rows), 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the identifier is not a county part.</returns>
        [HttpGet("countbycountyid", Name = $"{nameof(BuildingDataController)}_{nameof(GetCountByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountByCountyIdAsync([BindRequired, FromQuery(Name = "countyid")] int countyId, [FromQuery(Name = "estimated")] bool estimated = false, [FromQuery(Name = "analyze")] bool analyze = false, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId}", nameof(BuildingDataController), nameof(GetCountByCountyIdAsync), countyId);

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
                    ? await buildingDataPostgreSQLConverter.GetEstimatedCountAsync(countyId, analyze, commandTimeout, cancellationToken)
                    : await buildingDataPostgreSQLConverter.GetCountAsync(countyId, commandTimeout, cancellationToken);

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
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetCountByCountyIdAsync));
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
                    Serilog.Modify.Log("County {CountyId} has no building data partition, so it holds no rows", countyId);
                    return Content("0", "application/json");
                }

                Serilog.Modify.Log("County {CountyId} is not a county part", countyId);
                return NotFound();
            }

            if (estimated && count < 0)
            {
                Serilog.Modify.Log("County {CountyId} building data partition exists but has not been analysed", countyId);
                return NoContent();
            }

            return Content(count.Value.ToString(CultureInfo.CurrentCulture), "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the counties whose building data holds a row for one reference.
        /// <para>A reference addresses one building of one county, so more than one identifier coming back means the reference was written outside the county it belongs to.</para>
        /// </summary>
        /// <param name="reference">The building reference to look up.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the county identifiers in ascending order, or 404 when the reference is not stored.</returns>
        [HttpGet("countyidsbyreference", Name = $"{nameof(BuildingDataController)}_{nameof(GetCountyIdsByReferenceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountyIdsByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for reference {Reference}", nameof(BuildingDataController), nameof(GetCountyIdsByReferenceAsync), reference ?? string.Empty);

            if (string.IsNullOrWhiteSpace(reference))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            List<int>? countyIds;
            try
            {
                countyIds = await buildingDataPostgreSQLConverter.GetCountyIdsByReferenceAsync(reference, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetCountyIdsByReferenceAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (countyIds is null || countyIds.Count == 0)
            {
                return NotFound();
            }

            JsonArray jsonArray = [.. countyIds];

            return Content(jsonArray.ToJsonString(), "application/json");
        }

        /// <summary>
        /// Asynchronously measures what one county's building data holds against the buildings that county actually has.
        /// <para>What a row count cannot answer: how much was left out, and how much of that no run could have reached. A shortfall larger than the unresolved subdivision count is a run that did not finish what it could have.</para>
        /// <para>Reads both databases - the buildings from the main one and their data from the storage one - so it costs more than a count. Call it per county rather than in a sweep.</para>
        /// </summary>
        /// <param name="countyId">The identifier of the county to measure.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the <see cref="BuildingDataCoverageResult"/>, or 404 when either side could not be read.</returns>
        [HttpGet("coveragebycountyid", Name = $"{nameof(BuildingDataController)}_{nameof(GetCoverageByCountyIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(BuildingDataCoverageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCoverageByCountyIdAsync([BindRequired, FromQuery(Name = "countyid")] int countyId, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started for county {CountyId}", nameof(BuildingDataController), nameof(GetCoverageByCountyIdAsync), countyId);

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            BuildingDataCoverageResult? buildingDataCoverageResult;
            try
            {
                buildingDataCoverageResult = await buildingDataPostgreSQLConverter.BuildingDataCoverageResultAsync(building2DPostgreSQLConverter, countyId, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetCoverageByCountyIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (buildingDataCoverageResult is null)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(buildingDataCoverageResult);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Asynchronously retrieves the references the building data holds under more than one county, ordered by collision count descending.
        /// <para>Expected to come back empty. A reference addresses one building of one county, so anything listed here was written outside the county it belongs to and nothing removes it afterwards.</para>
        /// </summary>
        /// <param name="limit">The maximum number of references to return. Defaults to 100.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the duplicated references, or 404 when there are none.</returns>
        [HttpGet("duplicatereferences", Name = $"{nameof(BuildingDataController)}_{nameof(GetDuplicateReferencesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<Building2DReferenceDuplicate>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDuplicateReferencesAsync([Minimum(1), FromQuery(Name = "limit")] int limit = 100, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetDuplicateReferencesAsync));

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

            List<Building2DReferenceDuplicate>? building2DReferenceDuplicates;
            try
            {
                building2DReferenceDuplicates = await buildingDataPostgreSQLConverter.GetDuplicateReferencesAsync(limit, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetDuplicateReferencesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Database could not be queried");
                return StatusCode(500, "Internal server error during database query");
            }

            if (building2DReferenceDuplicates is null || building2DReferenceDuplicates.Count == 0)
            {
                return NotFound();
            }

            string? json = Core.Convert.ToSystem_String(building2DReferenceDuplicates);
            if (string.IsNullOrWhiteSpace(json))
            {
                return NotFound();
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Generates a value range distribution histogram for a specific building data column inside a county partition, applying optional dynamic filters.
        /// <para>Each row of the answer is one bucket - <c>{bucket, rangeStart, rangeEnd, count}</c>, the actual minimum and maximum of the values the bucket holds and their count. The buckets are of equal value width unless the body asks for <see cref="DiGi.PostgreSQL.Table.Enums.HistogramBucketing.EqualCount"/> (<c>HistogramBucketing: 1</c>), which gives every bucket the same number of rows and keeps a skewed column's resolution where its rows are (DiGi-Organization/DiGi.GIS.WebAPI#35).</para>
        /// <para>An invalid target or filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator. A scope that holds no non-NULL values keeps its documented 404 empty answer (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>Area scope: send <c>AdministrativeAreal2DId</c> instead of <c>CountyId</c> (the two are mutually exclusive; both answer 400) to confine the read to the buildings of an administrative area - a Subdivision through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition: one spanning several answers 400 (use <c>countbyadministrativeareal2did</c> to count across them); 204 means the resolution found no building or subdivision to read by; 404 means the area does not exist (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <param name="histogramRequestParameter">The parameter containing the target column, county identifier, desired bucket count, bucketing rule, and optional dynamic filters.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning the histogram bucket list as a JSON array.</returns>
        [HttpPost("histogramsummary", Name = $"{nameof(BuildingDataController)}_{nameof(GetHistogramSummaryAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(JsonArray), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHistogramSummaryAsync([FromBody] HistogramRequestParameter histogramRequestParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetHistogramSummaryAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                (IActionResult? actionResult_Scope, int? countyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup) = await AdministrativeAreal2DScopeAsync(histogramRequestParameter.CountyId, histogramRequestParameter.AdministrativeAreal2DId, histogramRequestParameter.FilterGroup, commandTimeout, cancellationToken);
                if (actionResult_Scope is not null)
                {
                    return actionResult_Scope;
                }

                JsonArray? histogramArray = await buildingDataPostgreSQLConverter.GetHistogramSummaryAsync(histogramRequestParameter.ColumnUniqueId, histogramRequestParameter.BucketCount, countyId, filterGroup, histogramRequestParameter.HistogramBucketing, commandTimeout, cancellationToken);

                if (histogramArray is null)
                {
                    return NotFound();
                }

                string json = histogramArray.ToJsonString();
                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetHistogramSummaryAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetHistogramSummaryAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetHistogramSummaryAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Computes multi-value statistical summaries (SplitDistinctCount, SplitValueDistribution) on a partition column.
        /// <para>An invalid target or filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>Group-by count: <c>SplitValueDistribution</c> (<c>multivalueAggregateFunction = 1</c>) on a single-value column returns one <c>{ "item", "count" }</c> object per distinct value, ordered <c>count</c> descending. Worked example (2026-10-12, partition <c>countyId 53477</c>, gmina Łomianki): <c>columnUniqueId "municipality_name"</c> answers <c>[{ "item": "Łomianki", "count": 10502 }, { "item": "Stare Babice", "count": 7976 }, …]</c>.</para>
        /// <para>The result is capped at 1000 groups plus one trailing truncation-marker row (drop the last element when the array is that long). A non-text column is cast to its text form and distributed over it, so a boolean such as <c>is_residential</c> yields <c>true</c>/<c>false</c> items rather than a failure (DiGi-Organization/DiGi.PostgreSQL#11).</para>
        /// <para><c>SplitDistinctCount</c> (<c>multivalueAggregateFunction = 0</c>) counts the distinct split sub-items instead of returning them. For the plain distinct-value list (no split, no count), use <c>uniquevalues</c>.</para>
        /// <para>Area scope: send <c>AdministrativeAreal2DId</c> instead of <c>CountyId</c> (the two are mutually exclusive; both answer 400) to confine the read to the buildings of an administrative area - a Subdivision through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition: one spanning several answers 400 (use <c>countbyadministrativeareal2did</c> to count across them); 204 means the resolution found no building or subdivision to read by; 404 means the area does not exist (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <param name="multivalueAggregateRequestParameter">The parameter containing target column, multi-value aggregate function, county identifier, and optional separator.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning the aggregate result as a JSON node.</returns>
        [HttpPost("aggregatesummary/multivalue", Name = $"{nameof(BuildingDataController)}_{nameof(GetMultivalueAggregateSummaryAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(JsonNode), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMultivalueAggregateSummaryAsync([FromBody] MultivalueAggregateRequestParameter multivalueAggregateRequestParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetMultivalueAggregateSummaryAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                (IActionResult? actionResult_Scope, int? countyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup) = await AdministrativeAreal2DScopeAsync(multivalueAggregateRequestParameter.CountyId, multivalueAggregateRequestParameter.AdministrativeAreal2DId, multivalueAggregateRequestParameter.FilterGroup, commandTimeout, cancellationToken);
                if (actionResult_Scope is not null)
                {
                    return actionResult_Scope;
                }

                JsonNode? resultNode = await buildingDataPostgreSQLConverter.GetAggregateSummaryAsync(multivalueAggregateRequestParameter.ColumnUniqueId, multivalueAggregateRequestParameter.MultivalueAggregateFunction, countyId, multivalueAggregateRequestParameter.Separator, filterGroup, maxGroups: multivalueAggregateRequestParameter.MaxGroups, commandTimeout: commandTimeout, cancellationToken: cancellationToken);

                if (resultNode is null)
                {
                    return NotFound();
                }

                string json = resultNode.ToJsonString();
                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetMultivalueAggregateSummaryAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetMultivalueAggregateSummaryAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetMultivalueAggregateSummaryAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Computes single-value statistical summaries (Avg, Sum, Min, Max, Count, DistinctCount) on a partition column.
        /// <para>An invalid target or filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator; a valid filter that matches nothing keeps its 200 empty aggregate (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>Row count: to count the matching rows, set <c>columnUniqueId "reference"</c> and <c>singlevalueAggregateFunction 4</c> (<c>Count</c>) - <c>reference</c> is the always-present column and <c>Count</c> tallies the non-null rows, so the pair is the plain "how many rows" recipe. <c>singlevalueAggregateFunction</c> values: 0 <c>Avg</c> (mean of a numeric column), 1 <c>Sum</c> (total), 2 <c>Min</c>, 3 <c>Max</c>, 4 <c>Count</c> (non-null row count), 5 <c>DistinctCount</c> (unique values).</para>
        /// <para>Count with a filter - "residential buildings over 100 m² in gmina Łomianki" (2026-10-12, partition <c>countyId 53477</c>): <c>{ "columnUniqueId": "reference", "singlevalueAggregateFunction": 4, "countyId": 53477, "filterGroup": { "logicalOperator": 0, "filterConditions": [ { "columnUniqueId": "is_residential", "filterOperator": 0, "value": true }, { "columnUniqueId": "floor_area", "filterOperator": 2, "value": 100 } ] } }</c> answers <c>6798</c> (unfiltered anchor <c>13719</c>; DiGi-Organization/DiGi.GIS.WebAPI#53).</para>
        /// <para>Area scope: send <c>AdministrativeAreal2DId</c> instead of <c>CountyId</c> (the two are mutually exclusive; both answer 400) to confine the read to the buildings of an administrative area - a Subdivision through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition: one spanning several answers 400 (use <c>countbyadministrativeareal2did</c> to count across them); 204 means the resolution found no building or subdivision to read by; 404 means the area does not exist (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <param name="singlevalueAggregateRequestParameter">The parameter containing target column, single-value aggregate function, and county identifier.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning the aggregate result as a JSON node.</returns>
        [HttpPost("aggregatesummary/singlevalue", Name = $"{nameof(BuildingDataController)}_{nameof(GetSinglevalueAggregateSummaryAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(JsonNode), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSinglevalueAggregateSummaryAsync([FromBody] SinglevalueAggregateRequestParameter singlevalueAggregateRequestParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetSinglevalueAggregateSummaryAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                (IActionResult? actionResult_Scope, int? countyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup) = await AdministrativeAreal2DScopeAsync(singlevalueAggregateRequestParameter.CountyId, singlevalueAggregateRequestParameter.AdministrativeAreal2DId, singlevalueAggregateRequestParameter.FilterGroup, commandTimeout, cancellationToken);
                if (actionResult_Scope is not null)
                {
                    return actionResult_Scope;
                }

                JsonNode? resultNode = await buildingDataPostgreSQLConverter.GetAggregateSummaryAsync(singlevalueAggregateRequestParameter.ColumnUniqueId, singlevalueAggregateRequestParameter.SinglevalueAggregateFunction, countyId, filterGroup, commandTimeout, cancellationToken);

                if (resultNode is null)
                {
                    return NotFound();
                }

                string json = resultNode.ToJsonString();
                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetSinglevalueAggregateSummaryAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetSinglevalueAggregateSummaryAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetSinglevalueAggregateSummaryAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Computes several single-value aggregate measures - different columns, different functions - over one shared partition and filter in one request, served as one database command on one connection (DiGi-Organization/DiGi.GIS.WebAPI#56).
        /// <para>The answers arrive as a JSON array in the request's measure order; a SQL <c>NULL</c> aggregate (for example <c>Avg</c> over zero rows) is preserved as JSON <c>null</c>. An empty <c>Measures</c> list, a list above the cap of <see cref="Constants.BuildingData.MaximumAggregateSummariesMeasureCount"/>, or a measure naming a column the stored table does not hold is refused with 400 - a measure failure answers the whole request, never a partial result (DiGi-Organization/DiGi.GIS.WebAPI#56).</para>
        /// <para>Out of scope by design: per-measure filters, multi-value measures (<c>SplitValueDistribution</c>, <c>SplitDistinctCount</c>) and histograms (DiGi-Organization/DiGi.GIS.WebAPI#56).</para>
        /// <para>Area scope: send <c>AdministrativeAreal2DId</c> instead of <c>CountyId</c> (the two are mutually exclusive; both answer 400) to confine the read to the buildings of an administrative area - a Subdivision through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition: one spanning several answers 400 (use <c>countbyadministrativeareal2did</c> to count across them); 204 means the resolution found no building or subdivision to read by; 404 means the area does not exist (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <param name="aggregateSummariesRequestParameter">The parameter containing the shared county identifier, the shared filter, and the ordered list of measures.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning the measures as a JSON array in request order.</returns>
        [HttpPost("aggregatesummary/batch", Name = $"{nameof(BuildingDataController)}_{nameof(GetAggregateSummariesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(JsonNode), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Measures is missing, empty, or above the cap, or a measure or filter names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAggregateSummariesAsync([FromBody] AggregateSummariesRequestParameter aggregateSummariesRequestParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetAggregateSummariesAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (aggregateSummariesRequestParameter.Measures is null || aggregateSummariesRequestParameter.Measures.Count == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Measures cannot be null or empty");
                return BadRequest();
            }

            if (aggregateSummariesRequestParameter.Measures.Count > Constants.BuildingData.MaximumAggregateSummariesMeasureCount)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Measures exceeds the cap");
                return BadRequest();
            }

            try
            {
                (IActionResult? actionResult_Scope, int? countyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup) = await AdministrativeAreal2DScopeAsync(aggregateSummariesRequestParameter.CountyId, aggregateSummariesRequestParameter.AdministrativeAreal2DId, aggregateSummariesRequestParameter.FilterGroup, commandTimeout, cancellationToken);
                if (actionResult_Scope is not null)
                {
                    return actionResult_Scope;
                }

                JsonNode? resultNode = await buildingDataPostgreSQLConverter.GetAggregateSummariesAsync(aggregateSummariesRequestParameter.Measures, countyId, filterGroup, commandTimeout, cancellationToken);

                if (resultNode is null)
                {
                    return NotFound();
                }

                string json = resultNode.ToJsonString();
                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetAggregateSummariesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetAggregateSummariesAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetAggregateSummariesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Retrieves one page of a county part's building data, following a cursor from page to page.
        /// <para><b>Reference order</b> (default, <see cref="BuildingDataByPagingParameter.PhysicalOrder"/> false): rows ordered by <c>reference</c>. The cursor is the previous page's last <c>reference</c>, and a page shorter than <c>PageSize</c> ends the part. Every row is one random heap read, which on a large part is slow: part 55417 (155 307 rows) took 368-654 s cold (DiGi.GIS.WebAPI.UI#29).</para>
        /// <para><b>Physical order</b> (<see cref="BuildingDataByPagingParameter.PhysicalOrder"/> true): rows in heap order, read sequentially. The next cursor is returned in the <c>DiGi-Next-Cursor</c> response header (<see cref="Constants.Header.NextCursor"/>), and a response without the header ends the part. The page may still carry the part's last rows. Row order is unspecified. When the physical read is unavailable - a database older than PostgreSQL 14, or a <c>Cursor</c> that is not a physical position - the same request is answered by the reference-ordered page for that <c>Cursor</c>, without the header. A client that keeps asking for physical order and follows the last row's <c>reference</c> whenever the header is absent from a full page therefore pages correctly either way.</para>
        /// <para><b>Concurrent writes.</b> Each request reads its own snapshot. In physical order a row rewritten during a walk moves: it is repeated when its new version lands ahead of the walk and missed when it lands behind. Clients dedup on <c>(reference, county_id)</c>, which covers the repeats. The misses are the price of the sequential read and matter only while the part is being rewritten.</para>
        /// </summary>
        /// <param name="buildingDataByPagingParameter">The parameter containing paging options, including column projections, county identifier, cursor, page size and order.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning the populated table. In physical order the next cursor, if any, is in the <c>DiGi-Next-Cursor</c> header.</returns>
        [HttpPost("tablebybuildingdatabypagingparameter", Name = $"{nameof(BuildingDataController)}_{nameof(GetTableByBuildingDataByPagingParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(DiGi.Core.IO.Table.Classes.Table), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTableByBuildingDataByPagingParameterAsync([FromBody] BuildingDataByPagingParameter buildingDataByPagingParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetTableByBuildingDataByPagingParameterAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (buildingDataByPagingParameter is null)
            {
                return BadRequest();
            }

            try
            {
                IEnumerable<string>? columnUniqueIds = buildingDataByPagingParameter.ColumnUniqueIds;
                if (columnUniqueIds is not null && !columnUniqueIds.Any())
                {
                    columnUniqueIds = null;
                }

                if (buildingDataByPagingParameter.PhysicalOrder)
                {
                    (Table? Table, string? Position) page = await buildingDataPostgreSQLConverter.PullByPhysicalOrderAsync(
                        buildingDataByPagingParameter.CountyId,
                        columnUniqueIds,
                        buildingDataByPagingParameter.Cursor,
                        buildingDataByPagingParameter.PageSize,
                        commandTimeout,
                        cancellationToken);

                    if (page.Table is not null && page.Position is not null)
                    {
                        string? json_Physical = Core.IO.Table.Convert.ToSystem_String<Table, Column, Row>(page.Table);
                        if (string.IsNullOrWhiteSpace(json_Physical))
                        {
                            return NotFound();
                        }

                        if (page.Position.Length != 0)
                        {
                            HttpContext.Response.Headers[Constants.Header.NextCursor] = page.Position;
                        }

                        return Content(json_Physical, "application/json");
                    }

                    // Declined (PostgreSQL older than 14, or a cursor that is not a physical position) or failed: answer
                    // the reference-ordered page for the same cursor, without the header, so the client pages on by reference.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "{Type}:{Name} physical order unavailable for county {CountyId}; answering in reference order", nameof(BuildingDataController), nameof(GetTableByBuildingDataByPagingParameterAsync), buildingDataByPagingParameter.CountyId);
                }

                Table? table = await buildingDataPostgreSQLConverter.PullAsync(
                    buildingDataByPagingParameter.CountyId,
                    columnUniqueIds,
                    buildingDataByPagingParameter.Cursor,
                    buildingDataByPagingParameter.PageSize,
                    commandTimeout,
                    cancellationToken);

                if (table is null)
                {
                    return NotFound();
                }

                string? json = Core.IO.Table.Convert.ToSystem_String<Table, Column, Row>(table);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetTableByBuildingDataByPagingParameterAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetTableByBuildingDataByPagingParameterAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary> Retrieves a building data table by building data by references parameter (column unique ids, county id and references).</summary>
        /// <param name="buildingDataByReferencesParameter">The parameter containing references for querying building data, including column unique identifiers, county identifier, and specific references.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult" /> representing the result of the operation, typically containing a <see cref="DiGi.Core.IO.Table.Classes.Table" /> if found.</returns>
        [HttpPost("tablebybuildingdatabyreferencesparameter", Name = $"{nameof(BuildingDataController)}_{nameof(GetTableByBuildingDataByReferencesParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(DiGi.Core.IO.Table.Classes.Table), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTableByBuildingDataByReferencesParameterAsync([FromBody] BuildingDataByReferencesParameter buildingDataByReferencesParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetTableByBuildingDataByReferencesParameterAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                IEnumerable<string>? references = buildingDataByReferencesParameter.References;
                if (references is null || !references.Any())
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "At least one reference has to be provided");
                    return BadRequest();
                }

                if (references.Count() > referenceCount_Maximum)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "At most {Maximum} references can be asked for in one request", referenceCount_Maximum);
                    return BadRequest();
                }


                IEnumerable<string>? columnUniqueIds = buildingDataByReferencesParameter.ColumnUniqueIds;
                if (columnUniqueIds is not null && !columnUniqueIds.Any())
                {
                    columnUniqueIds = null;
                    Serilog.Modify.Log("No column Ids have been provided");
                }

                Table? table = await buildingDataPostgreSQLConverter.PullAsync(buildingDataByReferencesParameter.References, buildingDataByReferencesParameter.CountyId, columnUniqueIds, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (table is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Table could not be extracted from Converter");
                    return NotFound();
                }

                string? json = Core.IO.Table.Convert.ToSystem_String<Table, Column, Row>(table);
                if (string.IsNullOrWhiteSpace(json))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Table could not be converted to json");
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetTableByBuildingDataByReferencesParameterAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetTableByBuildingDataByReferencesParameterAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary> Retrieves a building data table by building data by subdivision ids parameter (column unique ids, subdivision ids). </summary>
        /// <param name="buildingDataBySubdivisionIdsParameter">The parameter containing the subdivision IDs and optional column unique identifiers for querying building data.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("tablebybuildingdatabysubdivisionidsparameter", Name = $"{nameof(BuildingDataController)}_{nameof(GetTableByBuildingDataBySubdivisionIdsParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(DiGi.Core.IO.Table.Classes.Table), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTableByBuildingDataBySubdivisionIdsParameterAsync([FromBody] BuildingDataBySubdivisionIdsParameter buildingDataBySubdivisionIdsParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetTableByBuildingDataBySubdivisionIdsParameterAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                IEnumerable<string>? columnUniqueIds = buildingDataBySubdivisionIdsParameter.ColumnUniqueIds;
                if (columnUniqueIds is not null && !columnUniqueIds.Any())
                {
                    columnUniqueIds = null;
                }

                Table? table = await buildingDataPostgreSQLConverter.PullAsync(IO.Constants.Column.SubdivisionId.UniqueId()!, buildingDataBySubdivisionIdsParameter.SubdivisionIds, columnUniqueIds, commandTimeout, cancellationToken);
                if (table is null)
                {
                    return NotFound();
                }

                string? json = Core.IO.Table.Convert.ToSystem_String<Table, Column, Row>(table);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetTableByBuildingDataBySubdivisionIdsParameterAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetTableByBuildingDataBySubdivisionIdsParameterAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Retrieves a building data table filtered by the specified dynamic hierarchical filters.
        /// <para>The table is partitioned by <c>county_id</c>. Supply <see cref="BuildingDataByFilterGroupParameter.CountyId"/> to scope the read to one county partition; omitting it (and not filtering <c>county_id</c> in the filter group) makes the read span every partition of the national table, which is the cost of an unscoped cross-partition read. A caller <c>county_id</c> condition in the filter group is left as-is and the <c>CountyId</c> condition is ANDed with it, so a conflicting pair answers empty.</para>
        /// <para>An invalid filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>Per-operator value shapes: each <c>FilterCondition</c>'s <c>Value</c> takes a scalar for the comparison operators, a JSON array for <c>In</c>/<c>NotIn</c>, and a case-insensitive substring for <c>Contains</c>; an unconvertible value is refused with 400 (see <c>FilterCondition.Value</c> for the full contract; DiGi-Organization/DiGi.PostgreSQL#13).</para>
        /// <para>Area scope: send <c>AdministrativeAreal2DId</c> instead of <c>CountyId</c> (the two are mutually exclusive; both answer 400) to confine the read to the buildings of an administrative area - a Subdivision through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition: one spanning several answers 400 (use <c>countbyadministrativeareal2did</c> to count across them); 204 means the resolution found no building or subdivision to read by; 404 means the area does not exist (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// <para>Top-N ordering: <c>SortConditions</c> orders the rows in list order (<c>NULLS LAST</c> in both directions) and <c>Limit</c> caps the count (1 to 1000, refused outside the range, never clamped) - one call answers "the tallest building in an area" instead of a maximum lookup followed by a fragile equality re-filter. Equal sort values are tie-broken deterministically by <c>reference</c>, so the same call answers the same rows every time; a sort column the stored table does not hold is refused with 400 exactly like a filter column (DiGi-Organization/DiGi.GIS.WebAPI#84). Recipe - the tallest building of an administrative area: body <c>{"administrativeAreal2DId": 55540, "sortConditions": [{"columnUniqueId": "storeys", "descending": true}], "limit": 1, "columnUniqueIds": ["reference", "storeys", "total_area"]}</c>. Ordering does not change the cost class: the sort runs on the scope the county or area gives, while an unscoped top-N still reads every partition - the cross-partition warning above applies to the scan even though the answer is one row.</para>
        /// </summary>
        /// <param name="buildingDataByFilterGroupParameter">The parameter containing the dynamic filter group, the optional county partition, the optional column unique identifiers, the optional sort conditions and the optional row limit.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation, returning the populated filtered table.</returns>
        [HttpPost("tablebyfiltergroup", Name = $"{nameof(BuildingDataController)}_{nameof(GetTableByFilterGroupAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(DiGi.Core.IO.Table.Classes.Table), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold (as a filter or sort column), carries a filter value the column's type cannot convert, or a limit outside 1-1000. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTableByFilterGroupAsync([FromBody] BuildingDataByFilterGroupParameter buildingDataByFilterGroupParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetTableByFilterGroupAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            // The limit is refused, never clamped, and refused before any connection is opened
            // (DiGi-Organization/DiGi.GIS.WebAPI#84).
            if (buildingDataByFilterGroupParameter.Limit is int int_Limit && (int_Limit < Constants.BuildingData.MinimumLimit || int_Limit > Constants.BuildingData.MaximumLimit))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Limit {Limit} is outside the accepted range {Minimum}-{Maximum}", int_Limit, Constants.BuildingData.MinimumLimit, Constants.BuildingData.MaximumLimit);
                return Problem(detail: $"Limit must be between {Constants.BuildingData.MinimumLimit} and {Constants.BuildingData.MaximumLimit}; {int_Limit} is refused, not clamped.", statusCode: 400);
            }

            try
            {
                List<string>? strings_ColumnUniqueIds = buildingDataByFilterGroupParameter.ColumnUniqueIds;
                if (strings_ColumnUniqueIds is not null && strings_ColumnUniqueIds.Count == 0)
                {
                    strings_ColumnUniqueIds = null;
                }

                // One connection for the column resolution and the read. The converter folds the county partition into the
                // filter rather than writing it into the statement, so county_id is resolved against the stored column
                // list like any filter column. A caller county_id condition is left as-is and this one is ANDed with it,
                // so a conflicting pair answers empty (DiGi-Organization/DiGi.GIS.WebAPI#55).
                (IActionResult? actionResult_Scope, int? countyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup) = await AdministrativeAreal2DScopeAsync(buildingDataByFilterGroupParameter.CountyId, buildingDataByFilterGroupParameter.AdministrativeAreal2DId, buildingDataByFilterGroupParameter.FilterGroup, commandTimeout, cancellationToken);
                if (actionResult_Scope is not null)
                {
                    return actionResult_Scope;
                }

                Table? table_Result = await buildingDataPostgreSQLConverter.PullByFilterGroupAsync(filterGroup, countyId, strings_ColumnUniqueIds, orderBy: buildingDataByFilterGroupParameter.SortConditions, limit: buildingDataByFilterGroupParameter.Limit, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (table_Result is null)
                {
                    return NotFound();
                }

                string? string_Json = Core.IO.Table.Convert.ToSystem_String<Table, Column, Row>(table_Result);
                if (string.IsNullOrWhiteSpace(string_Json))
                {
                    return NotFound();
                }

                return Content(string_Json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetTableByFilterGroupAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetTableByFilterGroupAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetTableByFilterGroupAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously counts the building data rows of a county partition, or of the whole table, matching an optional dynamic hierarchical filter group.
        /// <para>The table is partitioned by <c>county_id</c>. Supply <see cref="BuildingDataCountByFilterGroupParameter.CountyId"/> to scope the read to one county partition; omitting it (and not filtering <c>county_id</c> in the filter group) makes the read span every partition of the national table, which is the cost of an unscoped cross-partition read. Measured 2026-10-14 on the estate: with a <c>CountyId</c> the count returns in 0.1 s; without one it exceeds 300 s (client timeout). The sibling <c>GET countbycountyid</c> refuses an unscoped request with 400 (<c>[BindRequired]</c>); this endpoint accepts it. Figures are measurements, not guarantees.</para>
        /// <para>The answer is a bare JSON integer: the <c>COUNT(*)</c> of the scope, so it does not depend on any column being non-null and it never needs the magic <c>reference</c> column or an aggregate function. A <c>null</c> or empty filter group counts the whole partition scope; a filter that matches nothing answers <c>0</c> with a <c>200</c>, not an error status.</para>
        /// <para>An invalid filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>De-duplication across county parts is a separate concern (<c>referenceuniquenesssummary</c>) and is not applied here.</para>
        /// </summary>
        /// <param name="parameter">The parameter containing the optional partition (county) and the optional dynamic hierarchical filters.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the count as a bare JSON integer, 404 NotFound when no connection could be built or a filter column could not be resolved, or 400/503/500 on the guarded and failure paths.</returns>
        [HttpPost("countbyfiltergroup", Name = $"{nameof(BuildingDataController)}_{nameof(GetCountByFilterGroupAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountByFilterGroupAsync([FromBody] BuildingDataCountByFilterGroupParameter parameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetCountByFilterGroupAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            long? count;
            try
            {
                count = await buildingDataPostgreSQLConverter.GetCountByFilterGroupAsync(parameter.FilterGroup, parameter.CountyId, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetCountByFilterGroupAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetCountByFilterGroupAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetCountByFilterGroupAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            if (count is null)
            {
                Serilog.Modify.Log("No connection, or a filter column could not be resolved; answering NotFound");
                return NotFound();
            }

            return Content(count.Value.ToString(CultureInfo.CurrentCulture), "application/json");
        }

        /// <summary>
        /// Asynchronously counts the building data rows of an administrative area, optionally narrowed by a dynamic hierarchical filter group, in one call.
        /// <para>An invalid filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>Resolution (DiGi-Organization/DiGi.GIS.WebAPI#83): (1) the area reference is read by id, and a missing one answers 404. (2) A <b>Subdivision</b> is resolved through its <b>polygon</b>: every building whose bounding-box centre lies strictly inside it (a centre on the boundary belongs to neither side), whatever subdivision it is filed under - the same set <c>building2d/building2Dreferencesbyadministrativeareal2Did</c> answers. The buildings are grouped by county partition and each partition is counted with <c>reference IN (those buildings)</c> plus the caller's filters. A subdivision that only partly overlaps the polygon contributes exactly its buildings inside it. A Subdivision whose polygon holds no building centre answers 204. (3) A <b>County</b> is counted as its own partition. (4) A <b>Municipality</b>, <b>Voivodeship</b> or <b>Country</b> is resolved to its Subdivision children, grouped by the county partition they sit in, and each partition is counted with <c>subdivision_id IN (that county's subdivisions)</c> plus the caller's filters; one that resolves to no Subdivision answers 204. (5) The per-partition counts are <b>summed</b>, strictly one after another, never in parallel, because concurrent requests exhaust the Npgsql pool and turn healthy endpoints into 500s.</para>
        /// <para>Duration (measured 2026-10-14 on the estate; figures are measurements, not guarantees): a single county area returns in under a second; a voivodeship (typically 10–20 counties) takes roughly 1–3 minutes; a country-level area spans 400+ counties and exceeds normal client timeouts. <c>commandtimeout</c> applies per command, not per request: each sequential per-county count gets its own timeout, so total wall-clock can approach <c>N × commandtimeout</c> in the worst case.</para>
        /// <para>What the count is not. The count is of <c>building_data</c> rows, so a building inside the area with no <c>building_data</c> row is not counted. For a Municipality, Voivodeship or Country (step 4), rows with <c>subdivision_id IS NULL</c> are excluded and a building filed in a county partition other than the one its subdivision belongs to is not reached, so the count is not the count of every building inside the area's polygon; a Subdivision (step 2) and a County (step 3) have neither gap.</para>
        /// <para>An unfiltered body is valid (<c>FilterGroup</c> omitted counts every building the area resolves to). At most <see cref="Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount"/> distinct counties are iterated and a Subdivision may resolve to at most <see cref="Constants.BuildingData.MaximumAdministrativeAreal2DReferenceCount"/> buildings; an area beyond either answers 400 naming the limit and nothing is counted. 204 means <b>the resolution found no subdivision or building to count by</b> - for an area above the Subdivision level it is not evidence that the area holds no buildings. 404 means the area does not exist, or no connection could be built, or a filter column could not be resolved. A filter value that cannot be converted to its column's type answers 400.</para>
        /// <para>A client that sends the same <c>In</c> filter shape as a JSON array is subject to DiGi-Organization/DiGi.PostgreSQL#10; this endpoint builds its own subdivision list server-side and does not depend on that fix.</para>
        /// </summary>
        /// <param name="parameter">The parameter containing the administrative area identifier and the optional dynamic hierarchical filters.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of each command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> carrying the count as a bare JSON integer, 204 NoContent when no subdivision or building was found, 404 NotFound, or 400/503/500 on the guarded and failure paths.</returns>
        [HttpPost("countbyadministrativeareal2did", Name = $"{nameof(BuildingDataController)}_{nameof(GetCountByAdministrativeAreal2DIdAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetCountByAdministrativeAreal2DIdAsync([FromBody] BuildingDataCountByAdministrativeAreal2DIdParameter parameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetCountByAdministrativeAreal2DIdAsync));

            if (parameter is null || parameter.AdministrativeAreal2DId is null || parameter.AdministrativeAreal2DId.Value <= 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "AdministrativeAreal2DId is missing or not positive");
                return BadRequest();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            (Enums.AdministrativeAreal2DCountStatus Status, long Count) result;
            try
            {
                result = await Query.CountByAdministrativeAreal2DIdAsync(
                    parameter.AdministrativeAreal2DId.Value,
                    parameter.FilterGroup,
                    (id, token) => administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByIdAsync(id, commandTimeout, token),
                    (id, token) => administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(PostgreSQL.Enums.AdministrativeArealType.Subdivision, id, commandTimeout: commandTimeout, cancellationToken: token),
                    (id, token) => building2DPostgreSQLConverter.GetBuilding2DReferencesBySubdivisionIdAsync(id, commandTimeout, token),
                    (filterGroup, countyId, token) => buildingDataPostgreSQLConverter.GetCountByFilterGroupAsync(filterGroup, countyId, commandTimeout, token),
                    Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount,
                    Constants.BuildingData.MaximumAdministrativeAreal2DReferenceCount,
                    cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetCountByAdministrativeAreal2DIdAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetCountByAdministrativeAreal2DIdAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetCountByAdministrativeAreal2DIdAsync));
                return StatusCode(500, "Internal server error during database query");
            }

            switch (result.Status)
            {
                case Enums.AdministrativeAreal2DCountStatus.Counted:
                    return Content(result.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), "application/json");

                case Enums.AdministrativeAreal2DCountStatus.NoSubdivisions:
                case Enums.AdministrativeAreal2DCountStatus.NoBuildings:
                    Serilog.Modify.Log("Resolution found no subdivision or building to count by; answering NoContent");
                    return NoContent();

                case Enums.AdministrativeAreal2DCountStatus.TooManyCounties:
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Area spans more counties than the limit of {Maximum}", Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount);
                    return BadRequest($"The administrative area spans more than {Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount} counties; narrow the area.");

                case Enums.AdministrativeAreal2DCountStatus.TooManyReferences:
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Area holds more buildings than the limit of {Maximum}", Constants.BuildingData.MaximumAdministrativeAreal2DReferenceCount);
                    return BadRequest($"The administrative area holds more than {Constants.BuildingData.MaximumAdministrativeAreal2DReferenceCount} buildings; narrow the area.");

                default:
                    Serilog.Modify.Log("Area not found, no connection, or a filter column could not be resolved; answering NotFound");
                    return NotFound();
            }
        }

        /// <summary>
        /// Retrieves the building data table row of one building, optionally projected to the columns the caller names.
        /// <para><paramref name="columns"/> takes stored column unique ids as repeated query keys (<c>?columns=storeys&amp;columns=floor_area</c>) or as one comma-separated value (<c>?columns=storeys,floor_area</c>), and is checked against the stored column list before the table is read: an id the stored table does not hold answers 400 naming it, an entry naming no id answers 400, and a projection that cannot be checked because the stored column list could not be read answers 500 - never a silently narrowed read. Omitting it keeps every stored column, which is what this endpoint has always returned.</para>
        /// <para>The answer always carries <c>reference</c>, and <c>county_id</c> as well whenever <paramref name="countyId"/> is given: the read is keyed by <c>(county_id, reference)</c>, so <paramref name="countyId"/> is what confines it to one partition. Without it the reference is read across every partition, but a projection that contains <c>county_id</c> then answers nulls for every data column (DiGi-Organization/DiGi.GIS.PostgreSQL#140) - pass <paramref name="countyId"/> whenever you project <c>county_id</c>, which is what <c>byaddress</c> and the address search (<c>AddressPointMatch.CountyId</c>) make free.</para>
        /// </summary>
        /// <param name="reference">The <c>building_2d</c> reference of the building. Required.</param>
        /// <param name="countyId">The county part the building belongs to, which is the partition the row is read from; null reads the reference across partitions (DiGi-Organization/DiGi.GIS.PostgreSQL#140).</param>
        /// <param name="columns">The projected columns, as repeated query keys or as one comma-separated value; null for every stored column.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation: 200 with the table of the building, 400 for a missing reference, a negative timeout, an unknown column or an entry naming no column, 404 when the table could not be read, or 503/500 on the failure paths.</returns>
        [HttpGet("tablebyreference", Name = $"{nameof(BuildingDataController)}_{nameof(GetTableByReferenceAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(DiGi.Core.IO.Table.Classes.Table), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The reference is missing, the timeout is negative, or the projection names a column the stored table does not hold (the body names it).")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTableByReferenceAsync([BindRequired, FromQuery(Name = "reference")] string reference, [FromQuery(Name = "countyid")] int? countyId = null, [FromQuery(Name = "columns")] List<string>? columns = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetTableByReferenceAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                Serilog.Modify.Log("Reference provided: {Reference}", reference ?? string.Empty);
                Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString(CultureInfo.CurrentCulture) ?? string.Empty);

                if (string.IsNullOrWhiteSpace(reference))
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference cannot be null");
                    return BadRequest();
                }

                (IActionResult? actionResult_Columns, List<string>? columnUniqueIds) = await ColumnUniqueIdsAsync(columns, null, commandTimeout, cancellationToken);
                if (actionResult_Columns is not null)
                {
                    return actionResult_Columns;
                }

                BuildingDataByReferencesParameter buildingDataByReferencesParameter = new()
                {
                    References = [reference],
                    CountyId = countyId,
                    ColumnUniqueIds = columnUniqueIds
                };

                return await GetTableByBuildingDataByReferencesParameterAsync(buildingDataByReferencesParameter, commandTimeout, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetTableByReferenceAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetTableByReferenceAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Retrieves unique values for a specified column unique identifier and an optional county identifier.
        /// <para>An invalid column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator; a scope that holds nothing keeps its documented 404 empty answer (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// </summary>
        /// <param name="columnUniqueId">The unique identifier of the column from which to retrieve unique values.</param>
        /// <param name="countyId">The optional integer identifier of the county used to filter the results.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 90 seconds; a national (unfiltered) unique-values scan runs well past that and should pass <c>commandtimeout=600</c> explicitly.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("uniquevalues", Name = $"{nameof(BuildingDataController)}_{nameof(GetUniqueValuesAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(List<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUniqueValuesAsync([BindRequired, FromQuery(Name = "columnuniqueid")] string columnUniqueId, [FromQuery(Name = "countyid")] int? countyId = null, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 90, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetUniqueValuesAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                Serilog.Modify.Log("ColumnUniqueId provided: {ColumnUniqueId}", columnUniqueId ?? string.Empty);
                Serilog.Modify.Log("CountyId provided: {CountyId}", countyId?.ToString(CultureInfo.CurrentCulture) ?? string.Empty);
                IEnumerable<object?>? values;
                if (countyId is null)
                {
                    values = await buildingDataPostgreSQLConverter.GetUniqueValuesAsync<object?>(columnUniqueId, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                }
                else
                {
                    values = await buildingDataPostgreSQLConverter.GetUniqueValuesAsync<object?>(columnUniqueId, countyId.Value, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                }

                if (values is null || !values.Any())
                {
                    return NotFound();
                }

                JsonArray jsonArray = [];
                foreach (object? value in values)
                {
                    jsonArray.Add(value);
                }

                string? json = jsonArray.ToJsonString();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetUniqueValuesAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetUniqueValuesAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetUniqueValuesAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Retrieves unique values for a given <see cref="UniqueValuesByColumnUniqueIdParameter" /> (column unique id and optionally county id), applying optional dynamic filters.
        /// <para>An invalid column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator; a scope that holds nothing keeps its documented 404 empty answer (DiGi-Organization/DiGi.GIS.WebAPI#54).</para>
        /// <para>Area scope: send <c>AdministrativeAreal2DId</c> instead of <c>CountyId</c> (the two are mutually exclusive; both answer 400) to confine the read to the buildings of an administrative area - a Subdivision through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition: one spanning several answers 400 (use <c>countbyadministrativeareal2did</c> to count across them); 204 means the resolution found no building or subdivision to read by; 404 means the area does not exist (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <param name="uniqueValuesByColumnUniqueIdParameter">The parameter containing the column unique identifier, optional county identifier, and optional dynamic filters.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 90 seconds; a national (unfiltered) unique-values scan runs well past that and should pass <c>commandtimeout=600</c> explicitly.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult" /> representing the result of the operation, typically a list of unique values or a not found status.</returns>
        [HttpPost("uniquevaluesbycolumnuniqueidparameter", Name = $"{nameof(BuildingDataController)}_{nameof(GetUniqueValuesByColumnUniqueIdParameterAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request names a column the stored table does not hold, or a filter value the column's type cannot convert. The body names the offending column and, where known, the filter operator.")]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUniqueValuesByColumnUniqueIdParameterAsync([FromBody] UniqueValuesByColumnUniqueIdParameter uniqueValuesByColumnUniqueIdParameter, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 90, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(GetUniqueValuesByColumnUniqueIdParameterAsync));

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            try
            {
                (IActionResult? actionResult_Scope, int? countyId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup) = await AdministrativeAreal2DScopeAsync(uniqueValuesByColumnUniqueIdParameter.CountyId, uniqueValuesByColumnUniqueIdParameter.AdministrativeAreal2DId, uniqueValuesByColumnUniqueIdParameter.FilterGroup, commandTimeout, cancellationToken);
                if (actionResult_Scope is not null)
                {
                    return actionResult_Scope;
                }

                IEnumerable<object?>? values;
                if (countyId is null)
                {
                    values = await buildingDataPostgreSQLConverter.GetUniqueValuesAsync<object?>(
                        uniqueValuesByColumnUniqueIdParameter.ColumnUniqueId,
                        filterGroup,
                        commandTimeout,
                        cancellationToken);
                }
                else
                {
                    values = await buildingDataPostgreSQLConverter.GetUniqueValuesAsync<object?>(
                        uniqueValuesByColumnUniqueIdParameter.ColumnUniqueId,
                        countyId.Value,
                        filterGroup,
                        commandTimeout,
                        cancellationToken);
                }

                if (values is null || !values.Any())
                {
                    return NotFound();
                }

                JsonArray jsonArray = [];
                foreach (object? value in values)
                {
                    jsonArray.Add(value);
                }

                string? json = jsonArray.ToJsonString();
                if (string.IsNullOrWhiteSpace(json))
                {
                    return NotFound();
                }

                return Content(json, "application/json");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(GetUniqueValuesByColumnUniqueIdParameterAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (DiGi.PostgreSQL.Table.Classes.InvalidFilterException exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} refused an invalid filter", nameof(BuildingDataController), nameof(GetUniqueValuesByColumnUniqueIdParameterAsync));
                return Problem(detail: exception.Message, statusCode: 400);
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed", nameof(BuildingDataController), nameof(GetUniqueValuesByColumnUniqueIdParameterAsync));
                return StatusCode(500, "Internal server error during database query");
            }
        }

        /// <summary>
        /// Asynchronously updates building data for the specified county identifiers.
        /// <para>The identifiers are the parts of one county in play, and each row is filed under the part already holding the <c>building_2d</c> row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same <c>(county_id, reference)</c> pair.</para>
        /// <para>A row no named part holds is widened to every part of the county its parts name, and only a row no part of the county holds is left unwritten - nothing states where it belongs, and storing it under a guessed part is the state this replaced.</para>
        /// </summary>
        /// <param name="jsonObject">The JSON object containing the table structure and data to be updated.</param>
        /// <param name="countyIds">The identifiers of the county rows the building data belongs to. Normally every polygon part of one county.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the database command. A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600.</param>
        /// <param name="key">The secret access key supplied in the request header.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation with an <see cref="IActionResult"/> containing an <see cref="UpdateItemsResult"/>.</returns>
        [HttpPost("updateitemsbycountyids", Name = $"{nameof(BuildingDataController)}_{nameof(UpdateItemsByCountyIdsAsync)}")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [ProducesResponseType(typeof(UpdateItemsResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateItemsByCountyIdsAsync([FromBody] JsonObject? jsonObject, [BindRequired, FromQuery(Name = "countyids")] int[]? countyIds, [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600, [FromHeader(Name = "key")] string? key = null, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(BuildingDataController), nameof(UpdateItemsByCountyIdsAsync));
            Serilog.Modify.Log("CountyIds provided: {CountyIds}", countyIds is null ? string.Empty : string.Join(", ", countyIds));

            if (!GISWebAPIConfigurationFileWatcher.IsAuthorized(key))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "BuildingData update not authorized");
                return Unauthorized();
            }

            if (!GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "BuildingData update not allowed");
                return Unauthorized();
            }

            if (commandTimeout < 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CommandTimeout cannot be negative");
                return BadRequest();
            }

            if (countyIds is null || countyIds.Length == 0)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyIds cannot be null or empty");
                return BadRequest();
            }

            if (jsonObject is null)
            {
                Serilog.Modify.Log("No BuildingData to update");
                return NoContent();
            }

            Table? table = Create.Table(jsonObject);
            if (table is null || table.RowCount == 0)
            {
                Serilog.Modify.Log("No BuildingData rows to update");
                return NoContent();
            }

            try
            {
                Column? column_Reference = table.Columns.FirstOrDefault(c => string.Equals(c.UniqueId(), "reference", StringComparison.OrdinalIgnoreCase));
                if (column_Reference is null)
                {
                    table.TryGetColumn(IO.Constants.Column.Reference.Name, out column_Reference, caseSensitive: false);
                }

                if (column_Reference is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "Reference column could not be obtained");
                    return BadRequest();
                }

                Column? column_CountyId = table.Columns.FirstOrDefault(c => string.Equals(c.UniqueId(), "county_id", StringComparison.OrdinalIgnoreCase));
                if (column_CountyId is null)
                {
                    table.TryGetColumn(IO.Constants.Column.CountyId.Name, out column_CountyId, caseSensitive: false);
                }

                if (column_CountyId is null)
                {
                    column_CountyId = table.AddColumn(IO.Constants.Column.CountyId);
                }

                if (column_CountyId is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "CountyId column could not be obtained");
                    return BadRequest();
                }

                List<int> countyIds_Candidate = [.. new HashSet<int>(countyIds).OrderBy(x => x)];

                // A row carries no geometry, so the 2D building its reference names is the only thing that can say
                // which part it belongs to. Every row is resolved through building_2d regardless of how many ids the
                // caller sent - naming one id is not evidence the code has one part - and a row no named part holds
                // is widened to every part of the named county before it is rejected, never filed under a guessed part.
                List<string> references_ToResolve = [];
                foreach (Row row in table.Rows)
                {
                    if (row.TryGetValue(column_Reference.Index, out string? reference) && !string.IsNullOrWhiteSpace(reference))
                    {
                        references_ToResolve.Add(reference);
                    }
                }

                Dictionary<string, int>? countyIds_ByReference = await PostgreSQL.Query.CountyIdsByReferencesWithSiblingFallbackAsync(building2DPostgreSQLConverter, administrativeAreal2DPostgreSQLConverter, references_ToResolve, countyIds_Candidate, cancellationToken: cancellationToken);

                if (countyIds_ByReference is null)
                {
                    // The lookup could not run at all, which is a different failure from running and
                    // resolving nothing: classifying every row as CountyUnresolved below would report a
                    // broken connection as a batch-wide data problem.
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County parts could not be resolved: the building_2d lookup could not run (check the Main database configuration)");
                    return StatusCode(500, "County parts could not be resolved: the building_2d lookup could not run.");
                }

                List<UpdateItemsResult.Rejection> rejections = [];
                Table table_Resolved = new(table.Columns);

                foreach (Row row in table.Rows)
                {
                    if (!row.TryGetValue(column_Reference.Index, out string? reference) || string.IsNullOrWhiteSpace(reference))
                    {
                        rejections.Add(new UpdateItemsResult.Rejection
                        {
                            Reference = null,
                            Reason = PostgreSQL.Enums.UpdateRejectionReason.Undefined
                        });
                        continue;
                    }

                    if (!countyIds_ByReference.TryGetValue(reference, out int countyId))
                    {
                        rejections.Add(new UpdateItemsResult.Rejection
                        {
                            Reference = reference,
                            Reason = PostgreSQL.Enums.UpdateRejectionReason.CountyUnresolved
                        });
                        continue;
                    }

                    IO.Modify.SetValue(row, column_CountyId, countyId);
                    if (row[column_CountyId.Index] is null)
                    {
                        row[column_CountyId.Index] = countyId;
                    }

                    table_Resolved.AddRow(row);
                }

                if (table_Resolved.RowCount == 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "All {Sent} BuildingData rows were rejected before the database", table.RowCount);
                    return StatusCode(500, $"All {table.RowCount} BuildingData rows were rejected before the database; none could be filed under a county.");
                }

                Serilog.Modify.Log("Pushing BuildingData to database started. Rows to push: {Count}", table_Resolved.RowCount);

                bool pushed = await buildingDataPostgreSQLConverter.PushAsync(table_Resolved, batchSize: 1000, commandTimeout: commandTimeout, cancellationToken: cancellationToken);
                if (!pushed)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "Updating to database could not be completed");
                    return StatusCode(500, "Database update failed.");
                }

                UpdateItemsResult updateItemsResult = new()
                {
                    Sent = table.RowCount,
                    Updated = table_Resolved.RowCount,
                    Rejected = rejections
                };

                if (updateItemsResult.Rejected.Count != 0)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning, "BuildingData rows rejected before the database: {Count}/{Total}. References: {References}", updateItemsResult.Rejected.Count, updateItemsResult.Sent, updateItemsResult.Rejected.RejectionSample());
                }

                Serilog.Modify.Log("Updating to database ended. Updated BuildingData rows: {After}/{Before}, rejected: {Rejected}", updateItemsResult.Updated, updateItemsResult.Sent, updateItemsResult.Rejected.Count);
                return Ok(updateItemsResult);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (NpgsqlException exception) when (exception.IsTransient)
            {
                Serilog.Modify.Log(exception, "{Type}:{Name} failed (transient database failure)", nameof(BuildingDataController), nameof(UpdateItemsByCountyIdsAsync));
                HttpContext.Response.Headers["Retry-After"] = "30";
                return StatusCode(503, "Database temporarily unavailable; retry shortly");
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "Unhandled error during BuildingDataController.UpdateItemsByCountyIdsAsync");
                return StatusCode(500, exception.Message);
            }
        }
    }
}
