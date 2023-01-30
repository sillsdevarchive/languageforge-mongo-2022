using System.Linq.Expressions;
using LanguageForge.Api;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Dtos;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Services;

public class ProjectService
{
    private readonly SystemDbContext _systemDbContext;
    private readonly ILfProjectContext _projectContext;

    private readonly Expression<Func<Project, ProjectDto>> _projectToDto = p => new ProjectDto
    {
        Id = p.Id,
        ProjectCode = p.ProjectCode,
        ProjectName = p.ProjectName,
        InputSystems = p.InputSystems.Select(i => i.Value.Abbreviation).ToArray(),
        Users = p.Users.Select(pair => new ProjectUserDto(pair.Key, pair.Value.Role)).ToArray()
    };

    public ProjectService(SystemDbContext systemDbContext, ILfProjectContext projectContext)
    {
        _systemDbContext = systemDbContext;
        _projectContext = projectContext;
    }

    public async Task<List<ProjectDto>> ListAllProjects()
    {
        return await _systemDbContext.Projects
            .Find(FilterDefinition<Project>.Empty)
            .Project(_projectToDto)
            .ToListAsync();
    }

    public async Task<List<ProjectDto>> ListProjects(IEnumerable<string> projectCodes)
    {
        return await _systemDbContext.Projects
            .Find(p => projectCodes.Contains(p.ProjectCode))
            .Project(_projectToDto)
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetProject()
    {
        return await _systemDbContext.Projects
            .Find(p => p.ProjectCode == _projectContext.ProjectCode)
            .Project(_projectToDto)
            .SingleOrDefaultAsync();
    }
}
