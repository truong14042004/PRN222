using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.DAL.Models;

public class StudentAnswer
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int QuizResultId { get; set; }
    
    [Required]
    public int QuestionId { get; set; }
    
    public int? SelectedOptionId { get; set; }
    
    [MaxLength(500)]
    public string? FillInAnswer { get; set; }
    
    public bool IsCorrect { get; set; }
    
    public int PointsEarned { get; set; }
    
    // Navigation
    public QuizResult QuizResult { get; set; } = null!;
    public QuizQuestion Question { get; set; } = null!;
    public QuizOption? SelectedOption { get; set; }
}
