using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Auth;

public class VerifyRegistrationModel : PageModel
{
    private const string RegistrationOtpType = "Registration";
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;

    public VerifyRegistrationModel(IAuthService authService, IEmailService emailService)
    {
        _authService = authService;
        _emailService = emailService;
    }

    [BindProperty]
    public string OtpCode { get; set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public IActionResult OnGet()
    {
        var registration = LoadPendingRegistration();
        if (registration is null)
        {
            TempData["Error"] = "Phiên đăng ký đã hết hạn. Vui lòng nhập lại thông tin để nhận OTP mới.";
            return RedirectToPage("/Auth/Register");
        }

        PopulatePage(registration);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var registration = LoadPendingRegistration();
        if (registration is null)
        {
            TempData["Error"] = "Phiên đăng ký đã hết hạn. Vui lòng nhập lại thông tin để nhận OTP mới.";
            return RedirectToPage("/Auth/Register");
        }

        PopulatePage(registration);

        if (string.IsNullOrWhiteSpace(OtpCode) || OtpCode.Length != 6 || !OtpCode.All(char.IsDigit))
        {
            ModelState.AddModelError(string.Empty, "Mã OTP phải gồm đúng 6 chữ số.");
            return Page();
        }

        try
        {
            await _authService.EnsureRegistrationAllowedAsync(registration);

            var isValid = await _emailService.ValidateOtpAsync(registration.Email, OtpCode, RegistrationOtpType);
            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, "Mã OTP không hợp lệ hoặc đã hết hạn.");
                return Page();
            }

            await _authService.RegisterAsync(registration);
            RegistrationSessionStore.Clear(HttpContext.Session);

            var confirmationSent = await _emailService.SendRegistrationConfirmationAsync(
                registration.FullName,
                registration.Username,
                registration.Email);

            TempData["Success"] = confirmationSent
                ? "Đăng ký thành công. Vui lòng đăng nhập, email xác nhận đã được gửi."
                : "Đăng ký thành công. Vui lòng đăng nhập, nhưng hệ thống chưa gửi được email xác nhận.";

            return RedirectToPage("/Auth/Login");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostResendAsync()
    {
        var registration = LoadPendingRegistration();
        if (registration is null)
        {
            TempData["Error"] = "Phiên đăng ký đã hết hạn. Vui lòng nhập lại thông tin để nhận OTP mới.";
            return RedirectToPage("/Auth/Register");
        }

        try
        {
            await _authService.EnsureRegistrationAllowedAsync(registration);
            await _emailService.GenerateOtpAsync(registration.Email, RegistrationOtpType);
            TempData["Success"] = $"Đã gửi lại mã OTP tới {registration.Email}.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToPage();
    }

    private CreateUserDto? LoadPendingRegistration()
    {
        return RegistrationSessionStore.Get(HttpContext.Session);
    }

    private void PopulatePage(CreateUserDto registration)
    {
        Email = registration.Email;
        FullName = registration.FullName;
    }
}
