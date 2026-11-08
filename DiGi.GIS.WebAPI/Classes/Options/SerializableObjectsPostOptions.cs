using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Represents the configuration options for posting serializable objects, extending the base post options functionality.
    /// </summary>
    public class SerializableObjectsPostOptions : DiGi.WebAPI.Classes.PostOptions
    {
        /// <summary>
        /// Initializes a new instance of the SerializableObjectsPostOptions class.
        /// </summary>
        public SerializableObjectsPostOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SerializableObjectsPostOptions class using the specified JSON object.
        /// </summary>
        /// <param name="jsonObject">The <see cref="System.Text.Json.Nodes.JsonObject" /> containing the data used to initialize the options.</param>
        public SerializableObjectsPostOptions(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SerializableObjectsPostOptions class using values from an existing instance.
        /// </summary>
        /// <param name="serializableObjectsPostOptions">The source options instance to copy values from, or <see langword="null"/> to use default settings.</param>
        public SerializableObjectsPostOptions(SerializableObjectsPostOptions? serializableObjectsPostOptions)
            : base(serializableObjectsPostOptions)
        {
            if (serializableObjectsPostOptions is not null)
            {
                BatchMemorySize = serializableObjectsPostOptions.BatchMemorySize;
                Key = serializableObjectsPostOptions.Key;
            }
        }

        /// <summary>
        /// Gets or sets the memory size threshold in bytes used for processing batches of serializable objects.
        /// <para>This bounds the uncompressed JSON of one request. The receiving controller materializes a batch several times over - request DOM, domain objects with full geometry, database rows each re-serialized to JSONB, then one batch command per row - so the server's peak allocation is a large multiple of this value. Kept small deliberately; raising it trades server memory for fewer requests.</para>
        /// </summary>
        [JsonInclude, JsonPropertyName(nameof(BatchMemorySize))]
        public int BatchMemorySize { get; set; } = 3 * 1024 * 1024;

        /// <summary>
        /// Gets or sets the API authorization key used for authenticating requests to protected write endpoints.
        /// </summary>
        [JsonInclude, JsonPropertyName(nameof(Key))]
        public string? Key { get; set; }
    }
}