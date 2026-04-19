using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class Class
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ClassName { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }

    [ForeignKey(nameof(Teacher))]
    public int? TeacherId { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Upcoming"; // Upcoming, Ongoing, Finished

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Course Course { get; set; } = null!;
    public User? Teacher { get; set; }
    public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
