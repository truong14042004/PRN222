namespace StudentManagementApp.BLL.DTOs;

public class AttendanceDto
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = null!;
    public DateOnly Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Note { get; set; }
}
