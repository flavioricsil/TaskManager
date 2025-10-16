namespace TaskManager.Application.DTOs;

public class CreateProjectRequest
{
    public string Title { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
}
