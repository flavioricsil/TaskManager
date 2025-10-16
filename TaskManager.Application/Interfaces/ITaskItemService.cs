using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

public interface ITaskItemService
{
    Task<List<TaskItemDto>> GetTasksByProjectAsync(Guid projectId);
    Task<TaskItemDto> CreateTaskAsync(CreateTaskRequest request);
    Task<TaskItemDto> UpdateTaskAsync(UpdateTaskRequest request);
    Task DeleteTaskAsync(Guid taskId);
    Task AddCommentAsync(AddCommentRequest request);
}
