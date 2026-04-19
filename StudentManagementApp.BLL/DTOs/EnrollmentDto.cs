namespace StudentManagementApp.BLL.DTOs;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = null!;
    public int ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public DateTime EnrolledAt { get; set; }
    public string Status { get; set; } = null!;
    public ClassDto? Class { get; set; }
}
