using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<IEnumerable<CourseDto>> GetActiveCoursesAsync();
    Task<CourseDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateCourseDto dto);
    Task UpdateAsync(UpdateCourseDto dto);
    Task DeleteAsync(int id);
}
