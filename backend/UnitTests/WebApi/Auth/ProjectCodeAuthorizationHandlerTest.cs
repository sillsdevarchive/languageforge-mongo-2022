using LanguageForge.Api.Entities;
using LanguageForge.WebApi;
using LanguageForge.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using static LanguageForge.UnitTests.WebApi.ContextHelpers;

namespace LanguageForge.UnitTests.WebApi.Auth;

public class ProjectCodeAuthorizationHandlerTest
{
    [Fact]
    public async Task AuthFailsIfUserIsNotAuthorized()
    {
        // GIVEN a user that is not a member of the requested project
        var webContext = WebContext(UserRole.User, new[] { "fun-language", "spooky-language" });
        var projectContext = ProjectContext("missing-language");

        // WHEN the handler is invoked
        var authContext = await InvokeAuthorizationHandler(webContext, projectContext);

        // THEN the handler fails
        authContext.HasFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task AuthSucceedsIfUserIsAdmin()
    {
        // GIVEN an admin user that is not a member of the requested project
        var webContext = WebContext(UserRole.SystemAdmin, new[] { "fun-language", "spooky-language" });
        var projectContext = ProjectContext("missing-language");

        // WHEN the handler is invoked
        var authContext = await InvokeAuthorizationHandler(webContext, projectContext);

        // THEN the handler succeeds
        authContext.HasSucceeded.ShouldBeTrue();
    }

    [Fact]
    public async Task AuthSucceedsIfUserIsProjectMember()
    {
        // GIVEN a user that is a member of the requested project
        var webContext = WebContext(UserRole.User, new[] { "fun-language", "spooky-language" });
        var projectContext = ProjectContext("spooky-language");

        // WHEN the handler is invoked
        var authContext = await InvokeAuthorizationHandler(webContext, projectContext);

        // THEN the handler succeeds
        authContext.HasSucceeded.ShouldBeTrue();
    }

    private async Task<AuthorizationHandlerContext> InvokeAuthorizationHandler(ILfWebContext webContext, ILfProjectContext projectContext)
    {
        var handler = new ProjectCodeAuthorizationHandler(webContext, projectContext);
        var req = new ProjectAuthorizationRequirement();
        var context = new AuthorizationHandlerContext(new IAuthorizationRequirement[] { req }, null, null);
        await handler.HandleAsync(context);
        return context;
    }
}
