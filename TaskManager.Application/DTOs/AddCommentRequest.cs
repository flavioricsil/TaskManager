namespace TaskManager.Application.DTOs;

public class AddCommentRequest
{
    public Guid TaskId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid CreatedBy { get; set; }
}
