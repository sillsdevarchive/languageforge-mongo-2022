using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Auth;
using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using static LanguageForge.WebApi.Controllers.PathConstants;

namespace LanguageForge.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;
    private readonly ILfWebContext _lfContext;

    public ProjectController(ProjectService projectService, ILfWebContext lfContext)
    {
        _projectService = projectService;
        _lfContext = lfContext;
    }

    // GET: api/Project
    [HttpGet]
    public async Task<List<ProjectDto>> GetProjects()
    {
        return await _projectService.ListProjects(_lfContext.User.Projects.Select(p => p.ProjectCode));
    }

    // GET: api/Project/all
    [HttpGet("all")]
    [RoleRequired(UserRole.SystemAdmin)]
    public async Task<List<ProjectDto>> GetAllProjects()
    {
        return await _projectService.ListAllProjects();
    }

    // GET: api/Project/5
    [HttpGet($"{{{ProjectCode}}}")]
    public async Task<ProjectDto?> GetProject(string projectCode)
    {
        return await _projectService.GetProject(projectCode);
    }

    // POST: api/Project
    [HttpPost]
    public void PostProject([FromBody] string value)
    {
    }

    // PUT: api/Project/5
    [HttpPut($"{{{ProjectCode}}}")]
    public void PutProject([FromBody] string value)
    {
    }

    // DELETE: api/Project/5
    [HttpDelete($"{{{ProjectCode}}}")]
    public void DeleteProject()
    {
    }
}
