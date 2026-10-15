using DiGi.GIS.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
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
        /// Reads the stored solar radiation of a building model through <c>gis/buildingsolarradiation/solarradiationbybuildingmodelid</c>.
        /// <para>The query parameter names are the controller's (<c>id</c>, <c>countyid</c>, <c>radius</c>, <c>solverversion</c>, <c>shadingfactors</c>): an unknown name would be ignored by the host and answer an unfiltered 200 (<c>Coding - WebAPI Contracts.md</c> §1).</para>
        /// </summary>
        /// <param name="gisWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="buildingModelId">The id of the building model.</param>
        /// <param name="countyId">The county part the building is filed under.</param>
        /// <param name="radius">The neighbour radius, in m, exactly as stored (rounded by the producer to 0.1 m).</param>
        /// <param name="solverVersion">The solver version.</param>
        /// <param name="includeShadingFactors">True to read every surface's shading factors too.</param>
        /// <param name="postOptions">The options of the request; null for the defaults.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the stored result, or null when nothing is stored for the key or the request failed.</returns>
        public static async Task<BuildingSolarRadiationResult?> BuildingSolarRadiationAsync(
            this GISWebAPIManager? gisWebAPIManager,
            long buildingModelId,
            int countyId,
            double radius,
            int solverVersion,
            bool includeShadingFactors = false,
            PostOptions? postOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (gisWebAPIManager is null || buildingModelId <= 0 || !double.IsFinite(radius) || radius <= 0 || solverVersion <= 0)
            {
                return null;
            }

            HttpClient? httpClient = gisWebAPIManager.CreateHttpClient<BuildingSolarRadiationController>(nameof(BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("id", buildingModelId);
            urlBuilder.AddParameter("countyid", countyId);
            // TODO [UrlBuilderInvariantDouble]: UrlBuilder.AddParameter(string, double) formats with CurrentCulture, so under
            // pl-PL 50.5 goes out as "50,5" and the exact radius key never matches. Pass the invariant string until that
            // overload formats with CultureInfo.InvariantCulture (DiGi.WebAPI), then go back to the double overload.
            urlBuilder.AddParameter("radius", radius.ToString(CultureInfo.InvariantCulture));
            urlBuilder.AddParameter("solverversion", solverVersion);
            if (includeShadingFactors)
            {
                urlBuilder.AddParameter("shadingfactors", includeShadingFactors);
            }

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

                return Core.Convert.ToDiGi<BuildingSolarRadiationResult>(postResponse.Result)?.FirstOrDefault();
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The stored solar radiation of building model {Id} (county {CountyId}) could not be read", buildingModelId, countyId);
                return null;
            }
        }
    }
}
