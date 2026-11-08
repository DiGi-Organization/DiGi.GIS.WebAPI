using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously retrieves the building references that carry stored year built data for a specified county identifier.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyId">The unique identifier of the county.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout. Defaults to 30 seconds.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the set of reference strings, or null when the request failed or no partition was found.</returns>
        public static async Task<HashSet<string>?> YearBuiltDataReferencesAsync(this GISWebAPIManager? gISWebAPIManager, int countyId, int commandTimeout = 30, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || countyId <= 0 || commandTimeout < 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.GetReferencesByCountyIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.GetReferencesByCountyIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyid", countyId);
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
                    return [];
                }

                HashSet<string>? result = JsonSerializer.Deserialize<HashSet<string>>(postResponse.Result);
                return result ?? [];
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The year built data references could not be read for county {CountyId}", countyId);
                return null;
            }
        }
    }
}
