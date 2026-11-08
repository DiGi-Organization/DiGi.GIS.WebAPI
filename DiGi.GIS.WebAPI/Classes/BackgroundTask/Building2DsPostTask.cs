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
    /// Provides functionality to handle the asynchronous posting of multiple <see cref="Building2D"/> objects to the GIS PostgreSQL database.
    /// </summary>
    public class Building2DsPostTask : SerializableObjectsPostTask<Building2D>
    {
        /// <summary>
        /// Initializes a new instance of the Building2DsPostTask class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> used to manage PostgreSQL GIS operations.</param>
        public Building2DsPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the code associated with the building 2D post task.
        /// <para>A code does not identify a single county row - a multi-part county holds one row per polygon part - so set <see cref="CountyId"/> instead wherever the identifier is already known. <see cref="CountyId"/> takes precedence when both are set.</para>
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the county row the buildings belong to. When set it is used in preference to <see cref="Code"/>, which leaves the server to choose between the rows of a multi-part county.
        /// </summary>
        public int? CountyId { get; set; }

        /// <summary>
        /// Asynchronously executes the task of posting building 2D objects to the database, keyed by administrative code.
        /// </summary>
        /// <param name="values">The collection of <see cref="Building2D"/> instances to post.</param>
        /// <param name="code">The administrative code associated with the buildings.</param>
        /// <param name="longProgressWrapper">A <see cref="LongProgressWrapper"/> tracking the progress of the operation.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<Building2D>? values, string? code, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<Building2D>? building2Ds;

            bool result = true;

            MemorySizeSplitter<Building2D> memorySizeSplitter = new(values);
            while ((building2Ds = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(building2Ds.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(building2Ds, code, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously executes the task of posting building 2D objects to the database, keyed by county identifier.
        /// </summary>
        /// <param name="values">The collection of <see cref="Building2D"/> instances to post.</param>
        /// <param name="countyId">The identifier of the county row the buildings belong to.</param>
        /// <param name="longProgressWrapper">A <see cref="LongProgressWrapper"/> tracking the progress of the operation.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if all batches were posted successfully; otherwise, false.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<Building2D>? values, int countyId, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<Building2D>? building2Ds;

            bool result = true;

            MemorySizeSplitter<Building2D> memorySizeSplitter = new(values);
            while ((building2Ds = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(building2Ds.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(building2Ds, countyId, SerializableObjectsPostOptions);
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

            if (CountyId is int countyId)
            {
                return await ExecuteAsync(Values, countyId, longProgressWrapper, cancellationToken);
            }

            return await ExecuteAsync(Values, Code, longProgressWrapper, cancellationToken);
        }
    }
}