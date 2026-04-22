namespace StudentManagementApp.BLL.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Phone { get; set; }
    public string Role { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public decimal WalletBalance { get; set; }
    public bool RegistrationFeePaid { get; set; }
    public DateTime CreatedAt { get; set; }
}
