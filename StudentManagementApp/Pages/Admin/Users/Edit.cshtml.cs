using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Users;

public class EditModel : PageModel
{
    private readonly IUserService _userService;

    public EditModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public EditUserInput Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Tab { get; set; } = "students";

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        var user = await _userService.GetByIdAsync(id);
        if (user is null)
        {
            TempData["Error"] = "Không tìm thấy người dùng.";
            return RedirectToPage("/Admin/Users/Index", new { tab = Tab });
        }

        Input = new EditUserInput
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            IsActive = user.IsActive,
            Role = user.Role,
            Username = user.Username,
            AvatarUrl = user.AvatarUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        var user = await _userService.GetByIdAsync(Input.Id);
        if (user is null)
        {
            TempData["Error"] = "Không tìm thấy người dùng.";
            return RedirectToPage("/Admin/Users/Index", new { tab = Tab });
        }

        if (string.IsNullOrWhiteSpace(Input.FullName))
        {
            ModelState.AddModelError("Input.FullName", "Họ tên không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(Input.Email))
        {
            ModelState.AddModelError("Input.Email", "Email không được để trống.");
        }

        if (!ModelState.IsValid)
        {
            Input.Role = user.Role;
            Input.Username = user.Username;
            return Page();
        }

        try
        {
            await _userService.UpdateAsync(new UpdateUserDto
            {
                Id = Input.Id,
                FullName = Input.FullName.Trim(),
                Phone = string.IsNullOrWhiteSpace(Input.Phone) ? null : Input.Phone.Trim(),
                AvatarUrl = string.IsNullOrWhiteSpace(Input.AvatarUrl) ? null : Input.AvatarUrl.Trim(),
                IsActive = Input.IsActive
            });

            var newEmail = Input.Email.Trim();
            if (!string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
            {
                await _userService.UpdateEmailAsync(Input.Id, newEmail);
            }

            TempData["Success"] = "Đã cập nhật người dùng.";
            return RedirectToPage("/Admin/Users/Index", new { tab = Tab });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            Input.Role = user.Role;
            Input.Username = user.Username;
            return Page();
        }
    }

    public class EditUserInput
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
