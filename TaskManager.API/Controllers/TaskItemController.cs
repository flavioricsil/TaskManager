using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskService;

    public TaskItemController(ITaskItemService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetTasks(Guid projectId)
    {
        var tasks = await _taskService.GetTasksByProjectAsync(projectId);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskRequest request)
    {
        var task = await _taskService.CreateTaskAsync(request);
        return CreatedAtAction(nameof(GetTasks), new { projectId = task.ProjectId }, task);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTask(UpdateTaskRequest request)
    {
        var updated = await _taskService.UpdateTaskAsync(request);
        return Ok(updated);
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        await _taskService.DeleteTaskAsync(taskId);
        return NoContent();
    }

    [HttpPost("comment")]
    public async Task<IActionResult> AddComment(AddCommentRequest request)
    {
        await _taskService.AddCommentAsync(request);
        return Ok();
    }
}