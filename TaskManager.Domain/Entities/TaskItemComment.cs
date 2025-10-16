namespace TaskManager.Domain.Entities;

public class TaskItemComment
{
    public Guid Id { get; set; }
    public Guid TaskItemId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
}