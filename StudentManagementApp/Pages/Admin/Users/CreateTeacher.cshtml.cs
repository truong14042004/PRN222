using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Users;

public class CreateTeacherModel : PageModel
{
    private readonly IAuthService _authService;

    public CreateTeacherModel(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string fullName, string email, string username, string? phone, string password)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        try
        {
            var dto = new CreateUserDto
            {
                FullName = fullName,
                Email = email,
                Username = username,
                Phone = phone,
                Password = password,
                Role = "Teacher"
            };

            await _authService.RegisterAsync(dto);
            TempData["Success"] = "Tạo tài khoản giáo viên thành công.";
            return RedirectToPage("/Admin/Users/Index", new { tab = "teachers" });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
