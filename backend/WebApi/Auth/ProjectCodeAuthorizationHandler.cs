using LanguageForge.Api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace LanguageForge.WebApi.Auth;

public class ProjectAuthorizationRequirement : IAuthorizationRequirement { }

public partial class ProjectCodeAuthorizationHandler : AuthorizationHandler<ProjectAuthorizationRequirement>
{
    private readonly ILfWebContext _lfWebContext;
    private readonly ILfProjectContext _lfProjectContext;

    public ProjectCodeAuthorizationHandler(ILfWebContext lfWebContext, ILfProjectContext lfProjectContext)
    {
        _lfWebContext = lfWebContext;
        _lfProjectContext = lfProjectContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAuthorizationRequirement requirement)
    {
        var projectCode = _lfProjectContext.ProjectCode;
        if (string.IsNullOrWhiteSpace(projectCode))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var lfUser = _lfWebContext.User;

        if (lfUser == null)
        {
            context.Fail(new AuthorizationFailureReason(this, "User is not authenticated"));
            return Task.CompletedTask;
        }

        if (lfUser.Role == UserRole.SystemAdmin
            || lfUser.Projects.Any(p => p.ProjectCode == projectCode))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, $"User is not authorized for project: {projectCode}"));
        }

        return Task.CompletedTask;
    }
}
