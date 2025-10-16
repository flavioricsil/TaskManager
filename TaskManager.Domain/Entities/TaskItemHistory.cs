namespace TaskManager.Domain.Entities;

public class TaskItemHistory
{
    public Guid Id { get; set; }
    public Guid TaskItemId { get; set; }
    public string ChangeDescription { get; set; } = string.Empty;
    public DateTime ModifiedAt { get; set; }
    public Guid ModifiedBy { get; set; }
}