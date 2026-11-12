using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PrimeiraApi.Application.Swagger;

/// <summary>Cria um documento Swagger para cada versão exposta da API.</summary>
public sealed class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfo(description));
        }
    }

    private static OpenApiInfo CreateInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Primeira API",
            Version = description.ApiVersion.ToString(),
            Description = "Documentação da Primeira API."
        };

        if (description.IsDeprecated)
        {
            info.Description += " Esta versão foi descontinuada.";
        }

        return info;
    }
}