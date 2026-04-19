namespace StudentManagementApp.BLL.DTOs;

public class SaveAttendanceDto
{
    public int ClassId { get; set; }
    public DateOnly Date { get; set; }
    public List<AttendanceItemDto> Items { get; set; } = new();
}

public class AttendanceItemDto
{
    public int StudentId { get; set; }
    public bool IsPresent { get; set; }
    public string? Note { get; set; }
}
