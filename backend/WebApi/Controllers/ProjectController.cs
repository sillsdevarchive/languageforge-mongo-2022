using LanguageForge.WebApi.Auth;
using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanguageForge.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;
    private readonly WebUserContext _userContext;

    public ProjectController(ProjectService projectService, WebUserContext userContext)
    {
        _projectService = projectService;
        _userContext = userContext;
    }

    // GET: api/Project
    [HttpGet]
    public async Task<List<ProjectDto>> GetProjects()
    {
        return await _projectService.ListProjects(_userContext.Projects.Select(p => p.ProjectId));
    }

    // GET: api/Project/all
    [HttpGet("all")]
    [AdminRequired]
    public async Task<List<ProjectDto>> GetAllProjects()
    {
        return await _projectService.ListAllProjects();
    }

    // GET: api/Project/5
    [HttpGet("{projectCode}")]
    public string GetProject(string projectCode)
    {
        return "value";
    }

    // POST: api/Project
    [HttpPost]
    public void PostProject([FromBody] string value)
    {
    }

    // PUT: api/Project/5
    [HttpPut("{projectCode}")]
    public void PutProject(string projectCode, [FromBody] string value)
    {
    }

    // DELETE: api/Project/5
    [HttpDelete("{projectCode}")]
    public void DeleteProject(string projectCode)
    {
    }
}
