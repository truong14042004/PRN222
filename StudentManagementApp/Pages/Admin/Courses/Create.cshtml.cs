using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Courses;

public class CreateModel : PageModel
{
    private readonly ICourseService _courseService;

    public CreateModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [BindProperty]
    public CreateCourseDto Input { get; set; } = new();

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _courseService.CreateAsync(Input);
        TempData["Success"] = "Đã tạo khóa học mới.";
        return RedirectToPage("/Admin/Courses/Index");
    }
}
