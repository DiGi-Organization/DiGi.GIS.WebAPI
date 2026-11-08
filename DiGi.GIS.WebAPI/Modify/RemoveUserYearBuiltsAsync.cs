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
        /// Asynchronously withdraws the user-provided year built entry of the given buildings, whoever recorded it, through <c>gis/yearbuiltdata/removeuseryearbuiltsbycountyids</c> - the moderation path, by default as a dry run.
        /// <para>Needs the access key and <c>AllowDeleteYearBuiltData</c> on the host. The objects are kept; the derived building data columns of the withdrawn buildings are recomputed on the host when it allows building data updates.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyIds">The county parts the buildings are stored under. Normally every polygon part of one county.</param>
        /// <param name="references">The references of the buildings to withdraw the user entry of.</param>
        /// <param name="dryRun">A value indicating whether the buildings are only classified.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command on the server. A value of 0 disables it.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="key">The access key; defaults to the manager's.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the classification of every reference, or null when the request was refused or failed.</returns>
        public static async Task<UserYearBuiltRemoveResult?> RemoveUserYearBuiltsAsync(
            this GISWebAPIManager? gISWebAPIManager,
            IEnumerable<int>? countyIds,
            IEnumerable<string>? references,
            bool dryRun = true,
            int commandTimeout = 600,
            PostOptions? postOptions = null,
            string? key = null,
            CancellationToken cancellationToken = default)
        {
            int[] countyIds_Array = countyIds is null ? [] : [.. countyIds.Distinct()];
            if (gISWebAPIManager is null || countyIds_Array.Length == 0 || references is null || !references.Any())
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyids", countyIds_Array);
            urlBuilder.AddParameter("dryrun", dryRun);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            string? key_Resolved = key ?? (postOptions as SerializableObjectsPostOptions)?.Key ?? gISWebAPIManager.Key;

            return await PostReferencesAsync<UserYearBuiltRemoveResult>(httpClient, urlBuilder.ToString(), references, commandTimeout, postOptions, key_Resolved, cancellationToken);
        }
    }
}
