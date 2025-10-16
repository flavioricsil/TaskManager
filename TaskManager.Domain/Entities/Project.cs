// Entities/Project.cs
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }

    public List<TaskItem> Tasks { get; set; } = new();

    public bool CanBeDeleted => Tasks.All(t => t.Status == TaskItemStatus.Done);
}