using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public TaskItemService(ITaskItemRepository taskRepository, IProjectRepository projectRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<List<TaskItemDto>> GetTasksByProjectAsync(Guid projectId)
    {
        var tasks = await _taskRepository.GetTasksByProjectAsync(projectId);
        return _mapper.Map<List<TaskItemDto>>(tasks);
    }

    public async Task<TaskItemDto> CreateTaskAsync(CreateTaskRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new Exception("Projeto não encontrado.");

        if (project.Tasks.Count >= 20)
            throw new Exception("Limite de tarefas por projeto atingido.");

        var task = new TaskItem(request.Priority)
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate
        };

        task.UpdateStatus(TaskItemStatus.Pending, request.CreatedBy);
        await _taskRepository.AddAsync(task);
        return _mapper.Map<TaskItemDto>(task);
    }

    public async Task<TaskItemDto> UpdateTaskAsync(UpdateTaskRequest request)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
            throw new Exception("Tarefa não encontrada.");

        if (request.Title != null) task.Title = request.Title;
        if (request.Description != null) task.Description = request.Description;
        if (request.DueDate.HasValue) task.DueDate = request.DueDate.Value;

        task.UpdateStatus(request.Status, request.ModifiedBy);
        await _taskRepository.UpdateAsync(task);
        return _mapper.Map<TaskItemDto>(task);
    }

    public async Task DeleteTaskAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
            throw new Exception("Tarefa não encontrada.");

        await _taskRepository.DeleteAsync(task);
    }

    public async Task AddCommentAsync(AddCommentRequest request)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
            throw new Exception("Tarefa não encontrada.");

        task.AddComment(request.Content, request.CreatedBy);
        await _taskRepository.UpdateAsync(task);
    }
}