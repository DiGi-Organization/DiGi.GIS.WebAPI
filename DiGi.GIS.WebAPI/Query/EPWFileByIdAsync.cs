using DiGi.EPW.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads the EPW file of one <c>epw_file</c> row through <c>gis/epwfile/itembyid</c>.
        /// <para>A client picks each building's station from <see cref="EPWFileLocationsAsync"/> and reads every station it uses once, by id, instead of resolving the location of every building (DiGi.GIS.WebAPI#93).</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="id">The id of the <c>epw_file</c> row.</param>
        /// <param name="postOptions">The options of the request; null for the defaults.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="EPWFile"/>, or null when the id is not positive, no row has it, or the request failed.</returns>
        public static async Task<EPWFile?> EPWFileByIdAsync(this GISWebAPIManager? gISWebAPIManager, int id, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || id <= 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<EPWFileController>(nameof(EPWFileController.GetEPWFileByIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(EPWFileController.GetEPWFileByIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("id", id);

            PostOptions postOptions_Resolved = postOptions is null ? new PostOptions() : new PostOptions(postOptions);
            postOptions_Resolved.RequestResult = true;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                PostResponse<string?> postResponse = await DiGi.WebAPI.Query.GetAsync<string>(httpClient, urlBuilder.ToString(), postOptions_Resolved);
                if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                return Core.Convert.ToDiGi<EPWFile>(postResponse.Result)?.FirstOrDefault();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The EPW file of row {Id} could not be read", id);
                return null;
            }
        }
    }
}
