using LanguageForge.Api.Entities;
using Microsoft.AspNetCore.Authorization;

namespace LanguageForge.WebApi.Auth;

public class RoleRequiredAttribute : AuthorizeAttribute
{
    public RoleRequiredAttribute(UserRole userRole)
    {
        Roles = userRole.ToString();
    }

    public RoleRequiredAttribute(UserRole[] userRoles)
    {
        Roles = string.Join(',', userRoles);
    }
}
