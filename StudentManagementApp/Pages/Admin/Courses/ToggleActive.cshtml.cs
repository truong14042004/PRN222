using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Courses;

public class ToggleActiveModel : PageModel
{
    private readonly ICourseService _courseService;

    public ToggleActiveModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> OnPostAsync(int id)
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

        await _courseService.UpdateAsync(new UpdateCourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Level = course.Level,
            Description = course.Description,
            TuitionFee = course.TuitionFee,
            ThumbnailUrl = course.ThumbnailUrl,
            IsActive = !course.IsActive
        });

        TempData["Success"] = course.IsActive ? "Đã ẩn khóa học." : "Đã mở lại khóa học.";
        return RedirectToPage("/Admin/Courses/Index");
    }
}
