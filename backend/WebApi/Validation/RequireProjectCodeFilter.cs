using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using static LanguageForge.WebApi.Constants;

namespace LanguageForge.WebApi.Validation;

public class RequireProjectCodeFilter : IResourceFilter
{
    private readonly ILfProjectContext _lfProjectContext;

    public RequireProjectCodeFilter(ILfProjectContext lfProjectContext)
    {
        _lfProjectContext = lfProjectContext;
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (_lfProjectContext.ProjectCode.IsNullOrEmpty())
        {
            var requiresProjectCode = context.ActionDescriptor.FilterDescriptors.Any(x => x.Filter is RequireProjectCodeAttribute);
            if (requiresProjectCode)
            {
                context.Result = new BadRequestObjectResult($"{ProjectCodeHeader} header is required");
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class RequireProjectCodeAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
    }
}
