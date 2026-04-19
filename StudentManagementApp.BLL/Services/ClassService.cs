using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class ClassService : IClassService
{
    private readonly IClassRepository _classRepository;

    public ClassService(IClassRepository classRepository)
    {
        _classRepository = classRepository;
    }

    public async Task<IEnumerable<ClassDto>> GetAllAsync()
    {
        var classes = await _classRepository.GetWithSchedulesAsync();
        return classes.Select(MapToDto);
    }

    public async Task<ClassDto?> GetByIdAsync(int id)
    {
        var cls = await _classRepository.GetWithDetailsAsync(id);
        return cls is null ? null : MapToDto(cls);
    }

    public async Task<IEnumerable<ClassDto>> GetByTeacherIdAsync(int teacherId)
    {
        var classes = await _classRepository.GetByTeacherIdAsync(teacherId);
        return classes.Select(MapToDto);
    }

    public async Task<IEnumerable<ClassDto>> GetByCourseIdAsync(int courseId)
    {
        var classes = await _classRepository.GetByCourseIdAsync(courseId);
        return classes.Select(MapToDto);
    }

    public async Task CreateAsync(CreateClassDto dto)
    {
        var cls = new Class
        {
            ClassName = dto.ClassName,
            CourseId = dto.CourseId,
            TeacherId = dto.TeacherId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Schedules = dto.Schedules.Select(s => new ClassSchedule
            {
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList()
        };

        await _classRepository.AddAsync(cls);
    }

    public async Task UpdateAsync(UpdateClassDto dto)
    {
        var cls = await _classRepository.GetWithDetailsAsync(dto.Id)
            ?? throw new InvalidOperationException("Không tìm thấy lớp học.");

        cls.ClassName = dto.ClassName;
        cls.TeacherId = dto.TeacherId;
        cls.StartDate = dto.StartDate;
        cls.EndDate = dto.EndDate;
        cls.Status = dto.Status;
        cls.Schedules = dto.Schedules.Select(s => new ClassSchedule
        {
            ClassId = dto.Id,
            DayOfWeek = s.DayOfWeek,
            StartTime = s.StartTime,
            EndTime = s.EndTime
        }).ToList();

        await _classRepository.UpdateAsync(cls);
    }

    public async Task DeleteAsync(int id) =>
        await _classRepository.DeleteAsync(id);

    public async Task<bool> HasScheduleConflictAsync(int teacherId, int dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeClassId = null) =>
        await _classRepository.HasScheduleConflictAsync(teacherId, dayOfWeek, startTime, endTime, excludeClassId);

    private static ClassDto MapToDto(Class cls)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var status = today < cls.StartDate ? "Upcoming"
                   : today <= cls.EndDate  ? "Ongoing"
                   : "Finished";

        return new()
        {
            Id = cls.Id,
            ClassName = cls.ClassName,
            CourseId = cls.CourseId,
            CourseName = cls.Course?.Name ?? string.Empty,
            TeacherId = cls.TeacherId,
            TeacherName = cls.Teacher?.FullName,
            StartDate = cls.StartDate,
            EndDate = cls.EndDate,
            Status = status,
            Schedules = cls.Schedules.Select(s => new ClassScheduleDto
            {
                Id = s.Id,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList()
        };
    }
}
