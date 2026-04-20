using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.DAL.Models;

public class EmailLog
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Recipient { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;
    
    public string? Body { get; set; }
    
    public EmailType Type { get; set; }
    
    public bool IsSent { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public User User { get; set; } = null!;
}

public enum EmailType
{
    ScheduleNotification = 1,
    QuizResult = 2,
    RegistrationConfirmation = 3,
    PasswordReset = 4
}
