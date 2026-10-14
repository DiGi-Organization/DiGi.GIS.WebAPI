using DiGi.Core.IO.Table.Interfaces;
using DiGi.WebAPI.Interfaces;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Rewrites the schema of a Core.IO table payload so that it describes the format the DiGi <c>TableConverter</c> writes, not what the MVC JSON options would.
    /// <para>Every <c>buildingdata/table*</c> operation answers with <c>Core.IO.Table.Convert.ToSystem_String&lt;Table, Column, Row&gt;</c>: a root object carrying exactly <c>Columns</c>, an array of DiGi-serialized column objects, and <c>Rows</c>, an array of positional value arrays - one value per column, in column order - with no root <c>_type</c> (DiGi-Organization/DiGi.GIS.WebAPI#44).</para>
    /// <para>Registered through the host's <c>IWebAPISchemaFilter</c> hook, so it runs after <c>DiGi.WebAPI.WindowsService</c>'s <c>WireFormatSchemaFilter</c> and fully replaces whatever that filter produced for the type. A Core.IO table is not an <c>ISerializableObject</c>, so without this filter the host would describe the type's public members in camelCase, which matches neither writer.</para>
    /// <para>The column items are an open schema requiring only <c>_type</c> instead of referencing a component: the document's <c>Column</c> schema id is already taken by <c>DiGi.PostgreSQL.Table.Classes.Column</c> (the <c>buildingdata/columns*</c> operations serve it), and a second component registration would conflict on Swashbuckle's default id.</para>
    /// </summary>
    public class TableWireFormatSchemaFilter : IWebAPISchemaFilter
    {
        /// <summary>
        /// Replaces the schema of a Core.IO table with the <c>TableConverter</c> wire format: an object requiring exactly <c>Columns</c> and <c>Rows</c>.
        /// </summary>
        /// <param name="schema">The OpenAPI schema to be modified.</param>
        /// <param name="context">The context containing information about the schema being filtered.</param>
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema is not OpenApiSchema openApiSchema || context?.Type is null)
            {
                return;
            }

            if (!typeof(ITable).IsAssignableFrom(context.Type))
            {
                return;
            }

            string name_Type = DiGi.Core.Constants.Serialization.PropertyName.Type;

            openApiSchema.Type = JsonSchemaType.Object;
            openApiSchema.Items = null;
            openApiSchema.Properties = new Dictionary<string, IOpenApiSchema>
            {
                [nameof(ITable<,>.Columns)] = new OpenApiSchema
                {
                    Type = JsonSchemaType.Array,
                    Description = "The table's columns, each written by the DiGi serializer: a `_type` discriminator naming the concrete column type, whose members follow.",
                    Items = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Object,
                        Description = "A DiGi column object. The concrete type is named by `_type` - `DiGi.Core.IO.Table.Classes.Column` (members `Name`, `Type`, `Index`) or a derived type such as `DiGi.Core.IO.Table.Classes.ExtendedColumn` (adds `Category`, `Description`) - so the members beyond `_type` are those of that type.",
                        Properties = new Dictionary<string, IOpenApiSchema>
                        {
                            [name_Type] = new OpenApiSchema
                            {
                                Type = JsonSchemaType.String,
                                Description = "Type discriminator written on every DiGi payload: the full name of the serialized type and the short name of its assembly, `Namespace.Type,ShortAssembly` (no version, culture or key).",
                                Example = JsonValue.Create("DiGi.Core.IO.Table.Classes.ExtendedColumn,DiGi.Core.IO")
                            }
                        },
                        Required = new HashSet<string> { name_Type },
                        AdditionalPropertiesAllowed = true
                    }
                },
                [nameof(ITable<,>.Rows)] = new OpenApiSchema
                {
                    Type = JsonSchemaType.Array,
                    Description = "The table's rows.",
                    Items = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Array,
                        Description = "One row: one value per column, in column order. A cell may be null, a primitive, or an object written by the DiGi serializer.",
                        Items = new OpenApiSchema()
                    }
                }
            };
            openApiSchema.Required = new HashSet<string> { nameof(ITable<,>.Columns), nameof(ITable<,>.Rows) };
            openApiSchema.AdditionalPropertiesAllowed = false;
            openApiSchema.AdditionalProperties = null;
        }
    }
}
