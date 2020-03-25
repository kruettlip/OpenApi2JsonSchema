using Microsoft.Extensions.DependencyInjection;
using OpenApi2JsonSchema.Configuration;

namespace OpenApi2JsonSchema.DependencyInjection
{
    public static class ExtensionMethods
    {
        public static void AddJsonSchemaGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSchemaGenerator, JsonSchemaGenerator>();
        }

        public static void AddJsonSchemaGenerator(this IServiceCollection services, JsonSchemaGeneratorConfiguration config)
        {
            services.AddSingleton<IJsonSchemaGenerator>(g => new JsonSchemaGenerator(config));
        }
    }
}
