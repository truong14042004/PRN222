using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Auth
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        public ResetPasswordModel(IEmailService emailService, IAuthService authService)
        {
            _emailService = emailService;
            _authService = authService;
        }

        public void OnGet()
        {
            if (TempData["Email"] != null)
            {
                ViewData["Email"] = TempData["Email"];
            }
        }

        public async Task<IActionResult> OnPostAsync(string Email, string OtpCode, string NewPassword, string ConfirmPassword)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(OtpCode))
            {
                ViewData["Email"] = Email;
                ModelState.AddModelError(string.Empty, "Vui lòng điền đầy đủ thông tin.");
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ViewData["Email"] = Email;
                ModelState.AddModelError(string.Empty, "Mật khẩu xác nhận không khớp.");
                return Page();
            }

            var valid = await _emailService.ValidateOtpAsync(Email, OtpCode, "PasswordReset");
            if (!valid)
            {
                ViewData["Email"] = Email;
                ModelState.AddModelError(string.Empty, "OTP không hợp lệ hoặc đã hết hạn.");
                return Page();
            }

            var dto = new ResetPasswordDto
            {
                Email = Email,
                OtpCode = OtpCode,
                NewPassword = NewPassword
            };

            await _authService.ResetPasswordAsync(dto);
            TempData["Success"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
            return RedirectToPage("/Auth/Login");
        }
    }
}
