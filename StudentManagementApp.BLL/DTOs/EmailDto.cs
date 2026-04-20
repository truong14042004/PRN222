namespace StudentManagementApp.BLL.DTOs;

public class EmailDto
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsHtml { get; set; } = true;
}

public class OtpDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public bool IsValid { get; set; }
}

public class ScheduleEmailDto
{
    public string StudentName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly WeekStart { get; set; }
    public DateOnly WeekEnd { get; set; }
    public List<ScheduleSessionDto> Sessions { get; set; } = new();
}

public class ScheduleSessionDto
{
    public string DayName { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
}
