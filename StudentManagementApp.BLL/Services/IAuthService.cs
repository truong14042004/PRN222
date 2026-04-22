using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(LoginDto dto);
    Task EnsureRegistrationAllowedAsync(CreateUserDto dto);
    Task<UserDto> RegisterAsync(CreateUserDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
}
