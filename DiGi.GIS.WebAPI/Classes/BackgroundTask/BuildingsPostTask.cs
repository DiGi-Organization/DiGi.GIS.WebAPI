using DiGi.CityGML.Classes;
using DiGi.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Provides functionality to handle the asynchronous posting of <see cref="Building"/> collections to the PostgreSQL database.
    /// </summary>
    public class BuildingsPostTask : SerializableObjectsPostTask<Building>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingsPostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The GIS PostgreSQL Web API manager used to handle data persistence.</param>
        public BuildingsPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the code associated with the buildings post task.
        /// <para>A code does not identify a single county row - a multi-part county holds one row per polygon part - so set <see cref="CountyId"/> instead wherever the identifier is already known. <see cref="CountyId"/> takes precedence when both are set.</para>
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the county row the buildings belong to. When set it is used in preference to <see cref="Code"/>, which leaves the server to choose between the rows of a multi-part county.
        /// </summary>
        public int? CountyId { get; set; }

        /// <summary>
        /// Asynchronously executes the task of posting building objects to the database.
        /// </summary>
        protected async Task<bool> ExecuteAsync(IEnumerable<Building>? values, string? code, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            Utf8JsonBatch<Building>? utf8JsonBatch;

            bool result = true;

            // Utf8JsonSplitter serializes each building exactly once and hands the bytes over, so the
            // batch is not serialized a second time on the way into the request body.
            Utf8JsonSplitter<Building> utf8JsonSplitter = new(values);
            while ((utf8JsonBatch = utf8JsonSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(utf8JsonBatch.SerializableObjects.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(utf8JsonBatch.Utf8Json, code, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously executes the task of posting building objects to the database, keyed by county identifier.
        /// </summary>
        /// <param name="values">The collection of <see cref="Building"/> instances to post.</param>
        /// <param name="countyId">The identifier of the county row the buildings belong to.</param>
        /// <param name="longProgressWrapper">A <see cref="LongProgressWrapper"/> tracking the progress of the operation.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if all batches were posted successfully; otherwise, false.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<Building>? values, int countyId, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            Utf8JsonBatch<Building>? utf8JsonBatch;

            bool result = true;

            // Utf8JsonSplitter serializes each building exactly once and hands the bytes over, so the
            // batch is not serialized a second time on the way into the request body.
            Utf8JsonSplitter<Building> utf8JsonSplitter = new(values);
            while ((utf8JsonBatch = utf8JsonSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(utf8JsonBatch.SerializableObjects.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(utf8JsonBatch.Utf8Json, countyId, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <inheritdoc />
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