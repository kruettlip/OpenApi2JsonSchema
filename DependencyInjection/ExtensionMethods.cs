using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenApi2JsonSchema.Configuration;
using System.IO;

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

        public static void UseOpenApi2JsonSchemaGenerator(this IApplicationBuilder app, JsonSchemaGeneratorConfiguration config = null)
        {
            if (config == null)
            {
                config = new JsonSchemaGeneratorConfiguration
                {
                    OpenApiFileDownloadPath = "swagger.json"
                };
            }
            if (File.Exists(config.OpenApiFileDownloadPath))
                File.Delete(config.OpenApiFileDownloadPath);
        }
    }
}
