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
    /// Provides functionality to asynchronously post a collection of <see cref="AdministrativeAreal2D"/> objects to the PostgreSQL database.
    /// </summary>
    public class AdministrativeAreal2DsPostTask : SerializableObjectsPostTask<AdministrativeAreal2D>
    {
        /// <summary>
        /// Initializes a new instance of the AdministrativeAreal2DsPostTask class.
        /// </summary>
        /// <param name="gISWebAPIManager">The GIS PostgreSQL Web API manager used to handle database operations.</param>
        public AdministrativeAreal2DsPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        protected async Task<bool> ExecuteAsync(IEnumerable<AdministrativeAreal2D>? values, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken)
        {
            if (values is null || !values.Any())
            {
                return false;
            }

            List<AdministrativeAreal2D>? administrativeAreal2Ds;

            bool result = true;

            MemorySizeSplitter<AdministrativeAreal2D> memorySizeSplitter = new(values);
            while ((administrativeAreal2Ds = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                result = await gISWebAPIManager.UpdateItemsAsync(administrativeAreal2Ds, SerializableObjectsPostOptions);

                longProgressWrapper?.Increment(administrativeAreal2Ds.Count);

                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            return await ExecuteAsync(Values, Core.Create.LongProgressWrapper(progress), cancellationToken);
        }
    }
}