using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student;

public class QuizzesModel : PageModel
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IQuizService _quizService;
    private readonly IQuizResultService _quizResultService;

    public QuizzesModel(
        IEnrollmentService enrollmentService,
        IQuizService quizService,
        IQuizResultService quizResultService)
    {
        _enrollmentService = enrollmentService;
        _quizService = quizService;
        _quizResultService = quizResultService;
    }

    public IReadOnlyList<QuizDto> Quizzes { get; private set; } = Array.Empty<QuizDto>();
    public IReadOnlyList<QuizResultDto> Results { get; private set; } = Array.Empty<QuizResultDto>();
    public IReadOnlyList<CourseQuizProgressItem> CourseQuizProgresses { get; private set; } = Array.Empty<CourseQuizProgressItem>();
    public string UserName { get; private set; } = string.Empty;
    public string FlashError { get; private set; } = string.Empty;

    public async Task OnGetAsync()
    {
        var access = await ResolveStudentAccessAsync();
        if (!access.Ok)
        {
            return;
        }

        UserName = HttpContext.Session.GetString("UserName") ?? "Học viên";
        FlashError = TempData["Error"]?.ToString() ?? string.Empty;

        var enrollments = await _enrollmentService.GetByStudentIdAsync(access.StudentId);
        var allowedCourseIds = enrollments
            .Where(e => e.Status == "Confirmed")
            .Select(e => e.Class?.CourseId ?? 0)
            .Where(id => id > 0)
            .ToHashSet();

        if (!allowedCourseIds.Any())
        {
            Quizzes = Array.Empty<QuizDto>();
            Results = Array.Empty<QuizResultDto>();
            CourseQuizProgresses = Array.Empty<CourseQuizProgressItem>();
            return;
        }

        var allQuizzes = await _quizService.GetAllAsync();
        Quizzes = allQuizzes
            .Where(q => q.IsActive && allowedCourseIds.Contains(q.CourseId))
            .OrderBy(q => q.CourseName)
            .ThenBy(q => q.Title)
            .ToList();

        Results = (await _quizResultService.GetByStudentIdAsync(access.StudentId))
            .OrderByDescending(r => r.CompletedAt)
            .ToList();

        var quizzesById = Quizzes.ToDictionary(q => q.Id);
        var completedQuizIds = Results
            .Select(r => r.QuizId)
            .Distinct()
            .Where(id => quizzesById.ContainsKey(id))
            .ToHashSet();

        CourseQuizProgresses = Quizzes
            .GroupBy(q => new { q.CourseId, q.CourseName })
            .Select(g =>
            {
                var total = g.Count();
                var completed = g.Count(q => completedQuizIds.Contains(q.Id));
                return new CourseQuizProgressItem
                {
                    CourseId = g.Key.CourseId,
                    CourseName = g.Key.CourseName,
                    CompletedQuizCount = completed,
                    TotalQuizCount = total
                };
            })
            .OrderBy(x => x.CourseName)
            .ToList();
    }

    private async Task<(bool Ok, int StudentId)> ResolveStudentAccessAsync()
    {
        await Task.CompletedTask;

        var userIdRaw = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var userId))
        {
            Response.Redirect("/Auth/Login");
            return (false, 0);
        }

        if (role != "Student")
        {
            Response.Redirect("/");
            return (false, 0);
        }

        return (true, userId);
    }

    public class CourseQuizProgressItem
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CompletedQuizCount { get; set; }
        public int TotalQuizCount { get; set; }
        public decimal Percentage => TotalQuizCount <= 0
            ? 0
            : Math.Round((decimal)CompletedQuizCount * 100m / TotalQuizCount, 1);
    }
}
