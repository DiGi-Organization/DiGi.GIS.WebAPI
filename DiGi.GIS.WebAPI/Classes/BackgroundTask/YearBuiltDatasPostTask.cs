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
    /// Provides functionality to handle the asynchronous posting of <see cref="YearBuiltData"/> collections to the PostgreSQL database.
    /// </summary>
    public class YearBuiltDatasPostTask : SerializableObjectsPostTask<YearBuiltData>
    {
        /// <summary>
        /// Initializes a new instance of the YearBuiltDatasPostTask class.
        /// </summary>
        /// <param name="gISWebAPIManager">The GIS PostgreSQL Web API manager used to handle data persistence.</param>
        public YearBuiltDatasPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the code associated with the year built data post task.
        /// <para>A code does not identify a single county row - a multi-part county holds one row per polygon part - so set <see cref="CountyIds"/> instead wherever the identifiers are already known. <see cref="CountyIds"/> takes precedence when both are set.</para>
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the identifiers of the county rows the year built data belong to - normally every polygon part of one county, since a single id is not evidence the code has one part. When set it is used in preference to <see cref="Code"/>, which lets the server resolve the code to every part.
        /// </summary>
        public HashSet<int>? CountyIds { get; set; }

        protected async Task<bool> ExecuteAsync(IEnumerable<YearBuiltData>? values, string? code, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<YearBuiltData>? yearBuiltDatas;

            bool result = true;

            MemorySizeSplitter<YearBuiltData> memorySizeSplitter = new(values);
            while ((yearBuiltDatas = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(yearBuiltDatas.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(yearBuiltDatas, code, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        protected async Task<bool> ExecuteAsync(IEnumerable<YearBuiltData>? values, IEnumerable<int>? countyIds, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<YearBuiltData>? yearBuiltDatas;

            bool result = true;

            MemorySizeSplitter<YearBuiltData> memorySizeSplitter = new(values);
            while ((yearBuiltDatas = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(yearBuiltDatas.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(yearBuiltDatas, countyIds, SerializableObjectsPostOptions);
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

            // A set of identifiers names the county rows outright; a code only narrows it to the rows of a
            // multi-part county and lets the server pick them, so it is the fallback.
            if (CountyIds is { Count: > 0 } countyIds)
            {
                return await ExecuteAsync(Values, countyIds, longProgressWrapper, cancellationToken);
            }

            return await ExecuteAsync(Values, Code, longProgressWrapper, cancellationToken);
        }
    }
}