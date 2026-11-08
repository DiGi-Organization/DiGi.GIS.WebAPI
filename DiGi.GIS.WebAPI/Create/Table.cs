using DiGi.Core.IO.Table.Classes;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI
{
    public static partial class Create
    {
        /// <summary>
        /// Creates a <see cref="Table"/> instance from a JSON object using <see cref="TableConverter{UTable, UColumn, URow}"/>.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing the serialized table structure and data.</param>
        /// <returns>A new <see cref="Table"/> instance, or <see langword="null"/> if the JSON object is null or cannot be deserialized.</returns>
        public static Table? Table(JsonObject? jsonObject)
        {
            if (jsonObject is null)
            {
                return null;
            }

#pragma warning disable CA1869 // sharing the options would be a cache (Coding - General.md 1.20)
            JsonSerializerOptions jsonSerializerOptions = new();
#pragma warning restore CA1869
            jsonSerializerOptions.Converters.Add(new TableConverter<Table, Column, Row>());

            return JsonSerializer.Deserialize<Table>(jsonObject.ToJsonString(), jsonSerializerOptions);
        }
    }
}
