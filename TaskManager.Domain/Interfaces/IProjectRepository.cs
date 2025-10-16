using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetProjectsByUserAsync(Guid userId);
    Task<Project?> GetByIdAsync(Guid projectId);
    Task AddAsync(Project project);
    Task DeleteAsync(Project project);
}