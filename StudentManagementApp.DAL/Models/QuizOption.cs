using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.DAL.Models;

public class QuizOption
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int QuestionId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string OptionText { get; set; } = string.Empty;
    
    public bool IsCorrect { get; set; }
    
    public int SortOrder { get; set; }
    
    // Navigation
    public QuizQuestion Question { get; set; } = null!;
}
