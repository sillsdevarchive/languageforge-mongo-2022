using LanguageForge.UnitTests.Fixtures;
using LanguageForge.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageForge.UnitTests.WebApi.Services;

public class ProjectServiceTest : IClassFixture<IntegrationTestFixture>
{
    private readonly ProjectService _projectService;

    public ProjectServiceTest(IntegrationTestFixture iocFixture)
    {
        _projectService = iocFixture.Services.GetRequiredService<ProjectService>();
    }

    [Fact]
    public async Task CanGetProjects()
    {
        var projects = await _projectService.ListAllProjects();
        projects.ShouldNotBeEmpty();
    }
}
