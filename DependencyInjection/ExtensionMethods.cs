using Microsoft.Extensions.DependencyInjection;

namespace OpenApi2JsonSchema.DependencyInjection
{
    public static class ExtensionMethods
    {
        public static void AddJsonSchemaGenerator(this IServiceCollection services)
        {
            services.AddTransient<IJsonSchemaGenerator, JsonSchemaGenerator>();
        }
    }
}
