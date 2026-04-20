using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Courses;

public class EditModel : PageModel
{
    private readonly ICourseService _courseService;

    public EditModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [BindProperty]
    public UpdateCourseDto Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        var course = await _courseService.GetByIdAsync(id);
        if (course is null)
        {
            TempData["Error"] = "Không tìm thấy khóa học.";
            return RedirectToPage("/Admin/Courses/Index");
        }

        Input = new UpdateCourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Level = course.Level,
            Description = course.Description,
            TuitionFee = course.TuitionFee,
            ThumbnailUrl = course.ThumbnailUrl,
            IsActive = course.IsActive
        };

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

        await _courseService.UpdateAsync(Input);
        TempData["Success"] = "Đã cập nhật khóa học.";
        return RedirectToPage("/Admin/Courses/Index");
    }
}
