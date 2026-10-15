using DiGi.GIS.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Modify
    {
        /// <summary>
        /// Stores the solar radiation of a building through <c>gis/buildingsolarradiation/setsolarradiation</c>, on behalf of the signed-in user whose token is given.
        /// <para>The body is the DiGi JSON of <paramref name="buildingSolarRadiationResult"/> (<c>ToJsonObject()</c>, keeping <c>_type</c> and the declared names), never <c>JsonContent.Create</c>, which would write System.Text.Json's camelCase shape (<c>Coding - WebAPI Contracts.md</c> §2). The host stamps the user email from the token and the time itself, and needs <c>AllowUpdateSolarRadiationData</c>.</para>
        /// </summary>
        /// <param name="gisWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="buildingSolarRadiationResult">The result to store; every surface must carry 8 760 shading factors.</param>
        /// <param name="tokenString">The signed-in user's bearer token.</param>
        /// <param name="postOptions">The options of the request; null for the defaults.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the id of the stored row, or null when the request was refused or failed.</returns>
        public static async Task<long?> SetBuildingSolarRadiationAsync(this GISWebAPIManager? gisWebAPIManager, BuildingSolarRadiationResult? buildingSolarRadiationResult, string? tokenString, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gisWebAPIManager is null || buildingSolarRadiationResult is null || string.IsNullOrWhiteSpace(tokenString))
            {
                return null;
            }

            string? json = buildingSolarRadiationResult.ToJsonObject()?.ToJsonString();
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            HttpClient? httpClient = gisWebAPIManager.CreateHttpClient<BuildingSolarRadiationController>(nameof(BuildingSolarRadiationController.SetSolarRadiationAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(BuildingSolarRadiationController.SetSolarRadiationAsync));
                return null;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            PostOptions postOptions_Resolved = postOptions is null ? new PostOptions() : new PostOptions(postOptions);
            postOptions_Resolved.RequestResult = true;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Passed as a factory: sending consumes the content, so a retry has to build it again.
                PostResponse<string?> postResponse = await DiGi.WebAPI.Modify.PostAsync<string>(httpClient, path, () => Task.FromResult<HttpContent?>(new StringContent(json, Encoding.UTF8, "application/json")), postOptions_Resolved);
                if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result))
                {
                    return null;
                }

                return long.TryParse(postResponse.Result.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long id) ? id : null;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The solar radiation of building model {Id} could not be stored", buildingSolarRadiationResult.BuildingModelId);
                return null;
            }
        }
    }
}
