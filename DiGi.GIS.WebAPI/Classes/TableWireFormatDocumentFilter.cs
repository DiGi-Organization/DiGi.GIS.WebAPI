using DiGi.Core.IO.Table.Classes;
using DiGi.WebAPI.Interfaces;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Removes the Core.IO <see cref="Row"/> component a generated document no longer references.
    /// <para>Swashbuckle registers that component while it generates a table's schema - a Core.IO table is an <c>IEnumerable&lt;Row&gt;</c>, so the array items reference it - and <see cref="TableWireFormatSchemaFilter"/> then replaces those items with the <c>TableConverter</c> format, whose rows are positional value arrays (DiGi-Organization/DiGi.GIS.WebAPI#44). Nothing else can reference the Core.IO <c>Row</c>, so without this filter the served document would keep advertising a row object schema the wire never carries.</para>
    /// <para>Registered through the host's <c>IWebAPIDocumentFilter</c> hook, after the host's own document filters, mirroring how <c>DiGi.WebAPI.WindowsService</c> removes its unreferenced enum components.</para>
    /// </summary>
    public class TableWireFormatDocumentFilter : IWebAPIDocumentFilter
    {
        /// <summary>
        /// Removes the document's Core.IO <c>Row</c> component when nothing in the document references it.
        /// </summary>
        /// <param name="swaggerDoc">The OpenAPI document to be modified.</param>
        /// <param name="context">The context of the document being generated.</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc?.Components?.Schemas is not IDictionary<string, IOpenApiSchema> schemas || schemas.Count == 0)
            {
                return;
            }

            if (!context.SchemaRepository.TryLookupByType(typeof(Row), out OpenApiSchemaReference? openApiSchemaReference_Row) || openApiSchemaReference_Row?.Reference.Id is not string id_Row)
            {
                return;
            }

            TableWireFormatDocumentFilter.SchemaReferenceVisitor schemaReferenceVisitor = new();
            new OpenApiWalker(schemaReferenceVisitor).Walk(swaggerDoc);

            if (!schemaReferenceVisitor.Ids.Contains(id_Row))
            {
                schemas.Remove(id_Row);
            }
        }

        /// <summary>
        /// Collects the id of every schema component referenced in a walked OpenAPI document, the way <c>DiGi.WebAPI.WindowsService</c>'s own visitor does.
        /// <para>Overrides <c>Visit(IOpenApiReferenceHolder)</c>: the walker reports a <c>$ref</c> there, while <c>Visit(IOpenApiSchema)</c> never sees one.</para>
        /// </summary>
        private sealed class SchemaReferenceVisitor : OpenApiVisitorBase
        {
            /// <summary>
            /// Gets the ids of the referenced schema components collected so far.
            /// </summary>
            public HashSet<string> Ids { get; } = [];

            /// <summary>
            /// Records the component id of a schema reference.
            /// </summary>
            /// <param name="referenceHolder">The reference being visited.</param>
            public override void Visit(IOpenApiReferenceHolder referenceHolder)
            {
                if (referenceHolder is OpenApiSchemaReference openApiSchemaReference && openApiSchemaReference.Reference.Id is string id)
                {
                    Ids.Add(id);
                }
            }
        }
    }
}
