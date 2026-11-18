using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Reads the bounding-box centre of every building filed under one county part through <c>gis/building2d/centroidsbycountyid</c> - the set a server-side solar solve tiles the part by (DiGi.GIS.PostgreSQL.UI#48).
        /// <para>Coordinates come rounded to 0.01 m. Without <paramref name="postOptions"/> the request may take <paramref name="commandTimeout"/> seconds (no bound when it is 0), so the client never gives up before the server does (<c>Coding - WebAPI Contracts.md</c> §3). A failed request or a malformed answer is null, never an empty list.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="countyId">The identifier of the county part (<c>administrative_areal_2d.id</c>, never a county code).</param>
        /// <param name="commandTimeout">The server's timeout in seconds for the read; 0 disables it.</param>
        /// <param name="postOptions">The options of the request; null to bound it by <paramref name="commandTimeout"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the centroids (empty when the part holds no building), or null when an argument is out of range or the request failed - never read null as no building.</returns>
        public static async Task<List<Building2DCentroid>?> Building2DCentroidsAsync(this GISWebAPIManager? gISWebAPIManager, int countyId, int commandTimeout = 600, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || countyId < 1 || commandTimeout < 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<Building2DController>(nameof(Building2DController.GetCentroidsByCountyIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(Building2DController.GetCentroidsByCountyIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyid", countyId);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            PostOptions postOptions_Resolved = postOptions is null ? new PostOptions() { Delay = commandTimeout == 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(commandTimeout) } : new PostOptions(postOptions);
            postOptions_Resolved.RequestResult = true;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                PostResponse<string?> postResponse = await DiGi.WebAPI.Query.GetAsync<string>(httpClient, urlBuilder.ToString(), postOptions_Resolved);
                if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                List<Building2DCentroid>? result = (JsonNode.Parse(postResponse.Result) as JsonObject).ToDiGi_Building2DCentroids();
                if (result is null)
                {
                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The centroids of county part {CountyId} came back malformed", countyId);
                }

                return result;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The centroids of county part {CountyId} could not be read", countyId);
                return null;
            }
        }
    }
}
