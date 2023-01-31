using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static LanguageForge.WebApi.Constants;

namespace LanguageForge.WebApi.Validation;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class RequireProjectCodeAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ProjectCodeHeader, out var projectCode)
            || string.IsNullOrEmpty(projectCode))
        {
            context.Result = new BadRequestObjectResult($"{ProjectCodeHeader} header is required");
        }
    }
}
