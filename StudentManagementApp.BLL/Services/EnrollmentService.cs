using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IEmailService _emailService;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository, IEmailService emailService)
    {
        _enrollmentRepository = enrollmentRepository;
        _emailService = emailService;
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
        var enrollment = await _enrollmentRepository.GetByIdWithDetailsAsync(enrollmentId)
            ?? throw new InvalidOperationException("Không tìm thấy đăng ký.");

        // Generate 6-digit confirmation code
        var confirmationCode = new Random().Next(100000, 999999).ToString();
        enrollment.Status = "Confirmed";
        enrollment.ConfirmationCode = confirmationCode;
        enrollment.ConfirmedAt = DateTime.Now;
        await _enrollmentRepository.UpdateAsync(enrollment);

        // Send confirmation email with schedule
        await SendConfirmationEmailAsync(enrollment, confirmationCode);
    }

    private async Task SendConfirmationEmailAsync(Enrollment enrollment, string code)
    {
        var student = enrollment.Student;
        var classInfo = enrollment.Class;
        if (student is null || classInfo is null || string.IsNullOrWhiteSpace(student.Email))
        {
            return;
        }

        var schedules = (classInfo.Schedules ?? Enumerable.Empty<ClassSchedule>()).ToList();

        var scheduleText = schedules.Any()
            ? string.Join("\n", schedules.Select(s =>
                $"- {GetDayName(s.DayOfWeek)}: {s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}"))
            : "Chưa có lịch học";

        var subject = $"Xác nhận đăng ký lớp {classInfo.ClassName}";
        var body = $@"
 Xin chào {student.FullName},

 Đăng ký của bạn đã được xác nhận!

 Thông tin lớp học:
 - Lớp: {classInfo.ClassName}
 - Khóa học: {classInfo.Course?.Name}
 - Giáo viên: {classInfo.Teacher?.FullName}
 - Ngày bắt đầu: {classInfo.StartDate:dd/MM/yyyy}

 Lịch học:
 {scheduleText}

 Mã xác nhận: {code}

 Vui lòng đến trung tâm đúng giờ và mang theo mã xác nhận này.

 Trân trọng,
 English Center
        ";

        try
        {
            var email = new EmailDto
            {
                To = student.Email,
                Subject = subject,
                Body = body.Replace("\n", "<br>"),
                IsHtml = true
            };
            await _emailService.SendEmailAsync(email);
        }
        catch
        {
            // Log error but don't fail the confirmation
        }
    }

    private static string GetDayName(int day) => day switch
    {
        0 => "Chủ Nhật",
        1 => "Thứ Hai",
        2 => "Thứ Ba",
        3 => "Thứ Tư",
        4 => "Thứ Năm",
        5 => "Thứ Sáu",
        6 => "Thứ Bảy",
        _ => $"Ngày {day}"
    };

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
        ConfirmationCode = e.ConfirmationCode,
        ConfirmedAt = e.ConfirmedAt,
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
