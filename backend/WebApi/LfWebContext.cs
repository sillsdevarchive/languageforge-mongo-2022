using LanguageForge.Api;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Auth;
using LanguageForge.WebApi.Controllers;

namespace LanguageForge.WebApi;

public interface ILfWebContext
{
    /// <summary>
    /// Authenticated user details. Null if the current user is not authenticated.
    /// </summary>
    LfUser? User { get; }
}

public class LfWebContext : ILfWebContext
{
    public LfUser? User { get; }

    public LfWebContext(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext?.User == null)
        {
            throw new InvalidOperationException($"{nameof(ILfWebContext)} should only be accessed in contexts where the user is authenticated.");
        }
        User = JwtService.ExtractLfUser(httpContext.User);
    }
}

public class LfProjectContext : ILfProjectContext
{
    public string? ProjectCode { get; }

    public LfProjectContext(IHttpContextAccessor httpContextAccessor)
    {
        object? projectCode = null;
        httpContextAccessor.HttpContext?.Request.RouteValues.TryGetValue(PathConstants.ProjectCode, out projectCode);
        ProjectCode = projectCode?.ToString();
    }
}

public static class LfWebContextHelpers
{
    public static bool IsAuthorizedForProject(this ILfWebContext lfContext, string projectCode)
    {
        return lfContext.User.Role == UserRole.SystemAdmin ||
            lfContext.User.Projects.Any(p => p.ProjectCode == projectCode);
    }
}
