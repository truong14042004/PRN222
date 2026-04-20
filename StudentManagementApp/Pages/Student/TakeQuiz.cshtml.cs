using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student;

public class TakeQuizModel : PageModel
{
    private const string QuizStartKeyPrefix = "quiz_start_";

    private readonly IEnrollmentService _enrollmentService;
    private readonly IQuizService _quizService;
    private readonly IQuizResultService _quizResultService;

    public TakeQuizModel(
        IEnrollmentService enrollmentService,
        IQuizService quizService,
        IQuizResultService quizResultService)
    {
        _enrollmentService = enrollmentService;
        _quizService = quizService;
        _quizResultService = quizResultService;
    }

    [BindProperty]
    public int QuizId { get; set; }

    [BindProperty]
    public List<AnswerInput> Answers { get; set; } = new();

    public QuizViewModel? Quiz { get; private set; }
    public string Error { get; private set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int quizId)
    {
        var access = await ResolveStudentAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        QuizId = quizId;
        var loaded = await LoadQuizForStudentAsync(quizId, access.StudentId);
        if (!loaded.Ok || loaded.Quiz is null)
        {
            TempData["Error"] = loaded.Error;
            return RedirectToPage("/Student/Quizzes");
        }

        if (!loaded.Quiz.Questions.Any())
        {
            TempData["Error"] = "Bài quiz chưa có câu hỏi, vui lòng thử lại sau.";
            return RedirectToPage("/Student/Quizzes");
        }

        Quiz = loaded.Quiz;
        Answers = Quiz.Questions
            .OrderBy(q => q.SortOrder)
            .Select(q => new AnswerInput { QuestionId = q.Id })
            .ToList();

        HttpContext.Session.SetString(GetQuizStartKey(quizId), DateTime.UtcNow.Ticks.ToString());
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var access = await ResolveStudentAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        var loaded = await LoadQuizForStudentAsync(QuizId, access.StudentId);
        if (!loaded.Ok || loaded.Quiz is null)
        {
            TempData["Error"] = loaded.Error;
            return RedirectToPage("/Student/Quizzes");
        }

        Quiz = loaded.Quiz;

        var answerDtos = new List<SubmitAnswerDto>();
        foreach (var input in Answers)
        {
            var question = Quiz.Questions.FirstOrDefault(q => q.Id == input.QuestionId);
            if (question is null)
            {
                continue;
            }

            answerDtos.Add(new SubmitAnswerDto
            {
                QuestionId = input.QuestionId,
                SelectedOptionId = question.QuestionType == "MultipleChoice" ? input.SelectedOptionId : null,
                FillInAnswer = question.QuestionType == "FillInBlank" ? input.FillInAnswer : null
            });
        }

        var elapsed = CalculateElapsed(QuizId);
        try
        {
            var result = await _quizResultService.SubmitQuizAsync(access.StudentId, QuizId, answerDtos, elapsed);
            HttpContext.Session.Remove(GetQuizStartKey(QuizId));
            return RedirectToPage("/Student/QuizResult", new { id = result.Id });
        }
        catch (InvalidOperationException ex)
        {
            Error = ex.Message;
            return Page();
        }
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

    private async Task<(bool Ok, QuizViewModel? Quiz, string Error)> LoadQuizForStudentAsync(int quizId, int studentId)
    {
        var quiz = await _quizService.GetDetailAsync(quizId);
        if (quiz is null)
        {
            return (false, null, "Không tìm thấy bài quiz.");
        }

        if (!quiz.IsActive)
        {
            return (false, null, "Bài quiz đang tạm khóa.");
        }

        var now = DateTime.Now;
        if (quiz.StartAt.HasValue && now < quiz.StartAt.Value)
        {
            return (false, null, $"Quiz chưa mở. Bắt đầu lúc {quiz.StartAt.Value:dd/MM/yyyy HH:mm}.");
        }

        if (quiz.EndAt.HasValue && now > quiz.EndAt.Value)
        {
            return (false, null, $"Quiz đã đóng lúc {quiz.EndAt.Value:dd/MM/yyyy HH:mm}.");
        }

        var enrollments = await _enrollmentService.GetByStudentIdAsync(studentId);
        var allowedCourseIds = enrollments
            .Where(e => e.Status == "Confirmed")
            .Select(e => e.Class?.CourseId ?? 0)
            .Where(id => id > 0)
            .ToHashSet();

        if (!allowedCourseIds.Contains(quiz.CourseId))
        {
            return (false, null, "Bạn không có quyền làm bài quiz này.");
        }

        var vm = new QuizViewModel
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            CourseName = quiz.CourseName,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            StartAt = quiz.StartAt,
            EndAt = quiz.EndAt,
            Questions = quiz.Questions
                .OrderBy(q => q.SortOrder)
                .Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    SortOrder = q.SortOrder,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Point = q.Point,
                    Options = q.Options
                        .OrderBy(o => o.SortOrder)
                        .Select(o => new OptionViewModel
                        {
                            Id = o.Id,
                            OptionText = o.OptionText
                        })
                        .ToList()
                })
                .ToList()
        };

        return (true, vm, string.Empty);
    }

    private TimeSpan CalculateElapsed(int quizId)
    {
        var startRaw = HttpContext.Session.GetString(GetQuizStartKey(quizId));
        if (string.IsNullOrWhiteSpace(startRaw) || !long.TryParse(startRaw, out var startTicks))
        {
            return TimeSpan.Zero;
        }

        var start = new DateTime(startTicks, DateTimeKind.Utc);
        var elapsed = DateTime.UtcNow - start;
        if (elapsed < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        return elapsed;
    }

    private static string GetQuizStartKey(int quizId) => $"{QuizStartKeyPrefix}{quizId}";

    public class AnswerInput
    {
        public int QuestionId { get; set; }
        public int? SelectedOptionId { get; set; }
        public string? FillInAnswer { get; set; }
    }

    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int TimeLimitMinutes { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public int Point { get; set; }
        public List<OptionViewModel> Options { get; set; } = new();
    }

    public class OptionViewModel
    {
        public int Id { get; set; }
        public string OptionText { get; set; } = string.Empty;
    }
}
