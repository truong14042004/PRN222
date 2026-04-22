using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.Pages.Course;
using StudentManagementApp.Tests.TestSupport;
using Xunit;

namespace StudentManagementApp.Tests.Pages.Course;

public class DetailModelTests
{
    [Fact]
    public async Task OnPostAddToCartAsync_adds_upcoming_class_to_cart_for_logged_in_student()
    {
        var cartService = new CapturingCartService();
        var classService = new FakeClassService
        {
            Class = new ClassDto
            {
                Id = 9,
                CourseId = 3,
                CourseName = "IELTS",
                ClassName = "IELTS-01",
                Status = "Upcoming",
                StartDate = new DateOnly(2026, 5, 1),
                EndDate = new DateOnly(2026, 8, 1)
            }
        };
        var model = new DetailModel(new StubCourseService(), classService, new FakeEnrollmentService(), cartService);
        var (_, session, tempData) = model.AttachHttpContext();
        session.SetString("UserId", "42");

        var result = await model.OnPostAddToCartAsync(9);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Null(redirect.PageName);
        Assert.Equal(3, redirect.RouteValues?["id"]);
        Assert.Equal(42, cartService.AddedUserId);
        Assert.Equal(OrderItemType.Course, cartService.AddedItemType);
        Assert.Equal(9, cartService.AddedItemId);
        Assert.Equal("Đã thêm lớp học vào giỏ hàng. Hoàn tất thanh toán để ghi nhận đăng ký.", tempData["Success"]);
    }

    private sealed class CapturingCartService : ICartService
    {
        public int AddedUserId { get; private set; }
        public OrderItemType AddedItemType { get; private set; }
        public int AddedItemId { get; private set; }

        public Task<CartDto> GetCartAsync(int userId) => Task.FromResult(new CartDto());

        public Task AddToCartAsync(int userId, OrderItemType itemType, int itemId)
        {
            AddedUserId = userId;
            AddedItemType = itemType;
            AddedItemId = itemId;
            return Task.CompletedTask;
        }

        public Task RemoveFromCartAsync(int userId, OrderItemType itemType, int itemId) => Task.CompletedTask;
        public Task ClearCartAsync(int userId) => Task.CompletedTask;
    }

    private sealed class FakeClassService : IClassService
    {
        public ClassDto? Class { get; set; }

        public Task<IEnumerable<ClassDto>> GetAllAsync() => Task.FromResult(Enumerable.Empty<ClassDto>());
        public Task<ClassDto?> GetByIdAsync(int id) => Task.FromResult(Class?.Id == id ? Class : null);
        public Task<IEnumerable<ClassDto>> GetByTeacherIdAsync(int teacherId) => Task.FromResult(Enumerable.Empty<ClassDto>());
        public Task<IEnumerable<ClassDto>> GetByCourseIdAsync(int courseId) => Task.FromResult(Enumerable.Empty<ClassDto>());
        public Task CreateAsync(CreateClassDto dto) => Task.CompletedTask;
        public Task UpdateAsync(UpdateClassDto dto) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;
        public Task<bool> HasScheduleConflictAsync(int teacherId, int dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeClassId = null) => Task.FromResult(false);
    }

    private sealed class FakeEnrollmentService : IEnrollmentService
    {
        public Task<IEnumerable<EnrollmentDto>> GetAllAsync() => Task.FromResult(Enumerable.Empty<EnrollmentDto>());
        public Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(int studentId) => Task.FromResult(Enumerable.Empty<EnrollmentDto>());
        public Task<IEnumerable<EnrollmentDto>> GetByClassIdAsync(int classId) => Task.FromResult(Enumerable.Empty<EnrollmentDto>());
        public Task EnrollAsync(int studentId, int classId) => Task.CompletedTask;
        public Task ConfirmAsync(int enrollmentId) => Task.CompletedTask;
        public Task CancelAsync(int enrollmentId) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;
    }

    private sealed class StubCourseService : ICourseService
    {
        public Task<IEnumerable<CourseDto>> GetAllAsync() => Task.FromResult(Enumerable.Empty<CourseDto>());
        public Task<IEnumerable<CourseDto>> GetActiveCoursesAsync() => Task.FromResult(Enumerable.Empty<CourseDto>());
        public Task<CourseDto?> GetByIdAsync(int id) => Task.FromResult<CourseDto?>(null);
        public Task CreateAsync(CreateCourseDto dto) => Task.CompletedTask;
        public Task UpdateAsync(UpdateCourseDto dto) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;
    }
}
