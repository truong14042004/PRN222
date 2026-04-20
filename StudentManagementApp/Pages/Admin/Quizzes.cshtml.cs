using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin;

public class QuizzesModel : PageModel
{
    private readonly IQuizService _quizService;
    private readonly IClassService _classService;

    public QuizzesModel(IQuizService quizService, IClassService classService)
    {
        _quizService = quizService;
        _classService = classService;
    }

    public string CurrentRole { get; private set; } = string.Empty;
    public IEnumerable<QuizDto> Quizzes { get; private set; } = Enumerable.Empty<QuizDto>();

    public async Task OnGetAsync()
    {
        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return;
        }

        CurrentRole = access.Role;
        var quizzes = await _quizService.GetAllAsync();
        if (access.Role == "Admin")
        {
            Quizzes = quizzes.OrderBy(q => q.CourseName).ThenBy(q => q.Title);
            return;
        }

        Quizzes = quizzes
            .Where(q => access.AllowedCourseIds.Contains(q.CourseId))
            .OrderBy(q => q.CourseName)
            .ThenBy(q => q.Title);
    }

    private async Task<(bool Ok, string Role, HashSet<int> AllowedCourseIds)> ResolveAccessAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");
        var userIdRaw = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var userId))
        {
            Response.Redirect("/Auth/Login");
            return (false, string.Empty, new HashSet<int>());
        }

        if (role == "Admin")
        {
            return (true, role, new HashSet<int>());
        }

        if (role != "Teacher")
        {
            Response.Redirect("/");
            return (false, string.Empty, new HashSet<int>());
        }

        var classes = await _classService.GetByTeacherIdAsync(userId);
        var allowedCourseIds = classes.Select(c => c.CourseId).ToHashSet();
        return (true, role, allowedCourseIds);
    }
}
