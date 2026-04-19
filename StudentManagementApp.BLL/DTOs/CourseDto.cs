namespace StudentManagementApp.BLL.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Level { get; set; } = null!;
    public string? Description { get; set; }
    public decimal TuitionFee { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; }
}
