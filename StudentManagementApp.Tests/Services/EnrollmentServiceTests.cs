using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;
using Xunit;

namespace StudentManagementApp.Tests.Services;

public class EnrollmentServiceTests
{
    [Fact]
    public async Task ConfirmAsync_updates_enrollment_and_publishes_realtime_notification_to_student()
    {
        var enrollment = new Enrollment
        {
            Id = 7,
            StudentId = 42,
            ClassId = 9,
            Status = "Registered",
            Student = new User
            {
                Id = 42,
                FullName = "Nguyen Van Student",
                Email = "student@example.com",
                Username = "student42",
                PasswordHash = "hashed",
                Role = "Student"
            },
            Class = new Class
            {
                Id = 9,
                ClassName = "IELTS Foundation A1",
                CourseId = 3,
                StartDate = new DateOnly(2026, 5, 10),
                EndDate = new DateOnly(2026, 7, 10),
                Course = new Course
                {
                    Id = 3,
                    Name = "English Basics",
                    Level = "A1",
                    TuitionFee = 1500000
                },
                Teacher = new User
                {
                    Id = 5,
                    FullName = "Tran Teacher",
                    Email = "teacher@example.com",
                    Username = "teacher5",
                    PasswordHash = "hashed",
                    Role = "Teacher"
                },
                Schedules =
                {
                    new ClassSchedule
                    {
                        DayOfWeek = 2,
                        StartTime = new TimeOnly(18, 0),
                        EndTime = new TimeOnly(19, 30)
                    }
                }
            }
        };

        var repository = new FakeEnrollmentRepository(enrollment);
        var emailService = new FakeEmailService();
        var notificationPublisher = new CapturingEnrollmentNotificationPublisher();
        var service = new EnrollmentService(repository, emailService, notificationPublisher);

        await service.ConfirmAsync(enrollment.Id);

        Assert.Equal("Confirmed", enrollment.Status);
        Assert.NotNull(enrollment.ConfirmationCode);
        Assert.NotNull(enrollment.ConfirmedAt);

        var notification = Assert.Single(notificationPublisher.PublishedNotifications);
        Assert.Equal(42, notification.StudentId);
        Assert.Equal(7, notification.EnrollmentId);
        Assert.Equal("IELTS Foundation A1", notification.ClassName);
        Assert.Equal("English Basics", notification.CourseName);
    }

    [Fact]
    public async Task CancelAsync_marks_enrollment_as_cancelled()
    {
        var enrollment = new Enrollment
        {
            Id = 8,
            StudentId = 42,
            ClassId = 9,
            Status = "Registered",
            Student = new User
            {
                Id = 42,
                FullName = "Nguyen Van Student",
                Email = "student@example.com",
                Username = "student42",
                PasswordHash = "hashed",
                Role = "Student"
            },
            Class = new Class
            {
                Id = 9,
                ClassName = "IELTS Foundation A1",
                CourseId = 3,
                StartDate = new DateOnly(2026, 5, 10),
                EndDate = new DateOnly(2026, 7, 10)
            }
        };

        var repository = new FakeEnrollmentRepository(enrollment);
        var service = new EnrollmentService(repository, new FakeEmailService(), new CapturingEnrollmentNotificationPublisher());

        await service.CancelAsync(enrollment.Id);

        Assert.Equal("Cancelled", enrollment.Status);
    }

    private sealed class FakeEnrollmentRepository : IEnrollmentRepository
    {
        private readonly Enrollment _enrollment;

        public FakeEnrollmentRepository(Enrollment enrollment)
        {
            _enrollment = enrollment;
        }

        public Task<IEnumerable<Enrollment>> GetAllAsync() =>
            Task.FromResult(Enumerable.Empty<Enrollment>());

        public Task<Enrollment?> GetByIdAsync(int id) =>
            Task.FromResult(id == _enrollment.Id ? _enrollment : null);

        public Task AddAsync(Enrollment entity) => Task.CompletedTask;

        public Task UpdateAsync(Enrollment entity) => Task.CompletedTask;

        public Task DeleteAsync(int id) => Task.CompletedTask;

        public Task<IEnumerable<Enrollment>> GetAllWithDetailsAsync() =>
            Task.FromResult(Enumerable.Empty<Enrollment>());

        public Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId) =>
            Task.FromResult(_enrollment.StudentId == studentId
                ? Enumerable.Repeat(_enrollment, 1)
                : Enumerable.Empty<Enrollment>());

        public Task<IEnumerable<Enrollment>> GetByClassIdAsync(int classId) =>
            Task.FromResult(_enrollment.ClassId == classId
                ? Enumerable.Repeat(_enrollment, 1)
                : Enumerable.Empty<Enrollment>());

        public Task<Enrollment?> GetByStudentAndClassAsync(int studentId, int classId) =>
            Task.FromResult(_enrollment.StudentId == studentId && _enrollment.ClassId == classId
                ? _enrollment
                : null);

        public Task<Enrollment?> GetByIdWithDetailsAsync(int id) =>
            Task.FromResult(id == _enrollment.Id ? _enrollment : null);
    }

    private sealed class FakeEmailService : IEmailService
    {
        public Task<bool> SendEmailAsync(EmailDto email) => Task.FromResult(true);

        public Task<bool> SendOtpAsync(string email, string otpCode, string type) => Task.FromResult(true);

        public Task<bool> SendRegistrationConfirmationAsync(string fullName, string username, string email) =>
            Task.FromResult(true);

        public Task<bool> SendScheduleNotificationAsync(ScheduleEmailDto schedule) => Task.FromResult(true);

        public Task<string> GenerateOtpAsync(string email, string type) => Task.FromResult("123456");

        public Task<bool> ValidateOtpAsync(string email, string otpCode, string type) => Task.FromResult(true);
    }

    private sealed class CapturingEnrollmentNotificationPublisher : IEnrollmentNotificationPublisher
    {
        public List<EnrollmentConfirmedNotification> PublishedNotifications { get; } = [];

        public Task PublishEnrollmentConfirmedAsync(EnrollmentConfirmedNotification notification)
        {
            PublishedNotifications.Add(notification);
            return Task.CompletedTask;
        }
    }
}
