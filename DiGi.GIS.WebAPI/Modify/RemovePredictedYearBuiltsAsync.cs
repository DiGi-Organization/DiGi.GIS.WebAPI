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
        /// Asynchronously removes one prediction run from the stored year built data objects of the given county parts through <c>gis/yearbuiltdata/removepredictedyearbuiltsbycountyids</c>, by default as a dry run.
        /// <para>The run is named by its stamp as <see cref="System.DateTime.Ticks"/> - the value <see cref="Query.PredictedYearBuiltRunsAsync"/> reports. Needs the access key and <c>AllowDeleteYearBuiltData</c> on the host, and <c>AllowUpdateBuildingData</c> as well with <paramref name="updateBuildingData"/>. A run on more objects than <paramref name="limit"/> is refused whole (HTTP 413) and answers null here.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyIds">The county parts to remove the run from. Normally every polygon part of one county.</param>
        /// <param name="ticks">The stamp of the run, as <see cref="System.DateTime.Ticks"/>.</param>
        /// <param name="references">The references of the buildings to remove the run from, or null for every building of the parts.</param>
        /// <param name="dryRun">A value indicating whether the entries are only counted.</param>
        /// <param name="limit">The largest number of objects the request may rewrite, from 1 to 10000.</param>
        /// <param name="updateBuildingData">A value indicating whether the derived building data year built columns of the changed buildings are recomputed after the removal.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command on the server. A value of 0 disables it.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="key">The access key; defaults to the manager's.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the counts, or null when the request was refused or failed.</returns>
        public static async Task<PredictedYearBuiltRemoveResult?> RemovePredictedYearBuiltsAsync(
            this GISWebAPIManager? gISWebAPIManager,
            IEnumerable<int>? countyIds,
            long ticks,
            IEnumerable<string>? references = null,
            bool dryRun = true,
            int limit = 10000,
            bool updateBuildingData = false,
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

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync));
                return null;
            }

            // Sent as the raw tick count, never as a formatted date: the stamp is the key the entries are stored under.
            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyids", countyIds_Array);
            urlBuilder.AddParameter("ticks", ticks.ToString(System.Globalization.CultureInfo.InvariantCulture));
            urlBuilder.AddParameter("dryrun", dryRun);
            urlBuilder.AddParameter("limit", limit);
            urlBuilder.AddParameter("updatebuildingdata", updateBuildingData);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            string? key_Resolved = key ?? (postOptions as SerializableObjectsPostOptions)?.Key ?? gISWebAPIManager.Key;

            return await PostReferencesAsync<PredictedYearBuiltRemoveResult>(httpClient, urlBuilder.ToString(), references, commandTimeout, postOptions, key_Resolved, cancellationToken);
        }
    }
}
