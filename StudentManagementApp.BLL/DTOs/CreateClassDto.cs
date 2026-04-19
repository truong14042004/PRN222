namespace StudentManagementApp.BLL.DTOs;

public class CreateClassDto
{
    public string ClassName { get; set; } = null!;
    public int CourseId { get; set; }
    public int? TeacherId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<ClassScheduleDto> Schedules { get; set; } = new();
}
