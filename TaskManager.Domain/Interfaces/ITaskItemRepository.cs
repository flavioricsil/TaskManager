using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITaskItemRepository
{
    Task<List<TaskItem>> GetTasksByProjectAsync(Guid projectId);
    Task<TaskItem?> GetByIdAsync(Guid taskId);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}
