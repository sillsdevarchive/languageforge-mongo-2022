using LanguageForge.WebApi.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static LanguageForge.UnitTests.WebApi.ContextHelpers;

namespace LanguageForge.UnitTests.WebApi.Validation;

public class RequireProjectCodeFilterTest
{
    [Fact]
    public void FailsIfRequiredProjectCodeNotPresent()
    {
        // GIVEN - an empty project context and a context that requires a project code
        var projectContext = ProjectContext();
        var resourceContext = ResourceExecutingContext(new List<IFilterMetadata> { new RequireProjectCodeAttribute() });

        // WHEN - the project code is validated
        var handler = new RequireProjectCodeFilter(projectContext);
        handler.OnResourceExecuting(resourceContext);

        // THEN - Validation fails
        resourceContext.Result.ShouldBeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void PassesIfRequiredProjectCodeIsPresent()
    {
        // GIVEN - an empty project context and a context that requires a project code
        var projectContext = ProjectContext("spicy-project");
        var resourceContext = ResourceExecutingContext(new List<IFilterMetadata> { new RequireProjectCodeAttribute() });

        // WHEN - the project code is validated
        var handler = new RequireProjectCodeFilter(projectContext);
        handler.OnResourceExecuting(resourceContext);

        // THEN - Validation fails
        resourceContext.Result.ShouldBeNull();
    }

    [Fact]
    public void PassesIfNoProjectCodeIsRequired()
    {
        // GIVEN - an empty project context and a context that requires a project code
        var projectContext = ProjectContext();
        var resourceContext = ResourceExecutingContext(new List<IFilterMetadata> { });

        // WHEN - the project code is validated
        var handler = new RequireProjectCodeFilter(projectContext);
        handler.OnResourceExecuting(resourceContext);

        // THEN - Validation fails
        resourceContext.Result.ShouldBeNull();
    }
}
