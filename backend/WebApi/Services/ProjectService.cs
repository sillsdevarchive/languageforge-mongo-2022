using LanguageForge.Api;
using LanguageForge.WebApi.Dtos;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Services;

public class ProjectService
{
    private readonly SystemDbContext _systemDbContext;

    public ProjectService(SystemDbContext systemDbContext)
    {
        _systemDbContext = systemDbContext;
    }

    public async Task<List<ProjectDto>> ListProjects()
    {
        return await _systemDbContext.Projects
            .Find(_ => true)
            .Project(p => new ProjectDto
            {
                Id = p.Id,
                ProjectCode = p.ProjectCode,
                ProjectName = p.ProjectName,
                InputSystems = p.InputSystems.Select(i => i.Value.Abbreviation).ToArray(),
            })
            .ToListAsync();
    }
}
