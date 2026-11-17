using DiGi.Core.Classes;
using DiGi.GIS.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Modify
    {
        /// <summary>
        /// Stores the solar radiation of many buildings through <c>gis/buildingsolarradiation/setsolarradiations</c>, on behalf of the signed-in user whose token is given - the batch counterpart of <see cref="SetBuildingSolarRadiationAsync"/> (DiGi.GIS.WebAPI#93).
        /// <para>The results are grouped by their <see cref="BuildingSolarRadiationResult.CountyId"/> - a request never mixes county parts - and each group is split into requests of at most <see cref="SerializableObjectsPostOptions.BatchMemorySize"/> bytes of DiGi JSON and <see cref="Constants.SolarRadiation.MaximumBatchCount"/> results. Every request is all-or-nothing on the server, so a failure stops here and leaves only whole requests stored; a rerun that skips the stored references (<see cref="Query.BuildingSolarRadiationReferencesAsync"/>) resumes it.</para>
        /// <para>Each body is the DiGi JSON of the results (<c>ToJsonObject()</c>, keeping <c>_type</c> and the declared names), never <c>JsonContent.Create</c> (<c>Coding - WebAPI Contracts.md</c> §2). The host stamps the user email from the token and the time itself, and needs <c>AllowUpdateSolarRadiationData</c>. Without <paramref name="serializableObjectsPostOptions"/> one request may take <paramref name="commandTimeout"/> seconds (no bound when it is 0), so the client never gives up before the server does (§3).</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The manager that creates the HTTP client.</param>
        /// <param name="buildingSolarRadiationResults">The results to store; every surface must carry 8 760 shading factors.</param>
        /// <param name="tokenString">The signed-in user's bearer token.</param>
        /// <param name="commandTimeout">The server's timeout in seconds for the lookups and the write of one request; 0 disables it.</param>
        /// <param name="serializableObjectsPostOptions">The options of the requests, including the memory size of one; null for the defaults bounded by <paramref name="commandTimeout"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of results stored, which is the number given, or null when an argument is invalid, a result cannot be serialized, or a request was refused or failed - the requests before it stay stored.</returns>
        public static async Task<int?> SetBuildingSolarRadiationsAsync(this GISWebAPIManager? gISWebAPIManager, IEnumerable<BuildingSolarRadiationResult>? buildingSolarRadiationResults, string? tokenString, int commandTimeout = 600, SerializableObjectsPostOptions? serializableObjectsPostOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || buildingSolarRadiationResults is null || string.IsNullOrWhiteSpace(tokenString) || commandTimeout < 0)
            {
                return null;
            }

            // Grouped in the order a part is first met, so a request carries the results of one county part only.
            Dictionary<int, List<BuildingSolarRadiationResult>> buildingSolarRadiationResults_ByCountyId = [];
            foreach (BuildingSolarRadiationResult? buildingSolarRadiationResult in buildingSolarRadiationResults)
            {
                if (buildingSolarRadiationResult is null)
                {
                    return null;
                }

                if (!buildingSolarRadiationResults_ByCountyId.TryGetValue(buildingSolarRadiationResult.CountyId, out List<BuildingSolarRadiationResult>? buildingSolarRadiationResults_CountyId))
                {
                    buildingSolarRadiationResults_CountyId = [];
                    buildingSolarRadiationResults_ByCountyId[buildingSolarRadiationResult.CountyId] = buildingSolarRadiationResults_CountyId;
                }

                buildingSolarRadiationResults_CountyId.Add(buildingSolarRadiationResult);
            }

            if (buildingSolarRadiationResults_ByCountyId.Count == 0)
            {
                return 0;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<BuildingSolarRadiationController>(nameof(BuildingSolarRadiationController.SetSolarRadiationsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(BuildingSolarRadiationController.SetSolarRadiationsAsync));
                return null;
            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);
            string requestUri = urlBuilder.ToString();

            SerializableObjectsPostOptions serializableObjectsPostOptions_Resolved = new(serializableObjectsPostOptions);
            if (serializableObjectsPostOptions is null)
            {
                serializableObjectsPostOptions_Resolved.Delay = commandTimeout == 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(commandTimeout);
            }

            serializableObjectsPostOptions_Resolved.RequestResult = true;

            int result = 0;
            try
            {
                foreach (KeyValuePair<int, List<BuildingSolarRadiationResult>> keyValuePair in buildingSolarRadiationResults_ByCountyId)
                {
                    MemorySizeSplitter<BuildingSolarRadiationResult> memorySizeSplitter = new(keyValuePair.Value);

                    List<BuildingSolarRadiationResult>? buildingSolarRadiationResults_Batch;
                    while ((buildingSolarRadiationResults_Batch = memorySizeSplitter.Next(serializableObjectsPostOptions_Resolved.BatchMemorySize)) is not null)
                    {
                        for (int index = 0; index < buildingSolarRadiationResults_Batch.Count; index += Constants.SolarRadiation.MaximumBatchCount)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            int count = Math.Min(Constants.SolarRadiation.MaximumBatchCount, buildingSolarRadiationResults_Batch.Count - index);

                            JsonArray jsonArray = [];
                            for (int i = index; i < index + count; i++)
                            {
                                JsonObject? jsonObject = buildingSolarRadiationResults_Batch[i].ToJsonObject();
                                if (jsonObject is null)
                                {
                                    Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "The solar radiation of building model {Id} could not be serialized; {Stored} results were stored before it", buildingSolarRadiationResults_Batch[i].BuildingModelId, result);
                                    return null;
                                }

                                jsonArray.Add(jsonObject);
                            }

                            string json = jsonArray.ToJsonString();

                            // Passed as a factory: sending consumes the content, so a retry has to build it again.
                            PostResponse<string?> postResponse = await DiGi.WebAPI.Modify.PostAsync<string>(httpClient, requestUri, () => Task.FromResult<HttpContent?>(new StringContent(json, Encoding.UTF8, "application/json")), serializableObjectsPostOptions_Resolved);
                            if (postResponse is null || !postResponse.Succeeded || string.IsNullOrWhiteSpace(postResponse.Result) || !int.TryParse(postResponse.Result.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int written) || written != count)
                            {
                                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "A request of {Count} solar radiation results of county part {CountyId} was not stored; {Stored} results were stored before it", count, keyValuePair.Key, result);
                                return null;
                            }

                            result += written;
                        }
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                Serilog.Modify.Log(exception, "The solar radiation results could not be stored; {Stored} results were stored before the failure", result);
                return null;
            }

            return result;
        }
    }
}
