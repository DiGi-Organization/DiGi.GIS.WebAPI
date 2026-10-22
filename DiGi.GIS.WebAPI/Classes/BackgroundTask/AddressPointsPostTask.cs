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
    /// Provides functionality to handle the asynchronous posting of multiple PRG <see cref="AddressPoint"/> objects to the GIS PostgreSQL database.
    /// </summary>
    public class AddressPointsPostTask : SerializableObjectsPostTask<AddressPoint>
    {
        /// <summary>
        /// Initializes a new instance of the AddressPointsPostTask class.
        /// </summary>
        /// <param name="GISWebAPIManager">The <see cref="GISWebAPIManager"/> used to manage PostgreSQL GIS operations.</param>
        public AddressPointsPostTask(GISWebAPIManager GISWebAPIManager)
            : base(GISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the identifiers of the county rows the address points belong to - every polygon part of one county, never a county code: a multi-part county holds one row per part, and the server assigns each point to the part it lies in.
        /// </summary>
        public IEnumerable<int>? CountyIds { get; set; }

        /// <summary>
        /// Asynchronously executes the task of posting address points to the database, keyed by county identifiers.
        /// </summary>
        /// <param name="values">The collection of <see cref="AddressPoint"/> instances to post.</param>
        /// <param name="countyIds">The identifiers of the county rows the address points belong to.</param>
        /// <param name="longProgressWrapper">A <see cref="LongProgressWrapper"/> tracking the progress of the operation.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if all batches were posted successfully; otherwise, false.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<AddressPoint>? values, IEnumerable<int>? countyIds, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any() || countyIds is null || !countyIds.Any())
            {
                return false;
            }

            List<AddressPoint>? addressPoints;

            bool result = true;

            MemorySizeSplitter<AddressPoint> memorySizeSplitter = new(values);
            while ((addressPoints = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(addressPoints.Count);

                result = await GISWebAPIManager.UpdateItemsAsync(addressPoints, countyIds, SerializableObjectsPostOptions);
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

            return await ExecuteAsync(Values, CountyIds, longProgressWrapper, cancellationToken);
        }
    }
}
