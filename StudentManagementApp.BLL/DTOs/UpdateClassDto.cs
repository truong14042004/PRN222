namespace StudentManagementApp.BLL.DTOs;

public class UpdateClassDto
{
    public int Id { get; set; }
    public string ClassName { get; set; } = null!;
    public int? TeacherId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Status { get; set; } = null!;
    public List<ClassScheduleDto> Schedules { get; set; } = new();
}
