using LanguageForge.WebApi;
using LanguageForge.WebApi.Auth;
using Moq;

namespace LanguageForge.UnitTests.WebApi;

public static class ContextHelpers
{
    public static ILfWebContext WebContext(LfUser user)
    {
        var contextMock = new Mock<ILfWebContext>();
        contextMock.SetupGet(c => c.User).Returns(user);
        return contextMock.Object;
    }

    public static ILfProjectContext ProjectContext(string projectCode)
    {
        var contextMock = new Mock<ILfProjectContext>();
        contextMock.SetupGet(c => c.ProjectCode).Returns(projectCode);
        return contextMock.Object;
    }
}
