using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectsByUserAsync(Guid userId);
    Task<ProjectDto> CreateProjectAsync(CreateProjectRequest request);
    Task DeleteProjectAsync(Guid projectId);
}
