using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs;

public class CreateTaskRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public TaskItemPriority Priority { get; set; }
    public Guid CreatedBy { get; set; }
}
