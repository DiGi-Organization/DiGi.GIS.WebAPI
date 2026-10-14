using DiGi.WebAPI.Interfaces;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Carries the floor a <see cref="MinimumAttribute"/> declares on an action parameter into that parameter's OpenAPI schema, so the served document states the domain with a <c>minimum</c> and no <c>maximum</c> (DiGi-Organization/DiGi.GIS.WebAPI#47).
    /// <para>Swashbuckle maps only the DataAnnotations it knows, so a custom attribute is invisible to it; but the pass that generates a query parameter's schema runs every registered schema filter with the parameter in the context, which is where this one reads the floor from. Registered through the host's <c>IWebAPISchemaFilter</c> hook, the same way as <see cref="TableWireFormatSchemaFilter"/>.</para>
    /// <para>The floor is rendered through <see cref="CultureInfo.InvariantCulture"/> because an OpenAPI number keyword is an identity, not prose: a decimal separator following the serving machine's culture would describe a different bound to every reader.</para>
    /// </summary>
    public class ParameterMinimumSchemaFilter : IWebAPISchemaFilter
    {
        /// <summary>
        /// Sets <c>minimum</c> on the schema of a parameter carrying <see cref="MinimumAttribute"/> - or <c>exclusiveMinimum</c> alone for an exclusive floor, which is how <c>Microsoft.OpenApi</c> carries a strict bound: setting <c>ExclusiveMinimum</c> clears <c>Minimum</c>, so the two never compete over the same edge.
        /// </summary>
        /// <param name="schema">The OpenAPI schema to be modified.</param>
        /// <param name="context">The context containing information about the schema being filtered.</param>
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema is not OpenApiSchema openApiSchema || context?.ParameterInfo is null)
            {
                return;
            }

            MinimumAttribute? minimumAttribute = context.ParameterInfo.GetCustomAttribute<MinimumAttribute>();
            if (minimumAttribute is null)
            {
                return;
            }

            string minimum = minimumAttribute.Minimum.ToString(CultureInfo.InvariantCulture);
            if (minimumAttribute.Exclusive)
            {
                openApiSchema.ExclusiveMinimum = minimum;
            }
            else
            {
                openApiSchema.Minimum = minimum;
            }
        }
    }
}
