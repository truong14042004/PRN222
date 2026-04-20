using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IOtpService _otpService;
        private readonly IUserService _userService;

        public ForgotPasswordModel(IOtpService otpService, IUserService userService)
        {
            _otpService = otpService;
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

            var code = await _otpService.GenerateOtpAsync(email);
            TempData["OtpDemo"] = $"OTP của bạn là: {code}";
            TempData["Email"] = email;
            
            return RedirectToPage("/Auth/ResetPassword");
        }
    }
}
