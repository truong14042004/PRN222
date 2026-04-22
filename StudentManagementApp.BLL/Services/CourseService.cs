using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseNotificationPublisher _notificationPublisher;

    public CourseService(
        ICourseRepository courseRepository,
        ICourseNotificationPublisher notificationPublisher)
    {
        _courseRepository = courseRepository;
        _notificationPublisher = notificationPublisher;
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
        var course = new Course
        {
            Name = dto.Name,
            Level = dto.Level,
            Description = dto.Description,
            TuitionFee = dto.TuitionFee,
            ThumbnailUrl = dto.ThumbnailUrl
        };

        await _courseRepository.AddAsync(course);
        await PublishCourseChangedAsync(
            "Created",
            MapToDto(course),
            $"Khóa học {course.Name} vừa được tạo.");
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
        await PublishCourseChangedAsync(
            "Updated",
            MapToDto(course),
            $"Khóa học {course.Name} vừa được cập nhật.");
    }

    public async Task DeleteAsync(int id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course is null)
        {
            await _courseRepository.DeleteAsync(id);
            return;
        }

        var snapshot = MapToDto(course);
        await _courseRepository.DeleteAsync(id);
        await PublishCourseChangedAsync(
            "Deleted",
            snapshot,
            $"Khóa học {snapshot.Name} vừa được xóa.");
    }

    private async Task PublishCourseChangedAsync(string action, CourseDto course, string message)
    {
        try
        {
            await _notificationPublisher.PublishCourseChangedAsync(new CourseChangedNotification
            {
                Action = action,
                CourseId = course.Id,
                Course = course,
                Title = GetTitle(action),
                Message = message,
                OccurredAt = DateTime.Now
            });
        }
        catch
        {
            // Realtime notification should not break course CRUD.
        }
    }

    private static string GetTitle(string action) => action switch
    {
        "Created" => "Khóa học mới",
        "Updated" => "Khóa học đã cập nhật",
        "Deleted" => "Khóa học đã xóa",
        _ => "Khóa học thay đổi"
    };

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
