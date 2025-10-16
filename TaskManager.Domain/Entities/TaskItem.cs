// Entities/TaskItem.cs
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskItemPriority Priority { get; private set; }

    public List<TaskItemHistory> History { get; set; } = new();
    public List<TaskItemComment> Comments { get; set; } = new();

    public TaskItem(TaskItemPriority priority)
    {
        Priority = priority;
    }

    public void UpdateStatus(TaskItemStatus newStatus, Guid userId)
    {
        Status = newStatus;
        History.Add(new TaskItemHistory
        {
            TaskItemId = Id,
            ModifiedAt = DateTime.UtcNow,
            ModifiedBy = userId,
            ChangeDescription = $"Status changed to {newStatus}"
        });
    }

    public void AddComment(string comment, Guid userId)
    {
        Comments.Add(new TaskItemComment
        {
            TaskItemId = Id,
            Content = comment,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        });

        History.Add(new TaskItemHistory
        {
            TaskItemId = Id,
            ModifiedAt = DateTime.UtcNow,
            ModifiedBy = userId,
            ChangeDescription = $"Comment added: {comment}"
        });
    }
}