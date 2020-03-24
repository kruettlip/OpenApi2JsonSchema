using System.IO;
using System.Net.Http;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;

namespace OpenApi2JsonSchema
{
  public class JsonSchemaGenerator : IJsonSchemaGenerator
  {
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

    private static OpenApiDocument GetOpenApiDocument(string openApiUrl, string filePath = "swagger.json")
    {
      if (!File.Exists(filePath))
      {
        var httpClient = new HttpClient();

        var json = httpClient.GetStringAsync(openApiUrl).Result;
        File.WriteAllText(filePath, json);
      }

      using var stream = File.OpenRead(filePath);
      return new OpenApiStreamReader().Read(stream, out var diagnostic);
    }
  }
}
