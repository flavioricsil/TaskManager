using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs;

public class UpdateTaskRequest
{
    public Guid TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskItemStatus Status { get; set; }
    public Guid ModifiedBy { get; set; }
}
