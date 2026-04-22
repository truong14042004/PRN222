using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public ForgotPasswordModel(IEmailService emailService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                ViewData["Error"] = "Email không hợp lệ.";
                return Page();
            }

            var all = await _userService.GetAllAsync();
            var exists = all.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (!exists)
            {
                ViewData["Error"] = "Không tìm thấy tài khoản với email này.";
                return Page();
            }

            await _emailService.GenerateOtpAsync(email, "PasswordReset");
            TempData["Email"] = email;
            TempData["Success"] = $"Đã gửi mã OTP tới {email}.";

            return RedirectToPage("/Auth/ResetPassword");
        }
    }
}
