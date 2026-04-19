using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllWithDetailsAsync();
        return enrollments.Select(MapToDto);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(int studentId)
    {
        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);
        return enrollments.Select(MapToDto);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetByClassIdAsync(int classId)
    {
        var enrollments = await _enrollmentRepository.GetByClassIdAsync(classId);
        return enrollments.Select(MapToDto);
    }

    public async Task EnrollAsync(int studentId, int classId)
    {
        var existing = await _enrollmentRepository.GetByStudentAndClassAsync(studentId, classId);
        if (existing is not null)
            throw new InvalidOperationException("Học viên đã đăng ký lớp này.");

        await _enrollmentRepository.AddAsync(new Enrollment
        {
            StudentId = studentId,
            ClassId = classId,
            Status = "Registered"
        });
    }

    public async Task ConfirmAsync(int enrollmentId)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId)
            ?? throw new InvalidOperationException("Không tìm thấy đăng ký.");

        enrollment.Status = "Confirmed";
        await _enrollmentRepository.UpdateAsync(enrollment);
    }

    public async Task DeleteAsync(int id) =>
        await _enrollmentRepository.DeleteAsync(id);

    private static EnrollmentDto MapToDto(Enrollment e) => new()
    {
        Id = e.Id,
        StudentId = e.StudentId,
        StudentName = e.Student?.FullName ?? string.Empty,
        ClassId = e.ClassId,
        ClassName = e.Class?.ClassName ?? string.Empty,
        CourseName = e.Class?.Course?.Name ?? string.Empty,
        EnrolledAt = e.EnrolledAt,
        Status = e.Status,
        Class = e.Class is null ? null : new ClassDto
        {
            Id = e.Class.Id,
            ClassName = e.Class.ClassName,
            CourseId = e.Class.CourseId,
            CourseName = e.Class.Course?.Name ?? string.Empty,
            TeacherId = e.Class.TeacherId,
            TeacherName = e.Class.Teacher?.FullName,
            StartDate = e.Class.StartDate,
            EndDate = e.Class.EndDate,
            Status = e.Class.Status,
            Schedules = e.Class.Schedules.Select(s => new ClassScheduleDto
            {
                Id = s.Id,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime
            }).ToList()
        }
    };
}
