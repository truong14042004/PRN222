using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Courses;

public class DeleteModel : PageModel
{
    private readonly ICourseService _courseService;

    public DeleteModel(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        try
        {
            await _courseService.DeleteAsync(id);
            TempData["Success"] = "Đã xóa khóa học.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToPage("/Admin/Courses/Index");
    }
}
