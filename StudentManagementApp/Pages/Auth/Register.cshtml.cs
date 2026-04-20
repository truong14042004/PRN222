using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private const string RegistrationOtpType = "Registration";
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public RegisterModel(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [BindProperty]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string? Phone { get; set; }

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                Response.Redirect("/");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dto = new CreateUserDto
            {
                FullName = FullName.Trim(),
                Email = Email.Trim(),
                Username = Username.Trim(),
                Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
                Password = Password,
                Role = "Student"
            };

            if (!ValidateRegistration(dto))
            {
                return Page();
            }

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu xác nhận không khớp.");
                return Page();
            }

            try
            {
                await _authService.EnsureRegistrationAllowedAsync(dto);
                RegistrationSessionStore.Save(HttpContext.Session, dto);
                await _emailService.GenerateOtpAsync(dto.Email, RegistrationOtpType);
                TempData["Success"] = $"Đã gửi mã OTP tới {dto.Email}. Vui lòng nhập mã để hoàn tất đăng ký.";
                return RedirectToPage("/Auth/VerifyRegistration");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private bool ValidateRegistration(CreateUserDto dto)
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);

            foreach (var result in results)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Dữ liệu đăng ký không hợp lệ.");
            }

            return isValid;
        }
    }
}
