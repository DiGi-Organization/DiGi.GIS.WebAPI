using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.WebAPI.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Modify
    {
        /// <summary>
        /// Asynchronously deletes stored year built data objects of the given county parts through <c>gis/yearbuiltdata/removeitemsbycountyids</c> - by default only the objects holding no entry, and by default as a dry run.
        /// <para>Needs the access key and <c>AllowDeleteYearBuiltData</c> on the host. Without references only <paramref name="emptyOnly"/> is accepted. A scope matching more rows than <paramref name="limit"/> is refused whole (HTTP 413) and answers null here - run it as a dry run first to read the count.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyIds">The county parts to delete from. Normally every polygon part of one county.</param>
        /// <param name="references">The references of the buildings whose objects are deleted, or null for every building of the parts.</param>
        /// <param name="emptyOnly">A value indicating whether only objects holding no entry are deleted.</param>
        /// <param name="dryRun">A value indicating whether the rows are only counted.</param>
        /// <param name="limit">The largest number of rows the request may delete, from 1 to 10000.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command on the server. A value of 0 disables it.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="key">The access key; defaults to the manager's.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the counts, or null when the request was refused or failed.</returns>
        public static async Task<YearBuiltDataRemoveResult?> RemoveYearBuiltDatasAsync(
            this GISWebAPIManager? gISWebAPIManager,
            IEnumerable<int>? countyIds,
            IEnumerable<string>? references = null,
            bool emptyOnly = true,
            bool dryRun = true,
            int limit = 10000,
            int commandTimeout = 600,
            PostOptions? postOptions = null,
            string? key = null,
            CancellationToken cancellationToken = default)
        {
            int[] countyIds_Array = countyIds is null ? [] : [.. countyIds.Distinct()];
            if (gISWebAPIManager is null || countyIds_Array.Length == 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.RemoveItemsByCountyIdsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.RemoveItemsByCountyIdsAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyids", countyIds_Array);
            urlBuilder.AddParameter("emptyonly", emptyOnly);
            urlBuilder.AddParameter("dryrun", dryRun);
            urlBuilder.AddParameter("limit", limit);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            string? key_Resolved = key ?? (postOptions as SerializableObjectsPostOptions)?.Key ?? gISWebAPIManager.Key;

            return await PostReferencesAsync<YearBuiltDataRemoveResult>(httpClient, urlBuilder.ToString(), references, commandTimeout, postOptions, key_Resolved, cancellationToken);
        }
    }
}
