using System.Linq;
using Microsoft.OpenApi.Models;
using NJsonSchema;

namespace OpenApi2JsonSchema
{
    public static class JsonSchemaExtensionMethods
    {
        public static void AddOpenApiSpecifications(this JsonSchema schema, OpenApiDocument openApiDocument, string objectName)
        {
            var properties = schema.Properties.Where(p => p.Value.ParentSchema.Title == objectName);
            foreach (var (propertyName, property) in properties)
            {
                openApiDocument.Components.Schemas.TryGetValue(objectName, out var openApiSchema);

                if (openApiSchema == null) continue;
                openApiSchema.Properties.TryGetValue(propertyName, out var openApiProperty);
                if (openApiProperty == null) continue;
                property.Maximum = openApiProperty.Maximum;
                property.Minimum = openApiProperty.Minimum;
                property.Format = openApiProperty.Format;
                property.IsNullableRaw = openApiProperty.Nullable;
                property.MaxLength = openApiProperty.MaxLength;
                property.MinLength = openApiProperty.MinLength;
                property.MaxItems = openApiProperty.MaxItems.GetValueOrDefault();
                property.MinItems = openApiProperty.MinItems.GetValueOrDefault();
                property.MultipleOf = openApiProperty.MultipleOf;
                property.MaxProperties = openApiProperty.MaxProperties.GetValueOrDefault();
                property.MinProperties = openApiProperty.MinProperties.GetValueOrDefault();
                property.Pattern = openApiProperty.Pattern;
                property.IsRequired = openApiProperty.MinLength.HasValue ? openApiProperty.MinLength > 0 : false;
            }
        }
    }
}
