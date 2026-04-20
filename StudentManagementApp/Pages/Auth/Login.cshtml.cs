using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                Response.Redirect("/");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Username = Username.Trim();

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return Page();
            }

            var dto = new LoginDto { Username = Username, Password = Password };
            var user = await _authService.LoginAsync(dto);

            if (user is null)
            {
                ViewData["Error"] = "Tên đăng nhập/email hoặc mật khẩu không đúng.";
                return Page();
            }

            if (!user.IsActive)
            {
                ViewData["Error"] = "Tài khoản của bạn đã bị vô hiệu hóa. Vui lòng liên hệ trung tâm.";
                return Page();
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);

            return user.Role switch
            {
                "Admin" => RedirectToPage("/Admin/Index"),
                "Teacher" => RedirectToPage("/Teacher/Index"),
                _ => RedirectToPage("/Student/Index")
            };
        }
    }
}
