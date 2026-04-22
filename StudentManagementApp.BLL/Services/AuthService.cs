using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> LoginAsync(LoginDto dto)
    {
        var credential = dto.Username.Trim();
        if (string.IsNullOrWhiteSpace(credential) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return null;
        }

        var user = await _userRepository.GetByUsernameAsync(credential);
        if (user is null)
        {
            user = await _userRepository.GetByEmailAsync(credential);
        }

        if (user is null) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;

        return MapToDto(user);
    }

    public async Task EnsureRegistrationAllowedAsync(CreateUserDto dto)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new InvalidOperationException("Email đã được sử dụng.");

        if (await _userRepository.ExistsByUsernameAsync(dto.Username))
            throw new InvalidOperationException("Tên đăng nhập đã được sử dụng.");
    }

    public async Task<UserDto> RegisterAsync(CreateUserDto dto)
    {
        await EnsureRegistrationAllowedAsync(dto);

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Username = dto.Username,
            Phone = dto.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            AvatarUrl = dto.AvatarUrl
        };

        await _userRepository.AddAsync(user);
        return MapToDto(user);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is null) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
        {
            return false;
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return false;
        }

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
        {
            return false;
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    private static UserDto MapToDto(User user) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Username = user.Username,
        Phone = user.Phone,
        Role = user.Role,
        AvatarUrl = user.AvatarUrl,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
