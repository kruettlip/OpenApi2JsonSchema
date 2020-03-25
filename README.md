# OpenApi2JsonSchema
## Getting Started
### Create OpenAPI-specifiation
First you will need to create an OpenAPI-specification, which you'll be translating to Json-Schema later.
#### Example
If you use [Swashbuckle](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/) this might look something like this:
```csharp
services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo {Title = "MyApplication", Version = "v1"});
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.AddFluentValidationRules();
      });
```
In this example, an OpenAPI-specification is created. In order to keep Data Annotations out of its model, this examples uses FluentValidationRules, that can be added easily using the last line in the example above.

### Installation
Install the library using NuGet-Package Manager:
```
Install-Package OpenAPI2JsonSchema
```

### Dependency Injection
In order to be able to use the library, simply add the following line to the ConfigureServices()-method in Startup.cs:
```csharp
services.AddJsonSchemaGenerator();
```
For this you will need to import the dependency in Startup.cs:
```csharp
using OpenApi2JsonSchema;
```

### Usage
You can generate a JsonSchema for any type using:
```csharp
var schema = JsonSchemaGenerator.GetSchemaWithOpenApi<T>("<url-to-openapi-file>");
var schemaWithSpecifiedFilepath = JsonSchemaGenerator.GetSchemaWithOpenApi<T>("<url-to-openapi-file>", "<openapi-filepath>");
var schemaNoCaching = JsonSchemaGenerator.GetSchemaWithOpenApi<T>("<url-to-openapi-file>", "", false);
```
_Just replace <url-to-openapi-file> with the URL that points to your OpenAPI-JSON-file and <openapi-filepath> with the path you want to save the OpenAPI-JSON-file to (only if caching is enabled -> default)._

#### Create SchemaProvider
Unless you want to specify the OpenAPI-path every time

### Configuration
#### Delete OpenAPI-file on Startup
Since the library downloads the specified OpenAPI-file and caches it for further usage, you can optionally delete the downloaded file on Startup by adding the following lines to the Configure()-method in Startup.cs:
```csharp
if (File.Exists("swagger.json"))
        File.Delete("swagger.json");
```
_Note that the path to the OpenAPI-file can vary, depending on what download-path you specified._
