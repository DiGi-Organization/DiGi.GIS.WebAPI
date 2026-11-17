using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads the id and location of every EPW station through <c>gis/epwfile/locations</c>, without the weather files.
        /// <para>A client positioning many buildings picks each one's station from this list - the smallest planar distance, as the server does - and reads only the stations it uses with <see cref="EPWFileByIdAsync"/> (DiGi.GIS.WebAPI#93).</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="postOptions">The options of the request; null for the defaults.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the stations in id order (empty when none is stored), or null when the request failed - never read null as no station.</returns>
        public static async Task<List<EPWFileLocationResult>?> EPWFileLocationsAsync(this GISWebAPIManager? gISWebAPIManager, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<EPWFileController>(nameof(EPWFileController.GetEPWFileLocationsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(EPWFileController.GetEPWFileLocationsAsync));
                return null;
            }

            PostOptions postOptions_Resolved = postOptions is null ? new PostOptions() : new PostOptions(postOptions);
            postOptions_Resolved.RequestResult = true;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                PostResponse<string?> postResponse = await DiGi.WebAPI.Query.GetAsync<string>(httpClient, path, postOptions_Resolved);
                if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                if (postResponse.Result.Trim() == "[]")
                {
                    return [];
                }

                return Core.Convert.ToDiGi<EPWFileLocationResult>(postResponse.Result);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The EPW station locations could not be read");
                return null;
            }
        }
    }
}
