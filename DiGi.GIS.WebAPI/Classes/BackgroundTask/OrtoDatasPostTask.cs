using DiGi.Core.Classes;
using DiGi.GIS.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Handles the posting of OrtoDatas objects to the PostgreSQL web API.
    /// </summary>
    public class OrtoDatasPostTask : SerializableObjectsPostTask<OrtoDatas>
    {
        /// <summary>
        /// Handles the posting of OrtoDatas objects to the PostgreSQL web API.
        /// <para>OrtoDatas carry imagery, so <see cref="SerializableObjectsPostTask{T}.SerializableObjectsPostOptions"/> defaults to <see cref="DefaultDelay"/> per attempt and <see cref="DefaultBatchMemorySize"/> per request instead of the general 20 s and 3 MB. No host exposes these settings, so these defaults are the production values.</para>
        /// </summary>
        /// <param name="GISWebAPIManager">The manager instance used to handle PostgreSQL web API operations.</param>
        public OrtoDatasPostTask(GISWebAPIManager GISWebAPIManager)
            : base(GISWebAPIManager)
        {
            SerializableObjectsPostOptions = new() { Delay = DefaultDelay, BatchMemorySize = DefaultBatchMemorySize };
        }

        /// <summary>
        /// Default per-attempt upload timeout for OrtoDatas. Kept below the 60 s <see cref="System.Net.Http.HttpClient.Timeout"/> set in <see cref="Create.ServiceProvider"/>, which would otherwise cut the attempt first.
        /// </summary>
        public static readonly TimeSpan DefaultDelay = TimeSpan.FromSeconds(45);

        /// <summary>
        /// Default uncompressed batch size in bytes for OrtoDatas uploads (1 MB), so a slow write fails less often and a retry resends less.
        /// </summary>
        public const int DefaultBatchMemorySize = 1024 * 1024;

        /// <summary>
        /// Gets or sets the code associated with the OrtoDatas post task.
        /// <para>A code does not identify a single county row - a multi-part county holds one row per polygon part - so set <see cref="CountyId"/> instead wherever the identifier is already known. <see cref="CountyId"/> takes precedence when both are set.</para>
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the county row the OrtoDatas belong to. When set it is used in preference to <see cref="Code"/>, which leaves the server to choose between the rows of a multi-part county.
        /// </summary>
        public int? CountyId { get; set; }

        protected async Task<bool> ExecuteAsync(IEnumerable<OrtoDatas>? values, string? code, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<OrtoDatas>? ortoDatasList;
            bool result = true;

            MemorySizeSplitter<OrtoDatas> memorySizeSplitter = new(values);
            while ((ortoDatasList = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(ortoDatasList.Count);

                result = await GISWebAPIManager.UpdateItemsAsync(ortoDatasList, code, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        protected async Task<bool> ExecuteAsync(IEnumerable<OrtoDatas>? values, int countyId, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<OrtoDatas>? ortoDatasList;
            bool result = true;

            MemorySizeSplitter<OrtoDatas> memorySizeSplitter = new(values);
            while ((ortoDatasList = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(ortoDatasList.Count);

                result = await GISWebAPIManager.UpdateItemsAsync(ortoDatasList, countyId, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously posts orthodata in memory-sized batches, naming the county rows they belong to.
        /// <para>Several identifiers are the polygon parts of one county, and the server files each entry under the part holding its <c>building_2d</c> row; a single identifier is written verbatim.</para>
        /// </summary>
        /// <param name="values">The orthodata to post.</param>
        /// <param name="countyIds">The identifiers of the county rows the orthodata belongs to - normally every polygon part of one county.</param>
        /// <param name="longProgressWrapper">Tracks the number of entries posted.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. True when every batch was accepted; false when there was nothing to post or a batch was refused.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<OrtoDatas>? values, IEnumerable<int>? countyIds, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any() || countyIds is null || !countyIds.Any())
            {
                return false;
            }

            List<OrtoDatas>? ortoDatasList;
            bool result = true;

            MemorySizeSplitter<OrtoDatas> memorySizeSplitter = new(values);
            while ((ortoDatasList = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(ortoDatasList.Count);

                result = await GISWebAPIManager.UpdateItemsAsync(ortoDatasList, countyIds, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            LongProgressWrapper? longProgressWrapper = Core.Create.LongProgressWrapper(progress);

            // An identifier names the county row outright; a code only narrows it to the rows of a
            // multi-part county and lets the server pick one, so it is the fallback.
            if (CountyId is int countyId)
            {
                return await ExecuteAsync(Values, countyId, longProgressWrapper, cancellationToken);
            }

            return await ExecuteAsync(Values, Code, longProgressWrapper, cancellationToken);
        }
    }
}