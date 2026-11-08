using DiGi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Create
    {
        /// <summary>
        /// Converts a collection of serializable objects into an <see cref="System.Net.Http.HttpContent"/> object by first serializing them to a JSON string.
        /// </summary>
        /// <typeparam name="TSerializableObject">The type of the objects in the collection, which must implement <see cref="ISerializableObject"/>.</typeparam>
        /// <param name="serializableObjects">The collection of objects to be serialized and converted to HTTP content.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task<HttpContent?> HttpContent<TSerializableObject>(this IEnumerable<TSerializableObject> serializableObjects, CancellationToken cancellationToken) where TSerializableObject : ISerializableObject
        {
            string? json = Core.Convert.ToSystem_String(serializableObjects);

            return await HttpContent(json ?? string.Empty, cancellationToken);
        }

        /// <summary>
        /// Converts a collection of strings into an <see cref="System.Net.Http.HttpContent"/> object holding a JSON array.
        /// </summary>
        /// <param name="values">The collection of strings to be serialized and converted to HTTP content.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task<HttpContent?> HttpContent(this IEnumerable<string>? values, CancellationToken cancellationToken)
        {
            if (values is null)
            {
                return null;
            }

            JsonArray jsonArray = [.. values];

            return await HttpContent(jsonArray.ToJsonString(), cancellationToken);
        }

        /// <summary>
        /// Converts a JSON string into an asynchronous <see cref="System.Net.Http.HttpContent"/> object.
        /// </summary>
        /// <param name="json">The JSON string to be converted.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task<HttpContent?> HttpContent(this string json, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            // Convert JSON string to byte array (UTF8)
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            return await HttpContent(jsonBytes, cancellationToken);
        }

        /// <summary>
        /// Asynchronously creates GZip-compressed HttpContent from the provided byte array.
        /// </summary>
        /// <param name="bytes">The raw byte array to be compressed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task<HttpContent?> HttpContent(this byte[] bytes, CancellationToken cancellationToken)
        {
            return await HttpContent(bytes, Constants.Compression.Level, cancellationToken);
        }

        /// <summary>
        /// Asynchronously creates GZip-compressed HttpContent from the provided byte array using the specified compression level.
        /// </summary>
        /// <param name="bytes">The raw byte array to be compressed.</param>
        /// <param name="compressionLevel">The compression level applied to the payload.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task<HttpContent?> HttpContent(this byte[] bytes, CompressionLevel compressionLevel, CancellationToken cancellationToken)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            // Use a pooled memory stream or a simple one, but ensure we don't dispose it before StreamContent uses it.
            MemoryStream memoryStream = new();

            try
            {
                using (GZipStream gzipStream = new(memoryStream, compressionLevel, leaveOpen: true))
                {
                    // If cancellationToken is already cancelled, this throws OperationCanceledException
                    await gzipStream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
                }

                memoryStream.Position = 0;

                // StreamContent takes ownership of the memoryStream and will dispose it.
                StreamContent result = new(memoryStream);

                result.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                {
                    CharSet = Encoding.UTF8.WebName
                };
                result.Headers.ContentEncoding.Add("gzip");

                return result;
            }
            catch (Exception)
            {
                memoryStream.Dispose();
                throw; // Re-throw to be caught in ExecuteAsync
            }
        }
    }
}