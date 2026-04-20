using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class CourseProgress
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    public int CompletedLessons { get; set; }
    
    public int TotalLessons { get; set; } = 100;
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Percentage => TotalLessons > 0 ? (CompletedLessons * 100m / TotalLessons) : 0;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public User Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
