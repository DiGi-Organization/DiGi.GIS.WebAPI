using DiGi.Core.Classes;
using DiGi.EPW.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Provides functionality to handle the asynchronous posting of multiple <see cref="EPWFile"/> objects to the GIS PostgreSQL database.
    /// </summary>
    public class EPWFilesPostTask : SerializableObjectsPostTask<EPWFile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EPWFilesPostTask"/> class.
        /// </summary>
        /// <param name="gISWebAPIManager">The <see cref="GISWebAPIManager"/> used to manage PostgreSQL GIS operations.</param>
        public EPWFilesPostTask(GISWebAPIManager gISWebAPIManager)
            : base(gISWebAPIManager)
        {
        }

        /// <summary>
        /// Asynchronously executes the posting of EPW files.
        /// </summary>
        /// <param name="ePWFiles">The collection of EPW files to post.</param>
        /// <param name="longProgressWrapper">The progress wrapper to track the number of posted files.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, returning <see langword="true"/> if the post succeeded; otherwise, <see langword="false"/>.</returns>
        protected async Task<bool> ExecuteAsync(IEnumerable<EPWFile>? ePWFiles, LongProgressWrapper? longProgressWrapper, CancellationToken cancellationToken = default)
        {
            if (ePWFiles is null || !ePWFiles.Any())
            {
                return false;
            }

            List<EPWFile>? ePWFiles_Batch;

            bool result = true;

            MemorySizeSplitter<EPWFile> memorySizeSplitter = new(ePWFiles);
            while ((ePWFiles_Batch = memorySizeSplitter.Next(SerializableObjectsPostOptions.BatchMemorySize)) is not null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                longProgressWrapper?.Increment(ePWFiles_Batch.Count);

                result = await gISWebAPIManager.UpdateItemsAsync(ePWFiles_Batch, SerializableObjectsPostOptions);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously executes the background task, reporting progress.
        /// </summary>
        /// <param name="progress">The progress reporter for reporting progress updates.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, returning <see langword="true"/> if the operation succeeded; otherwise, <see langword="false"/>.</returns>
        protected override async Task<bool> ExecuteAsync(IProgress<long> progress, CancellationToken cancellationToken)
        {
            return await ExecuteAsync(Values, Core.Create.LongProgressWrapper(progress), cancellationToken);
        }
    }
}