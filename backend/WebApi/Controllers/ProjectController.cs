using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanguageForge.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: api/Project
    [HttpGet]
    public async Task<List<ProjectDto>> GetProjects()
    {
        return await _projectService.ListProjects();
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
