using DiGi.Analytical.Building.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads one page of the building models filed under a county part through <c>gis/buildingmodel/itemsbycountyid</c>, with the cursor of the next page.
        /// <para>One page, not the whole part: a part holds tens of thousands of models with full geometry, so the caller walks it - start at <paramref name="idAfter"/> 0, pass each answer's <c>IdAfter</c> back, stop when it is null - and keeps only what it needs. The cursor travels in the <c>DiGi-Next-Cursor</c> response header (<see cref="Constants.Header.NextCursor"/>), which <see cref="DiGi.WebAPI.Query.GetAsync{T}(HttpClient?, string?, PostOptions?)"/> does not expose, so the request is sent here directly. These are the receivers of a solar solve; their neighbours cross county borders and come from <c>itemsbycircle</c>.</para>
        /// <para>Without <paramref name="postOptions"/> one request may take <paramref name="commandTimeout"/> seconds (no bound when it is 0), so the client never gives up before the server does (<c>Coding - WebAPI Contracts.md</c> §3). A failed page answers null - never a short page - and is not retried here.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="countyId">The identifier of the county part (<c>administrative_areal_2d.id</c>).</param>
        /// <param name="idAfter">The exclusive lower bound of the row ids: 0 for the first page, then the previous page's <c>IdAfter</c>.</param>
        /// <param name="limit">The largest number of models in the page, from 1 to <see cref="Constants.BuildingModel.MaximumItemsByCountyIdLimit"/>.</param>
        /// <param name="commandTimeout">The server's timeout in seconds for the read; 0 disables it.</param>
        /// <param name="postOptions">The options of the request; null to bound it by <paramref name="commandTimeout"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the models of the page and the row id to pass as the next <paramref name="idAfter"/> (null after the last page), or null when an argument is out of range or the request failed.</returns>
        public static async Task<(List<BuildingModel> BuildingModels, long? IdAfter)?> BuildingModelsByCountyIdAsync(this GISWebAPIManager? gISWebAPIManager, int countyId, long idAfter = 0, int limit = Constants.BuildingModel.DefaultItemsByCountyIdLimit, int commandTimeout = 600, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || countyId < 1 || idAfter < 0 || limit < 1 || limit > Constants.BuildingModel.MaximumItemsByCountyIdLimit || commandTimeout < 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<BuildingModelController>(nameof(BuildingModelController.GetItemsByCountyIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(BuildingModelController.GetItemsByCountyIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyid", countyId);
            urlBuilder.AddParameter("idafter", idAfter);
            urlBuilder.AddParameter("limit", limit);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            TimeSpan delay = postOptions?.Delay ?? (commandTimeout == 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(commandTimeout));

            try
            {
                using CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cancellationTokenSource.CancelAfter(delay);

                using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(urlBuilder.ToString(), cancellationTokenSource.Token);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The building models of county part {CountyId} after row {IdAfter} could not be read: {StatusCode}", countyId, idAfter, (int)httpResponseMessage.StatusCode);
                    return null;
                }

                string json = await httpResponseMessage.Content.ReadAsStringAsync(cancellationTokenSource.Token);

                List<BuildingModel>? buildingModels = Core.Convert.ToDiGi<BuildingModel>(json);
                if (buildingModels is null)
                {
                    if (json.Trim() != "[]")
                    {
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The building models of county part {CountyId} after row {IdAfter} could not be read from the answer", countyId, idAfter);
                        return null;
                    }

                    buildingModels = [];
                }

                long? idAfter_Next = null;
                if (httpResponseMessage.Headers.TryGetValues(Constants.Header.NextCursor, out IEnumerable<string>? values))
                {
                    if (!long.TryParse(values.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long idAfter_Parsed) || idAfter_Parsed <= idAfter)
                    {
                        // A cursor that does not advance would walk the same page forever.
                        Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "County part {CountyId}: the next cursor {Cursor} does not follow row {IdAfter}", countyId, values.FirstOrDefault() ?? "-", idAfter);
                        return null;
                    }

                    idAfter_Next = idAfter_Parsed;
                }

                return (buildingModels, idAfter_Next);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The building models of county part {CountyId} after row {IdAfter} could not be read", countyId, idAfter);
                return null;
            }
        }
    }
}
