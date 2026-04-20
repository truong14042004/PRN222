using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class QuizResult
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    [Required]
    public int QuizId { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Score { get; set; }
    
    public int TotalPoints { get; set; }
    
    public int CorrectCount { get; set; }
    
    public int TotalQuestions { get; set; }
    
    public TimeSpan TimeTaken { get; set; }
    
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    
    public bool Passed => TotalPoints > 0 && (Score / TotalPoints * 100) >= 50;
    
    // Navigation
    public User Student { get; set; } = null!;
    public Quiz Quiz { get; set; } = null!;
    
    public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
