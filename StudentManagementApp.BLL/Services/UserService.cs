using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task UpdateAsync(UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(dto.Id)
            ?? throw new InvalidOperationException("Không tìm thấy người dùng.");

        user.FullName = dto.FullName;
        user.Phone = dto.Phone;
        user.AvatarUrl = dto.AvatarUrl;
        user.IsActive = dto.IsActive;

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(int id) =>
        await _userRepository.DeleteAsync(id);

    public async Task UpdateEmailAsync(int userId, string newEmail)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("Không tìm thấy người dùng.");

        user.Email = newEmail;
        await _userRepository.UpdateAsync(user);
    }

    public async Task SetActiveAsync(int userId, bool isActive)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("Không tìm thấy người dùng.");

        user.IsActive = isActive;
        await _userRepository.UpdateAsync(user);
    }

    public async Task ToggleActiveAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("Không tìm thấy người dùng.");

        user.IsActive = !user.IsActive;
        await _userRepository.UpdateAsync(user);
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
