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
        /// Reads the references of the buildings of a county part that already hold a stored solar radiation for one neighbour radius and solver version, through <c>gis/buildingsolarradiation/referencesbycountyid</c>.
        /// <para>The skip-existing read of a client-side solve (DiGi.GIS.WebAPI#93): one call per part and variant, as the server-side solve reads it. Without <paramref name="postOptions"/> the request may take <paramref name="commandTimeout"/> seconds (no bound when it is 0), so the client never gives up before the server does.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="countyId">The identifier of the county part (<c>administrative_areal_2d.id</c>).</param>
        /// <param name="radius">The neighbour radius, in m, exactly as stored; 0 for the isolated building.</param>
        /// <param name="solverVersion">The solver version.</param>
        /// <param name="commandTimeout">The server's timeout in seconds for the read; 0 disables it.</param>
        /// <param name="postOptions">The options of the request; null to bound it by <paramref name="commandTimeout"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the references (empty when none is stored), or null when an argument is out of range or the request failed - never read null as nothing solved.</returns>
        public static async Task<HashSet<string>?> BuildingSolarRadiationReferencesAsync(this GISWebAPIManager? gISWebAPIManager, int countyId, double radius, int solverVersion, int commandTimeout = 600, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || countyId < 1 || !double.IsFinite(radius) || radius < 0 || solverVersion <= 0 || commandTimeout < 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<BuildingSolarRadiationController>(nameof(BuildingSolarRadiationController.GetReferencesByCountyIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(BuildingSolarRadiationController.GetReferencesByCountyIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyid", countyId);
            urlBuilder.AddParameter("radius", radius);
            urlBuilder.AddParameter("solverversion", solverVersion);
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

                if (JsonNode.Parse(postResponse.Result) is not JsonArray jsonArray)
                {
                    return null;
                }

                HashSet<string> result = [];
                foreach (JsonNode? jsonNode in jsonArray)
                {
                    if (jsonNode is JsonValue jsonValue && jsonValue.TryGetValue(out string? reference) && !string.IsNullOrWhiteSpace(reference))
                    {
                        result.Add(reference);
                    }
                }

                return result;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The solved references of county part {CountyId} (radius {Radius} m, solver version {SolverVersion}) could not be read", countyId, radius, solverVersion);
                return null;
            }
        }
    }
}
