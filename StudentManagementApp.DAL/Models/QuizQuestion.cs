using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public enum QuestionType
{
    MultipleChoice = 1,
    FillInBlank = 2
}

public class QuizQuestion
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int QuizId { get; set; }
    
    [Required]
    public string QuestionText { get; set; } = string.Empty;
    
    public QuestionType QuestionType { get; set; } = QuestionType.MultipleChoice;
    
    public int Point { get; set; } = 10;
    
    public int SortOrder { get; set; }
    
    // Navigation
    public Quiz Quiz { get; set; } = null!;
    public ICollection<QuizOption> Options { get; set; } = new List<QuizOption>();
    
    [NotMapped]
    public string? CorrectAnswer { get; set; }
}
