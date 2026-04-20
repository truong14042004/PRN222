using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.DAL.Models;

public class EmailOtp
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string OtpCode { get; set; } = string.Empty;
    
    public OtpType Type { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public bool IsUsed { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsValid => !IsUsed && DateTime.UtcNow < ExpiresAt;
}

public enum OtpType
{
    Registration = 1,
    PasswordReset = 2
}
