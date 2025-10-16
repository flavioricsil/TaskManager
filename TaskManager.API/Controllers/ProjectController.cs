using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetProjects(Guid userId)
    {
        var projects = await _projectService.GetProjectsByUserAsync(userId);
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectRequest request)
    {
        var project = await _projectService.CreateProjectAsync(request);
        return CreatedAtAction(nameof(GetProjects), new { userId = project.OwnerId }, project);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        await _projectService.DeleteProjectAsync(projectId);
        return NoContent();
    }
}
