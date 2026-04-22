using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizAssistantController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IQuizService _quizService;
    private readonly IQuizHintService _quizHintService;

    public QuizAssistantController(
        IEnrollmentService enrollmentService,
        IQuizService quizService,
        IQuizHintService quizHintService)
    {
        _enrollmentService = enrollmentService;
        _quizService = quizService;
        _quizHintService = quizHintService;
    }

    [HttpPost("hint")]
    public async Task<IActionResult> GetHint([FromBody] QuizHintRequestDto request, CancellationToken cancellationToken)
    {
        var userIdRaw = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var studentId))
        {
            return Unauthorized(new { message = "Vui long dang nhap de su dung tro ly." });
        }

        if (!string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
        {
            return Forbid();
        }

        var quiz = await _quizService.GetDetailAsync(request.QuizId);
        if (quiz is null || !quiz.IsActive)
        {
            return BadRequest(new { message = "Quiz khong hop le hoac da dong." });
        }

        var now = DateTime.Now;
        if (quiz.StartAt.HasValue && now < quiz.StartAt.Value)
        {
            return BadRequest(new { message = "Quiz chua mo." });
        }
        if (quiz.EndAt.HasValue && now > quiz.EndAt.Value)
        {
            return BadRequest(new { message = "Quiz da dong." });
        }

        var enrollments = await _enrollmentService.GetByStudentIdAsync(studentId);
        var allowedCourseIds = enrollments
            .Where(e => e.Status == "Confirmed")
            .Select(e => e.Class?.CourseId ?? 0)
            .Where(id => id > 0)
            .ToHashSet();

        if (!allowedCourseIds.Contains(quiz.CourseId))
        {
            return Forbid();
        }

        var question = quiz.Questions.FirstOrDefault(q => q.Id == request.QuestionId);
        if (question is null)
        {
            return BadRequest(new { message = "Khong tim thay cau hoi." });
        }

        var context = new QuizHintContextDto
        {
            QuizTitle = quiz.Title,
            CourseName = quiz.CourseName,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            SelectedOptionId = request.SelectedOptionId,
            FillInAnswer = request.FillInAnswer,
            StudentMessage = request.Message ?? string.Empty,
            History = (request.History ?? new List<QuizChatMessageDto>())
                .Where(h => !string.IsNullOrWhiteSpace(h.Content))
                .Select(h => new QuizChatMessageDto
                {
                    Role = NormalizeRole(h.Role),
                    Content = h.Content.Trim()
                })
                .TakeLast(12)
                .ToList(),
            Options = question.Options
                .OrderBy(o => o.SortOrder)
                .Select(o => new QuizOptionHintDto
                {
                    Id = o.Id,
                    Text = o.OptionText,
                    IsCorrect = o.IsCorrect
                })
                .ToList()
        };

        var hint = await _quizHintService.GenerateHintAsync(context, cancellationToken);
        return Ok(hint);
    }

    private static string NormalizeRole(string? role)
    {
        return string.Equals(role, "assistant", StringComparison.OrdinalIgnoreCase) ? "assistant" : "user";
    }
}
