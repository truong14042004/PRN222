using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.DAL.Models;

public class OtpCode
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = null!;

    [Required]
    public DateTime ExpiredAt { get; set; }

    public bool IsUsed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
