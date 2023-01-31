using LanguageForge.Api.Entities;
using LanguageForge.WebApi;
using LanguageForge.WebApi.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace LanguageForge.UnitTests.WebApi;

public static class ContextHelpers
{
    public static ILfWebContext WebContext(UserRole role, IReadOnlyList<string> projects)
    {
        var user = new LfUser("test@testeroolaboom.fun", LfId<User>.Parse("User:6359f8855e3dc273d4662f2a"),
            UserRole.User,
            projects.Select(p => new UserProjectRole(p, ProjectRole.Contributor)).ToArray());
        return WebContext(user);
    }

    public static ILfWebContext WebContext(LfUser user = null)
    {
        var contextMock = new Mock<ILfWebContext>();
        contextMock.SetupGet(c => c.User).Returns(user);
        return contextMock.Object;
    }

    public static ILfProjectContext ProjectContext(string projectCode = null)
    {
        var contextMock = new Mock<ILfProjectContext>();
        contextMock.SetupGet(c => c.ProjectCode).Returns(projectCode);
        return contextMock.Object;
    }

    public static ResourceExecutingContext ResourceExecutingContext(string? projectCode)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add(Constants.ProjectCodeHeader, projectCode);
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        return new ResourceExecutingContext(actionContext, new List<IFilterMetadata>(), new List<IValueProviderFactory>());
    }
}
