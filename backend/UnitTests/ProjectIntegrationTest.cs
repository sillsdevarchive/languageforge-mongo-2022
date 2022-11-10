using LanguageForge.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using UnitTests.Fixtures;

namespace UnitTests;

public class ProjectIntegrationTest: IClassFixture<IocFixture>
{
    private readonly ProjectService _projectService;

    public ProjectIntegrationTest(IocFixture iocFixture)
    {
        _projectService = iocFixture.ServiceProvider.GetRequiredService<ProjectService>();
    }

    [Fact]
    public async Task CanGetProjects()
    {
        var projects = await _projectService.ListProjects();
        projects.ShouldNotBeEmpty();
    }
}