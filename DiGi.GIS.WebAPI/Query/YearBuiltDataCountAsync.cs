using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously retrieves the number of year built data items stored for a specified county identifier.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyId">The unique identifier of the county.</param>
        /// <param name="estimated">A boolean value indicating whether to return an estimated count from table statistics rather than an exact count.</param>
        /// <param name="analyze">A boolean value indicating whether to perform an ANALYZE operation before reading the estimate.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout. Defaults to 600 seconds.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the record count, or null when the county has no partition or the request failed.</returns>
        public static async Task<long?> YearBuiltDataCountAsync(this GISWebAPIManager? gISWebAPIManager, int countyId, bool estimated = false, bool analyze = false, int commandTimeout = 600, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || countyId <= 0 || commandTimeout < 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.GetCountByCountyIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.GetCountByCountyIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyid", countyId);
            urlBuilder.AddParameter("estimated", estimated);
            urlBuilder.AddParameter("analyze", analyze);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            string requestUri = urlBuilder.ToString();

            try
            {
                PostResponse<string?> postResponse = await DiGi.WebAPI.Query.GetAsync<string>(httpClient, requestUri, postOptions ?? new PostOptions() { RequestResult = true });
                if (postResponse is null || !postResponse.Succeeded)
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                if (long.TryParse(postResponse.Result, NumberStyles.Integer, CultureInfo.InvariantCulture, out long count))
                {
                    return count;
                }

                return null;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The year built data count could not be read for county {CountyId}", countyId);
                return null;
            }
        }
    }
}
