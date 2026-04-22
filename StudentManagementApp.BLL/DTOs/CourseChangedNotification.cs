namespace StudentManagementApp.BLL.DTOs;

public class CourseChangedNotification
{
    public string Action { get; init; } = string.Empty;
    public int CourseId { get; init; }
    public CourseDto? Course { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}
