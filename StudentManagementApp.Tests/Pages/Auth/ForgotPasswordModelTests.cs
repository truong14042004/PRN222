using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.Pages.Auth;
using StudentManagementApp.Tests.TestSupport;
using Xunit;

namespace StudentManagementApp.Tests.Pages.Auth;

public class ForgotPasswordModelTests
{
    [Fact]
    public async Task OnPostAsync_generates_email_otp_and_redirects_to_reset_password()
    {
        var emailService = new CapturingEmailService();
        var userService = new FakeUserService();
        var model = new ForgotPasswordModel(emailService, userService);
        var (_, _, tempData) = model.AttachHttpContext();

        var result = await model.OnPostAsync("student@example.com");

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Auth/ResetPassword", redirect.PageName);
        Assert.Equal("student@example.com", tempData["Email"]);
        Assert.Equal("student@example.com", emailService.GeneratedEmail);
        Assert.Equal("PasswordReset", emailService.GeneratedType);
        Assert.False(tempData.ContainsKey("OtpDemo"));
    }

    private sealed class CapturingEmailService : IEmailService
    {
        public string? GeneratedEmail { get; private set; }
        public string? GeneratedType { get; private set; }

        public Task<bool> SendEmailAsync(EmailDto email) => Task.FromResult(true);
        public Task<bool> SendOtpAsync(string email, string otpCode, string type) => Task.FromResult(true);
        public Task<bool> SendRegistrationConfirmationAsync(string fullName, string username, string email) => Task.FromResult(true);
        public Task<bool> SendScheduleNotificationAsync(ScheduleEmailDto schedule) => Task.FromResult(true);

        public Task<string> GenerateOtpAsync(string email, string type)
        {
            GeneratedEmail = email;
            GeneratedType = type;
            return Task.FromResult("123456");
        }

        public Task<bool> ValidateOtpAsync(string email, string otpCode, string type) => Task.FromResult(true);
    }

    private sealed class FakeUserService : IUserService
    {
        public Task<IEnumerable<UserDto>> GetAllAsync() =>
            Task.FromResult(Enumerable.Repeat(new UserDto
            {
                Id = 42,
                Email = "student@example.com",
                Username = "student01",
                FullName = "Student"
            }, 1).AsEnumerable());

        public Task<UserDto?> GetByIdAsync(int id) => Task.FromResult<UserDto?>(null);
        public Task UpdateAsync(UpdateUserDto dto) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;
        public Task UpdateEmailAsync(int userId, string newEmail) => Task.CompletedTask;
        public Task SetActiveAsync(int userId, bool isActive) => Task.CompletedTask;
        public Task ToggleActiveAsync(int userId) => Task.CompletedTask;
    }
}
