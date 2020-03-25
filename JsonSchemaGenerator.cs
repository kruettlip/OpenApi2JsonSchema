using System.IO;
using System.Net.Http;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
using OpenApi2JsonSchema.Configuration;

namespace OpenApi2JsonSchema
{
    public class JsonSchemaGenerator : IJsonSchemaGenerator
    {
        public string FilePath { get; set; }

        public bool CachingDisabled { get; set; }

        public JsonSchemaGenerator()
        {
            var config = new JsonSchemaGeneratorConfiguration();
            SetPropertiesBasedOnConfig(config);
        }

        public JsonSchemaGenerator(JsonSchemaGeneratorConfiguration config)
        {
            SetPropertiesBasedOnConfig(config);
        }

        private void SetPropertiesBasedOnConfig(JsonSchemaGeneratorConfiguration config)
        {
            FilePath = config.OpenApiFileDownloadPath;
            CachingDisabled = config.CachingDisabled;
        }

        public JsonSchema GetSchemaWithOpenApi<T>(string openApiUrl)
        {
            var schema = GetSchema<T>();
            var openApiDocument = GetOpenApiDocument(openApiUrl);
            var objectName = typeof(T).Name;
            schema.AddOpenApiSpecifications(openApiDocument, objectName);
            return schema;
        }

        public JsonSchema GetSchema<T>(JsonSchemaGeneratorSettings generatorSettings = null)
        {
            var schema = JsonSchema.FromType<T>(generatorSettings ?? new JsonSchemaGeneratorSettings
            {
                FlattenInheritanceHierarchy = false,
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Include,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                }
            });
            return schema;
        }

        private OpenApiDocument GetOpenApiDocument(string openApiUrl)
        {
            string json = "";

            if (!File.Exists(FilePath) || CachingDisabled)
            {
                var httpClient = new HttpClient();
                json = httpClient.GetStringAsync(openApiUrl).Result;
                if (!CachingDisabled)
                {
                    File.WriteAllText(FilePath, json);
                }
            }

            if (!CachingDisabled)
            {
                using var stream = File.OpenRead(FilePath);
                return new OpenApiStreamReader().Read(stream, out var diagnostic);
            }
            else
            {
                return new OpenApiStringReader().Read(json, out var diagnostic);
            }
        }
    }
}
