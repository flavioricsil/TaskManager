using AutoMapper;
using FluentAssertions;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using Xunit;

namespace TaskManager.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly IMapper _mapper;
        private Mock<IProjectRepository> _mockRepo;
        private ProjectService _service;

        public ProjectServiceTests()
        {
            var loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, loggerFactory);
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task DeleteProject_ShouldThrowException_WhenTasksArePending()
        {
            // Arrange
            _mockRepo = new Mock<IProjectRepository>();

            var pendingTask = new TaskItem(TaskItemPriority.Medium)
            {
                Id = Guid.NewGuid(),
                Title = "Tarefa Pendente",
                Status = TaskItemStatus.Pending
            };

            var project = new Project
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Title = "Projeto Teste com Pendências",
                Tasks = new List<TaskItem> { pendingTask }
            };

            _mockRepo.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
            _service = new ProjectService(_mockRepo.Object, _mapper);

            // Act
            Func<Task> act = async () => await _service.DeleteProjectAsync(project.Id);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Projeto não pode ser removido. Existem tarefas pendentes.");

            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProject_ShouldDelete_WhenNoPendingTasks()
        {
            // Arrange
            _mockRepo = new Mock<IProjectRepository>();
            var project = new Project
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Title = "Projeto Teste Sem Pendências",
                Tasks = new List<TaskItem>() // Sem tarefas
            };

            _mockRepo.Setup(r => r.GetByIdAsync(project.Id)).ReturnsAsync(project);
            _service = new ProjectService(_mockRepo.Object, _mapper);

            // Act
            await _service.DeleteProjectAsync(project.Id);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(project), Times.Once);
        }

        [Fact]
        public async Task CreateProject_ShouldReturnProjectDto()
        {
            // Arrange
            _mockRepo = new Mock<IProjectRepository>();

            var request = new CreateProjectRequest
            {
                Title = "Novo Projeto",
                OwnerId = Guid.NewGuid()
            };

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                OwnerId = request.OwnerId
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>()))
                     .Returns(Task.FromResult(project));

            _service = new ProjectService(_mockRepo.Object, _mapper);

            // Act
            var result = await _service.CreateProjectAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);
            result.OwnerId.Should().Be(request.OwnerId);
        }

        [Fact]
        public async Task GetProjectsByUserAsync_ShouldReturnProjects()
        {
            // Arrange
            _mockRepo = new Mock<IProjectRepository>();

            var userId = Guid.NewGuid();
            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Title = "Projeto 1", OwnerId = userId },
                new Project { Id = Guid.NewGuid(), Title = "Projeto 2", OwnerId = userId }
            };

            _mockRepo.Setup(r => r.GetProjectsByUserAsync(userId)).ReturnsAsync(projects);
            _service = new ProjectService(_mockRepo.Object, _mapper);

            // Act
            var result = await _service.GetProjectsByUserAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Projeto 1");
            result[1].Title.Should().Be("Projeto 2");
        }
    }
}