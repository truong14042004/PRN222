using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.Pages.Auth;
using StudentManagementApp.Tests.TestSupport;
using Xunit;

namespace StudentManagementApp.Tests.Pages.Auth;

public class LoginModelTests
{
    [Theory]
    [InlineData("Student", "/Student/Index")]
    [InlineData("Teacher", "/Teacher/Index")]
    [InlineData("Admin", "/Admin/Index")]
    public void OnGet_redirects_logged_in_user_to_role_dashboard(string role, string expectedLocation)
    {
        var model = new LoginModel(new StubAuthService());
        var (httpContext, session, _) = model.AttachHttpContext();
        session.SetString("UserId", "42");
        session.SetString("UserRole", role);

        model.OnGet();

        Assert.Equal(StatusCodes.Status302Found, httpContext.Response.StatusCode);
        Assert.Equal(expectedLocation, httpContext.Response.Headers.Location.ToString());
    }

    [Fact]
    public async Task OnPostAsync_sets_session_and_redirects_student_after_successful_login()
    {
        var authService = new StubAuthService
        {
            LoginResult = new UserDto
            {
                Id = 42,
                FullName = "Student User",
                Role = "Student",
                IsActive = true
            }
        };
        var model = new LoginModel(authService)
        {
            Username = "student01",
            Password = "CorrectPass123"
        };
        var (_, session, _) = model.AttachHttpContext();

        var result = await model.OnPostAsync();

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Student/Index", redirect.PageName);
        Assert.Equal("42", session.GetString("UserId"));
        Assert.Equal("Student User", session.GetString("UserName"));
        Assert.Equal("Student", session.GetString("UserRole"));
    }

    private sealed class StubAuthService : IAuthService
    {
        public UserDto? LoginResult { get; set; }

        public Task<UserDto?> LoginAsync(LoginDto dto) => Task.FromResult(LoginResult);

        public Task EnsureRegistrationAllowedAsync(CreateUserDto dto) => Task.CompletedTask;

        public Task<UserDto> RegisterAsync(CreateUserDto dto) =>
            Task.FromResult(new UserDto());

        public Task<bool> ResetPasswordAsync(ResetPasswordDto dto) => Task.FromResult(true);

        public Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword) =>
            Task.FromResult(true);
    }
}
