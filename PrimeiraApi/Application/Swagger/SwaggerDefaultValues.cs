using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PrimeiraApi.Application.Swagger;

/// <summary>Inclui no Swagger os valores padrão e metadados dos parâmetros da API.</summary>
public sealed class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Deprecated |= context.ApiDescription.IsDeprecated();

        foreach (var parameter in operation.Parameters ?? [])
        {
            var description = context.ApiDescription.ParameterDescriptions
                .FirstOrDefault(item => item.Name == parameter.Name);

            if (description is null)
            {
                continue;
            }

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter is OpenApiParameter openApiParameter)
            {
                openApiParameter.Required |= description.IsRequired;

                if (openApiParameter.Schema is OpenApiSchema schema &&
                    schema.Default is null &&
                    description.DefaultValue is not null)
                {
                    schema.Default = JsonValue.Create(description.DefaultValue);
                }
            }
        }
    }
}