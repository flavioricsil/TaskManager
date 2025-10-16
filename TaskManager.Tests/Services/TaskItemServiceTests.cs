using AutoMapper;
using FluentAssertions;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class TaskItemServiceTests
    {
        private readonly ITaskItemService _taskItemService;
        private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly IMapper _mapper;

        public TaskItemServiceTests()
        {
            var loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, loggerFactory);
            _mapper = config.CreateMapper();

            _taskItemRepositoryMock = new Mock<ITaskItemRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();

            _taskItemService = new TaskItemService(
                _taskItemRepositoryMock.Object,
                _projectRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldThrowException_WhenProjectHas20Tasks()
        {
            var projectId = Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                OwnerId = Guid.NewGuid(),
                Title = "Projeto Cheio",
                Tasks = Enumerable.Range(1, 20).Select(i => new TaskItem(TaskItemPriority.Medium)).ToList()
            };

            _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId)).Returns(Task.FromResult(project));

            var request = new CreateTaskRequest
            {
                ProjectId = projectId,
                Title = "Nova Tarefa",
                Description = "Descrição",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskItemPriority.High,
                CreatedBy = Guid.NewGuid()
            };

            var exception = await Assert.ThrowsAsync<Exception>(() => _taskItemService.CreateTaskAsync(request));
            exception.Message.Should().Be("Limite de tarefas por projeto atingido.");
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldCreateTask_WhenValidRequest()
        {
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, Tasks = new List<TaskItem>() };

            _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId)).Returns(Task.FromResult(project));

            var request = new CreateTaskRequest
            {
                ProjectId = projectId,
                Title = "Nova Tarefa",
                Description = "Descrição",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskItemPriority.High,
                CreatedBy = Guid.NewGuid()
            };

            var task = new TaskItem(request.Priority)
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate
            };

            _taskItemRepositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).Returns(Task.FromResult(task));

            var result = await _taskItemService.CreateTaskAsync(request);

            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);
            result.Description.Should().Be(request.Description);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask_WhenTaskExists()
        {
            var taskId = Guid.NewGuid();
            var task = new TaskItem(TaskItemPriority.Medium) { Id = taskId };

            _taskItemRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).Returns(Task.FromResult(task));

            await _taskItemService.DeleteTaskAsync(taskId);

            _taskItemRepositoryMock.Verify(r => r.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldThrowException_WhenTaskNotFound()
        {
            var taskId = Guid.NewGuid();

            _taskItemRepositoryMock.Setup(r => r.GetByIdAsync(taskId)).Returns(Task.FromResult<TaskItem>(null));

            Func<Task> act = async () => await _taskItemService.DeleteTaskAsync(taskId);

            await act.Should().ThrowAsync<Exception>().WithMessage("Tarefa não encontrada.");
        }
    }
}