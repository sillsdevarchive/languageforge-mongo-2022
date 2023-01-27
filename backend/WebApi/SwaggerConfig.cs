using LanguageForge.WebApi.Validation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static LanguageForge.WebApi.Constants;

namespace LanguageForge.WebApi;

public partial class AddRequiredProjectCodeHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors.Select(x => x.Filter);

        if (!filters.Any(filter => filter is RequireProjectCodeAttribute))
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = ProjectCodeHeader,
            In = ParameterLocation.Header,
            Required = true
        });
    }
}
