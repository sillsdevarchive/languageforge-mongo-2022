using LanguageForge.WebApi.Validation;
using Microsoft.AspNetCore.Mvc;
using static LanguageForge.UnitTests.WebApi.ContextHelpers;

namespace LanguageForge.UnitTests.WebApi.Validation;

public class RequireProjectCodeAttributeTest
{
    [Fact]
    public void FailsIfProjectCodeIsNotPresent()
    {
        // GIVEN - no project code
        var resourceContext = ResourceExecutingContext(null);

        // WHEN - the project code is validated
        var handler = new RequireProjectCodeAttribute();
        handler.OnResourceExecuting(resourceContext);

        // THEN - Validation fails
        resourceContext.Result.ShouldBeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void PassesIfProjectCodeIsPresent()
    {
        // GIVEN - a project code
        var resourceContext = ResourceExecutingContext("spicy-project");

        // WHEN - the project code is validated
        var handler = new RequireProjectCodeAttribute();
        handler.OnResourceExecuting(resourceContext);

        // THEN - Validation passes
        resourceContext.Result.ShouldBeNull();
    }
}
