namespace StudentManagementApp.BLL.DTOs;

public class ClassScheduleDto
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
