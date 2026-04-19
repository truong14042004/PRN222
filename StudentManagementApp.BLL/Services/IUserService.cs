using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task UpdateAsync(UpdateUserDto dto);
    Task DeleteAsync(int id);
    Task UpdateEmailAsync(int userId, string newEmail);
    Task SetActiveAsync(int userId, bool isActive);
    Task ToggleActiveAsync(int userId);
}
