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
        /// Asynchronously recomputes the derived building data year built columns - predicted, user and calculated - of the given county parts from their stored history, through <c>gis/yearbuiltdata/updatebuildingdatabycountyids</c>, writing NULL where the history no longer holds a value.
        /// <para>Needs the access key and <c>AllowUpdateBuildingData</c> on the host.</para>
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the Web API.</param>
        /// <param name="countyIds">The county parts to recompute. Normally every polygon part of one county.</param>
        /// <param name="references">The references of the buildings to recompute, or null for every building of the parts.</param>
        /// <param name="commandTimeout">The timeout in seconds for each database command on the server. A value of 0 disables it.</param>
        /// <param name="postOptions">Optional configuration options for the HTTP request.</param>
        /// <param name="key">The access key; defaults to the manager's.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task returning the counts, or null when the request was refused or failed.</returns>
        public static async Task<BuildingDataYearBuiltUpdateResult?> UpdateBuildingDataYearBuiltAsync(this GISWebAPIManager? gISWebAPIManager, IEnumerable<int>? countyIds, IEnumerable<string>? references = null, int commandTimeout = 600, PostOptions? postOptions = null, string? key = null, CancellationToken cancellationToken = default)
        {
            int[] countyIds_Array = countyIds is null ? [] : [.. countyIds.Distinct()];
            if (gISWebAPIManager is null || countyIds_Array.Length == 0)
            {
                return null;
            }

            HttpClient? httpClient = gISWebAPIManager.CreateHttpClient<YearBuiltDataController>(nameof(YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync), out string? path);
            if (httpClient is null || string.IsNullOrWhiteSpace(path))
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "HttpClient or path for {Method} could not be resolved", nameof(YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync));
                return null;
            }

            UrlBuilder urlBuilder = new(path);
            urlBuilder.AddParameter("countyids", countyIds_Array);
            urlBuilder.AddParameter("commandtimeout", commandTimeout);

            string? key_Resolved = key ?? (postOptions as SerializableObjectsPostOptions)?.Key ?? gISWebAPIManager.Key;

            return await PostReferencesAsync<BuildingDataYearBuiltUpdateResult>(httpClient, urlBuilder.ToString(), references, commandTimeout, postOptions, key_Resolved, cancellationToken);
        }
    }
}
