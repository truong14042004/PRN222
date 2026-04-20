using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin;

public class QuizResultsModel : PageModel
{
    private readonly IQuizService _quizService;
    private readonly IQuizResultService _quizResultService;
    private readonly IClassService _classService;

    public QuizResultsModel(IQuizService quizService, IQuizResultService quizResultService, IClassService classService)
    {
        _quizService = quizService;
        _quizResultService = quizResultService;
        _classService = classService;
    }

    [BindProperty(SupportsGet = true)]
    public int? QuizId { get; set; }

    public string CurrentRole { get; private set; } = string.Empty;
    public IReadOnlyList<QuizDto> AvailableQuizzes { get; private set; } = Array.Empty<QuizDto>();
    public IReadOnlyList<QuizResultDto> Results { get; private set; } = Array.Empty<QuizResultDto>();
    public QuizDto? SelectedQuiz { get; private set; }

    public decimal AveragePercent { get; private set; }
    public int PassedCount { get; private set; }
    public int TotalAttempts { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        CurrentRole = access.Role;
        var quizzes = await _quizService.GetAllAsync();
        AvailableQuizzes = access.Role == "Admin"
            ? quizzes.OrderBy(q => q.CourseName).ThenBy(q => q.Title).ToList()
            : quizzes.Where(q => access.AllowedCourseIds.Contains(q.CourseId))
                .OrderBy(q => q.CourseName)
                .ThenBy(q => q.Title)
                .ToList();

        if (!QuizId.HasValue)
        {
            return Page();
        }

        SelectedQuiz = AvailableQuizzes.FirstOrDefault(q => q.Id == QuizId.Value);
        if (SelectedQuiz is null)
        {
            TempData["Error"] = "Bạn không có quyền xem kết quả quiz này.";
            return RedirectToPage("/Admin/QuizResults");
        }

        Results = (await _quizResultService.GetByQuizIdAsync(QuizId.Value))
            .OrderByDescending(r => r.CompletedAt)
            .ToList();

        TotalAttempts = Results.Count;
        PassedCount = Results.Count(r => r.Passed);
        if (TotalAttempts > 0)
        {
            AveragePercent = Results.Average(r =>
                r.TotalPoints > 0 ? (r.Score / r.TotalPoints) * 100m : 0m);
        }

        return Page();
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
