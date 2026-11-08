using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously lists the prediction runs stored under the given county parts through <c>gis/yearbuiltdata/predictedyearbuiltruns</c>: one entry per part, stamp and model identifier, with the number of objects carrying it.
        /// <para>The stamp is <see cref="DateTime.Ticks"/>, the value <see cref="Modify.RemovePredictedYearBuiltsAsync"/> takes. An empty list means no prediction is stored; null means the request failed.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyIds">The county parts to list.</param>
        /// <param name="commandTimeout">The timeout in seconds for the query on the server. A value of 0 disables it.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the runs, or null when the request failed.</returns>
        public static async Task<List<PredictedYearBuiltRunResult>?> PredictedYearBuiltRunsAsync(this GISWebAPIManager? gISWebAPIManager, IEnumerable<int>? countyIds, int commandTimeout = 600, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            int[] countyIds_Array = countyIds is null ? [] : [.. countyIds.Distinct()];
            if (gISWebAPIManager is null || countyIds_Array.Length == 0 || commandTimeout < 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.GetPredictedYearBuiltRunsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.GetPredictedYearBuiltRunsAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyids", countyIds_Array);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            PostOptions postOptions_Resolved = postOptions is null ? new PostOptions() : new PostOptions(postOptions);
            postOptions_Resolved.RequestResult = true;

            TimeSpan delay = TimeSpan.FromSeconds(commandTimeout == 0 ? 600 : commandTimeout + 30);
            if (postOptions_Resolved.Delay < delay)
            {
                postOptions_Resolved.Delay = delay;
            }

            try
            {
                PostResponse<string?> postResponse = await DiGi.WebAPI.Query.GetAsync<string>(httpClient, urlBuilder.ToString(), postOptions_Resolved);
                if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                return Core.Convert.ToDiGi<PredictedYearBuiltRunResult>(postResponse.Result) ?? [];
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The prediction runs could not be read for county parts {CountyIds}", string.Join(", ", countyIds_Array));
                return null;
            }
        }
    }
}
