using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        return courses.Select(MapToDto);
    }

    public async Task<IEnumerable<CourseDto>> GetActiveCoursesAsync()
    {
        var courses = await _courseRepository.GetActiveCoursesAsync();
        return courses.Select(MapToDto);
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        return course is null ? null : MapToDto(course);
    }

    public async Task CreateAsync(CreateCourseDto dto)
    {
        await _courseRepository.AddAsync(new Course
        {
            Name = dto.Name,
            Level = dto.Level,
            Description = dto.Description,
            TuitionFee = dto.TuitionFee,
            ThumbnailUrl = dto.ThumbnailUrl
        });
    }

    public async Task UpdateAsync(UpdateCourseDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(dto.Id)
            ?? throw new InvalidOperationException("Không tìm thấy khóa học.");

        course.Name = dto.Name;
        course.Level = dto.Level;
        course.Description = dto.Description;
        course.TuitionFee = dto.TuitionFee;
        course.ThumbnailUrl = dto.ThumbnailUrl;
        course.IsActive = dto.IsActive;

        await _courseRepository.UpdateAsync(course);
    }

    public async Task DeleteAsync(int id) =>
        await _courseRepository.DeleteAsync(id);

    private static CourseDto MapToDto(Course course) => new()
    {
        Id = course.Id,
        Name = course.Name,
        Level = course.Level,
        Description = course.Description,
        TuitionFee = course.TuitionFee,
        ThumbnailUrl = course.ThumbnailUrl,
        IsActive = course.IsActive
    };
}
