using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> GetProjectsByUserAsync(Guid userId)
    {
        var projects = await _projectRepository.GetProjectsByUserAsync(userId);
        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectRequest request)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            OwnerId = request.OwnerId
        };

        await _projectRepository.AddAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task DeleteProjectAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
            throw new Exception("Projeto não encontrado.");

        if (!project.CanBeDeleted)
            throw new Exception("Projeto não pode ser removido. Existem tarefas pendentes.");

        await _projectRepository.DeleteAsync(project);
    }
}