using DiGi.GIS.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        private const int referenceCount_YearBuiltData_Maximum = 10000;

        /// <summary>
        /// Asynchronously retrieves year built data items for the specified references, optionally filtered by county identifier.
        /// <para>If the number of references exceeds the endpoint's maximum request limit (10,000), requests are automatically partitioned into batches.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="references">The collection of unique reference strings to retrieve.</param>
        /// <param name="countyId">An optional integer representing the county identifier to filter the results.</param>
        /// <param name="fallbackByReference">A boolean value indicating whether to perform a fallback search by reference alone if not found under the specified county.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the list of year built data items, or null when the request failed.</returns>
        public static async Task<List<YearBuiltData>?> YearBuiltDatasAsync(this GISWebAPIManager? gISWebAPIManager, IEnumerable<string>? references, int? countyId = null, bool fallbackByReference = false, PostOptions? postOptions = null, CancellationToken cancellationToken = default)
        {
            if (gISWebAPIManager is null || references is null)
            {
                return null;
            }

            HashSet<string> references_Unique = [];
            foreach (string reference in references)
            {
                if (!string.IsNullOrWhiteSpace(reference))
                {
                    references_Unique.Add(reference);
                }
            }

            if (references_Unique.Count == 0)
            {
                return [];
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.GetItemsByReferencesAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.GetItemsByReferencesAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            if (countyId.HasValue)
            {
                urlBuilder.AddParameter("countyid", countyId.Value);
            }
            if (fallbackByReference)
            {
                urlBuilder.AddParameter("fallbackbyreference", fallbackByReference);
            }

            string requestUri = urlBuilder.ToString();
            PostOptions postOptions_Resolved = postOptions ?? new PostOptions() { RequestResult = true };

            List<string> references_List = [.. references_Unique];
            List<YearBuiltData> result = [];

            for (int i = 0; i < references_List.Count; i += referenceCount_YearBuiltData_Maximum)
            {
                cancellationToken.ThrowIfCancellationRequested();

                int count = Math.Min(referenceCount_YearBuiltData_Maximum, references_List.Count - i);
                List<string> batch = references_List.GetRange(i, count);

                HttpContent? httpContent = await Create.HttpContent(batch, cancellationToken);
                if (httpContent is null)
                {
                    return null;
                }

                try
                {
                    PostResponse<string?> postResponse = await DiGi.WebAPI.Modify.PostAsync<string>(httpClient, requestUri, httpContent, postOptions_Resolved);
                    if (postResponse is null || !postResponse.Succeeded)
                    {
                        return null;
                    }

                    if (!string.IsNullOrWhiteSpace(postResponse.Result))
                    {
                        List<YearBuiltData>? items = Core.Convert.ToDiGi<YearBuiltData>(postResponse.Result);
                        if (items is not null && items.Count > 0)
                        {
                            result.AddRange(items);
                        }
                    }
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    Serilog.Modify.Log(exception, "The year built data could not be read for {Count} references in county {CountyId}", batch.Count, countyId?.ToString(CultureInfo.CurrentCulture) ?? string.Empty);
                    return null;
                }
            }

            return result;
        }
    }
}
