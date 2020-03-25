# OpenApi2JsonSchema
## Getting Started
### Create OpenAPI-specifiation
First you will need to create an OpenAPI-specification, which you'll be translating to Json-Schema later.
#### Example
If you use [Swashbuckle](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/) this might look something like this:
**ConfigureServices()-method**
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
In this example, an OpenAPI-specification is created. In order to keep Data Annotations out of its model, this examples uses FluentValidationRules, that can be added easily using the last line in the example above from [MicroElements.Swashbuckle.FluentValidation](https://www.nuget.org/packages/MicroElements.Swashbuckle.FluentValidation/).

**Configure()-method**
```csharp
app.UseSwagger();

if (env.IsDevelopment())
{
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApplication v1"); });
}
```

### Installation
Install the library using NuGet-Package Manager:
```
Install-Package OpenAPI2JsonSchema
```

### Dependency Injection
First import the following Package in Startup.cs:
```csharp
using OpenApi2JsonSchema.DependencyInjection;
using OpenApi2JsonSchema.Configuration;
```

Now, in order to be able to use the library, simply add the following line to the ConfigureServices()-method in Startup.cs:
```csharp
services.AddJsonSchemaGenerator(new JsonSchemaGeneratorConfiguration
            {
                OpenApiFileDownloadPath = "swagger-file.json",
                OpenApiUrl = "http://localhost:61306/swagger/v1/swagger.json"
            });
```
The JsonSchemaGeneratorConfiguration allows to specify the following optional properties:
- OpenApiFileDownloadPath -> Path where to save the downloaded file (ignored if caching is disabled)
- OpenApiUrl -> URL to the OpenAPI-specification
- CachingDisabled -> Specify wheter or not caching should be disabled. Default is false.

_Note that if you don't specify an URL for the OpenAPI-specification here in ConfigureServices()-method, you will need to 
specify it whenever you use the JsonSchemaGenerator-instance._


### Usage
You can generate a JsonSchema for any type using:
```csharp
var schema = JsonSchemaGenerator.GetSchemaWithOpenApi<T>("<url-to-openapi-file>");
```
_If not specified in Startup.cs, just replace <url-to-openapi-file> with the URL that points to your OpenAPI-JSON-file, otherwise pass 
no parameter._

### Configuration
#### Delete OpenAPI-file on Startup
Since the library downloads the specified OpenAPI-file and caches it for further usage, you can optionally delete the downloaded file 
on Startup by adding the following lines to the Configure()-method in Startup.cs:
```csharp
app.UseOpenApi2JsonSchemaGenerator(new JsonSchemaGeneratorConfiguration
{
    OpenApiFileDownloadPath = "swagger-file.json"
});
```
_Note that the if you did not set the DownloadPath to the OpenAPI-file in ConfigureServices()-method, you don't need to pass a parameter._
