namespace OpenApi2JsonSchema.Configuration
{
    public class JsonSchemaGeneratorConfiguration
    {
        public bool CachingDisabled { get; set; } = false;

        public string OpenApiFileDownloadPath { get; set; } = "swagger.json";

        public string OpenApiUrl { get; set; }
    }
}
