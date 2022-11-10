using LanguageForge.Api.Dtos;
using LanguageForge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LanguageForge.Api.Controllers;

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
    [HttpGet("{id}")]
    public string GetProject(int id)
    {
        return "value";
    }

    // POST: api/Project
    [HttpPost]
    public void PostProject([FromBody] string value)
    {
    }

    // PUT: api/Project/5
    [HttpPut("{id}")]
    public void PutProject(int id, [FromBody] string value)
    {
    }

    // DELETE: api/Project/5
    [HttpDelete("{id}")]
    public void DeleteProject(int id)
    {
    }
}
