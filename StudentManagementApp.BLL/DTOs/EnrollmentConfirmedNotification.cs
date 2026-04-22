namespace StudentManagementApp.BLL.DTOs;

public class EnrollmentConfirmedNotification
{
    public int EnrollmentId { get; init; }
    public int StudentId { get; init; }
    public int ClassId { get; init; }
    public string ClassName { get; init; } = string.Empty;
    public string CourseName { get; init; } = string.Empty;
    public string ConfirmationCode { get; init; } = string.Empty;
    public DateTime ConfirmedAt { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
