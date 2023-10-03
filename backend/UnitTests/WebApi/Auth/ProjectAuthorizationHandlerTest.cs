using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using static LanguageForge.UnitTests.WebApi.ContextHelpers;

namespace LanguageForge.UnitTests.WebApi.Auth;

public class ProjectAuthorizationHandlerTest
{
    [Fact]
    public async Task FailsIfUserIsNotAuthorized()
    {
        // GIVEN a user that is not authorized
        var user = new LfUser("test@testeroolaboom.fun", LfId<User>.Parse("User:6359f8855e3dc273d4662f2a"),
            UserRole.User,
            new[] {
                new UserProjectRole("fun-language", ProjectRole.Manager),
                new UserProjectRole("spooky-language", ProjectRole.Contributor),
             });
        var webContext = WebContext(user);
        var projectContext = ProjectContext("missing-language");

        // WHEN the handler is invoked
        var handler = new ProjectAuthorizationHandler(webContext, projectContext);
        var req = new ProjectAuthorizationRequirement();
        var context = new AuthorizationHandlerContext(new IAuthorizationRequirement[] { req }, null, null);
        await handler.HandleAsync(context);

        // THEN the handler fails
        context.HasFailed.ShouldBeTrue();
    }

    [Fact]
    public async Task SucceedsIfUserIsAdmin()
    {
        // GIVEN an admin user
        var user = new LfUser("test@testeroolaboom.fun", LfId<User>.Parse("User:6359f8855e3dc273d4662f2a"),
            UserRole.SystemAdmin,
            new[] {
                new UserProjectRole("fun-language", ProjectRole.Manager),
                new UserProjectRole("spooky-language", ProjectRole.Contributor),
             });
        var webContext = WebContext(user);
        var projectContext = ProjectContext("missing-language");

        // WHEN the handler is invoked
        var handler = new ProjectAuthorizationHandler(webContext, projectContext);
        var req = new ProjectAuthorizationRequirement();
        var context = new AuthorizationHandlerContext(new IAuthorizationRequirement[] { req }, null, null);
        await handler.HandleAsync(context);

        // THEN the handler succeeds
        context.HasSucceeded.ShouldBeTrue();
    }
}
