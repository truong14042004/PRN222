using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Account;

public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public ProfileModel(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    public UserDto? CurrentUser { get; private set; }

    [BindProperty]
    public ProfileInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await LoadCurrentUserAsync();
        if (user is null)
        {
            return RedirectToPage("/Auth/Login");
        }

        CurrentUser = user;
        Input = new ProfileInput
        {
            FullName = user.FullName,
            Phone = user.Phone,
            AvatarUrl = user.AvatarUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPostProfileAsync()
    {
        var user = await LoadCurrentUserAsync();
        if (user is null)
        {
            return RedirectToPage("/Auth/Login");
        }

        if (string.IsNullOrWhiteSpace(Input.FullName))
        {
            TempData["Error"] = "Họ tên không được để trống.";
            return RedirectToPage();
        }

        await _userService.UpdateAsync(new UpdateUserDto
        {
            Id = user.Id,
            FullName = Input.FullName.Trim(),
            Phone = string.IsNullOrWhiteSpace(Input.Phone) ? null : Input.Phone.Trim(),
            AvatarUrl = string.IsNullOrWhiteSpace(Input.AvatarUrl) ? null : Input.AvatarUrl.Trim(),
            IsActive = user.IsActive
        });

        HttpContext.Session.SetString("UserName", Input.FullName.Trim());
        TempData["Success"] = "Đã cập nhật hồ sơ.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostPasswordAsync(string currentPassword, string newPassword, string confirmPassword)
    {
        var user = await LoadCurrentUserAsync();
        if (user is null)
        {
            return RedirectToPage("/Auth/Login");
        }

        if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
        {
            TempData["Error"] = "Vui lòng nhập đầy đủ thông tin đổi mật khẩu.";
            return RedirectToPage();
        }

        if (newPassword.Length < 6)
        {
            TempData["Error"] = "Mật khẩu mới phải có ít nhất 6 ký tự.";
            return RedirectToPage();
        }

        if (newPassword != confirmPassword)
        {
            TempData["Error"] = "Mật khẩu xác nhận không khớp.";
            return RedirectToPage();
        }

        var changed = await _authService.ChangePasswordAsync(user.Id, currentPassword, newPassword);
        if (!changed)
        {
            TempData["Error"] = "Mật khẩu hiện tại không đúng.";
            return RedirectToPage();
        }

        TempData["Success"] = "Đã đổi mật khẩu thành công.";
        return RedirectToPage();
    }

    private async Task<UserDto?> LoadCurrentUserAsync()
    {
        var userIdRaw = HttpContext.Session.GetString("UserId");
        if (!int.TryParse(userIdRaw, out var userId))
        {
            return null;
        }

        return await _userService.GetByIdAsync(userId);
    }

    public class ProfileInput
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
