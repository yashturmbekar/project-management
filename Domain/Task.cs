namespace Domain;

public enum TaskStatus
{
    NotStarted,
    InProgress,
    Completed
}

public class Task {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int AssignedUserId { get; set; }
}