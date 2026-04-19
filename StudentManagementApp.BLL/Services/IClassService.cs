using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IClassService
{
    Task<IEnumerable<ClassDto>> GetAllAsync();
    Task<ClassDto?> GetByIdAsync(int id);
    Task<IEnumerable<ClassDto>> GetByTeacherIdAsync(int teacherId);
    Task<IEnumerable<ClassDto>> GetByCourseIdAsync(int courseId);
    Task CreateAsync(CreateClassDto dto);
    Task UpdateAsync(UpdateClassDto dto);
    Task DeleteAsync(int id);
    Task<bool> HasScheduleConflictAsync(int teacherId, int dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeClassId = null);
}
