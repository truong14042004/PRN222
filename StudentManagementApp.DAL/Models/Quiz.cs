using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.DAL.Models;

public class Quiz
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CourseId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public int TimeLimitMinutes { get; set; } = 30;

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public Course Course { get; set; } = null!;
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public ICollection<QuizResult> Results { get; set; } = new List<QuizResult>();
}
