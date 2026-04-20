using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student;

public class ProgressModel : PageModel
{
    private readonly ICourseProgressService _courseProgressService;

    public ProgressModel(ICourseProgressService courseProgressService)
    {
        _courseProgressService = courseProgressService;
    }

    public IReadOnlyList<CourseProgressDto> Progresses { get; private set; } = Array.Empty<CourseProgressDto>();
    public string UserName { get; private set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var userIdRaw = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var userId))
        {
            return RedirectToPage("/Auth/Login");
        }

        if (role != "Student")
        {
            return RedirectToPage("/");
        }

        UserName = HttpContext.Session.GetString("UserName") ?? "Học viên";
        Progresses = (await _courseProgressService.GetByStudentIdAsync(userId))
            .OrderBy(p => p.CourseName)
            .ToList();
        return Page();
    }
}
