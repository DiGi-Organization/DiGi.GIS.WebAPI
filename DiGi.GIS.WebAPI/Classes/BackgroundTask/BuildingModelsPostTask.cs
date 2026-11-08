using DiGi.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Provides functionality to handle the asynchronous posting of <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> collections to the PostgreSQL database.
    /// </summary>
    public class BuildingModelsPostTask : SerializableObjectsPostTask<DiGi.Analytical.Building.Classes.BuildingModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingModelsPostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> instance used to communicate with the server.</param>
        public BuildingModelsPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Gets or sets the administrative area code the building models belong to. It is resolved server-side to a county identifier.
        /// <para>A code does not identify a single county row - a multi-part county holds one row per polygon part - so set <see cref="CountyIds"/> instead wherever the identifiers are already known. <see cref="CountyIds"/> takes precedence when both are set.</para>
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the identifiers of the county rows the building models belong to - normally every polygon part of one county, since a single id is not evidence the code has one part. When set it is used in preference to <see cref="Code"/>, which lets the server resolve the code to every part.
        /// </summary>
        public HashSet<int>? CountyIds { get; set; }

        /// <summary>
        /// Asynchronously executes the task of posting building models to the database in memory-size-split batches.
        /// </summary>
        /// <param name="buildingModels">The collection of <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> instances to post.</param>
        /// <param name="code">The administrative area code the building models belong to.</param>
        /// <param name="longProgressWrapper">A <see cref="LongProgressWrapper"/> tracking the progress of the operation.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if all batches were posted successfully; otherwise, false.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, string? code, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken = default)
        {
            if (buildingModels is null || !buildingModels.Any())
            {
                return false;
            }

            List<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels_Batch;

            bool result = true;

            MemorySizeSplitter<DiGi.Analytical.Building.Classes.BuildingModel> memorySizeSplitter = new(buildingModels);
            while ((buildingModels_Batch = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(buildingModels_Batch.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(buildingModels_Batch, code, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously executes the task of posting building models to the database in memory-size-split batches, keyed by county identifier.
        /// </summary>
        /// <param name="buildingModels">The collection of <see cref="DiGi.Analytical.Building.Classes.BuildingModel"/> instances to post.</param>
        /// <param name="countyIds">The identifiers of the county rows the building models belong to - normally every polygon part of one county.</param>
        /// <param name="longProgressWrapper">A <see cref="LongProgressWrapper"/> tracking the progress of the operation.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if all batches were posted successfully; otherwise, false.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, IEnumerable<int>? countyIds, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken = default)
        {
            if (buildingModels is null || !buildingModels.Any())
            {
                return false;
            }

            List<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels_Batch;

            bool result = true;

            MemorySizeSplitter<DiGi.Analytical.Building.Classes.BuildingModel> memorySizeSplitter = new(buildingModels);
            while ((buildingModels_Batch = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(buildingModels_Batch.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(buildingModels_Batch, countyIds, SerializableObjectsPostOptions);
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