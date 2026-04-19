namespace StudentManagementApp.BLL.DTOs;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
}
