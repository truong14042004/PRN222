using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin;

public class QuizQuestionsModel : PageModel
{
    private readonly IQuizService _quizService;
    private readonly IQuizQuestionService _quizQuestionService;
    private readonly IClassService _classService;

    public QuizQuestionsModel(IQuizService quizService, IQuizQuestionService quizQuestionService, IClassService classService)
    {
        _quizService = quizService;
        _quizQuestionService = quizQuestionService;
        _classService = classService;
    }

    [BindProperty]
    public QuestionInputModel Input { get; set; } = new();

    public QuizDetailDto? Quiz { get; private set; }
    public string CurrentRole { get; private set; } = string.Empty;
    public int QuizId { get; private set; }

    public async Task<IActionResult> OnGetAsync(int quizId, int? editId)
    {
        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        if (!await LoadQuizForAccessAsync(quizId, access))
        {
            return RedirectToPage("/Admin/Quizzes");
        }

        CurrentRole = access.Role;
        QuizId = quizId;
        PrepareInputForCreate();

        if (!editId.HasValue || Quiz is null)
        {
            return Page();
        }

        var question = Quiz.Questions.FirstOrDefault(q => q.Id == editId.Value);
        if (question is null)
        {
            TempData["Error"] = "Không tìm thấy câu hỏi.";
            return RedirectToPage("/Admin/QuizQuestions", new { quizId });
        }

        PopulateInputForEdit(question);
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync(int quizId)
    {
        if (quizId <= 0)
        {
            TempData["Error"] = "Thiếu quizId, không thể lưu câu hỏi.";
            return RedirectToPage("/Admin/Quizzes");
        }

        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        if (!await LoadQuizForAccessAsync(quizId, access))
        {
            return RedirectToPage("/Admin/Quizzes");
        }

        CurrentRole = access.Role;
        QuizId = quizId;

        if (Input.QuestionId.HasValue && (Quiz is null || !Quiz.Questions.Any(q => q.Id == Input.QuestionId.Value)))
        {
            TempData["Error"] = "Câu hỏi không thuộc quiz này.";
            return RedirectToPage("/Admin/QuizQuestions", new { quizId });
        }

        var dto = BuildQuestionDto();
        if (dto is null || !ModelState.IsValid)
        {
            var firstError = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault(msg => !string.IsNullOrWhiteSpace(msg));

            TempData["Error"] = firstError ?? "Dữ liệu không hợp lệ, vui lòng kiểm tra lại.";
            return RedirectToPage("/Admin/QuizQuestions", new { quizId });
        }

        try
        {
            if (Input.QuestionId.HasValue)
            {
                await _quizQuestionService.UpdateAsync(Input.QuestionId.Value, dto);
                TempData["Success"] = "Cập nhật câu hỏi thành công.";
            }
            else
            {
                await _quizQuestionService.CreateAsync(dto);
                TempData["Success"] = "Tạo câu hỏi thành công.";
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToPage("/Admin/QuizQuestions", new { quizId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToPage("/Admin/QuizQuestions", new { quizId });
        }

        return RedirectToPage("/Admin/QuizQuestions", new { quizId });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int quizId, int questionId)
    {
        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        if (!await LoadQuizForAccessAsync(quizId, access))
        {
            return RedirectToPage("/Admin/Quizzes");
        }

        if (Quiz is null || !Quiz.Questions.Any(q => q.Id == questionId))
        {
            TempData["Error"] = "Câu hỏi không thuộc quiz này.";
            return RedirectToPage("/Admin/QuizQuestions", new { quizId });
        }

        try
        {
            await _quizQuestionService.DeleteAsync(questionId);
            TempData["Success"] = "Đã xóa câu hỏi.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToPage("/Admin/QuizQuestions", new { quizId });
    }

    private CreateQuestionDto? BuildQuestionDto()
    {
        var type = (Input.QuestionType ?? string.Empty).Trim();
        if (type != "MultipleChoice" && type != "FillInBlank")
        {
            ModelState.AddModelError(string.Empty, "Loại câu hỏi không hợp lệ.");
            return null;
        }

        if (Input.Point <= 0)
        {
            ModelState.AddModelError(string.Empty, "Điểm phải lớn hơn 0.");
            return null;
        }

        var options = new List<CreateOptionDto>();
        if (type == "FillInBlank")
        {
            var correctAnswer = (Input.FillInAnswer ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(correctAnswer))
            {
                ModelState.AddModelError(string.Empty, "Câu điền đáp án phải có đáp án đúng.");
                return null;
            }

            options.Add(new CreateOptionDto
            {
                OptionText = correctAnswer,
                IsCorrect = true,
                SortOrder = 1
            });
        }
        else
        {
            var rawOptions = new[]
            {
                (Text: (Input.OptionA ?? string.Empty).Trim(), Slot: 1),
                (Text: (Input.OptionB ?? string.Empty).Trim(), Slot: 2),
                (Text: (Input.OptionC ?? string.Empty).Trim(), Slot: 3),
                (Text: (Input.OptionD ?? string.Empty).Trim(), Slot: 4)
            };

            var filledOptions = rawOptions.Where(o => !string.IsNullOrWhiteSpace(o.Text)).ToList();
            if (filledOptions.Count < 2)
            {
                ModelState.AddModelError(string.Empty, "Câu trắc nghiệm cần ít nhất 2 lựa chọn.");
                return null;
            }

            if (Input.CorrectOptionSlot < 1 || Input.CorrectOptionSlot > 4)
            {
                ModelState.AddModelError(string.Empty, "Vị trí đáp án đúng không hợp lệ.");
                return null;
            }

            var chosen = rawOptions.FirstOrDefault(o => o.Slot == Input.CorrectOptionSlot);
            if (string.IsNullOrWhiteSpace(chosen.Text))
            {
                ModelState.AddModelError(string.Empty, "Đáp án đúng không được để trống.");
                return null;
            }

            options = filledOptions
                .Select((o, index) => new CreateOptionDto
                {
                    OptionText = o.Text,
                    IsCorrect = o.Slot == Input.CorrectOptionSlot,
                    SortOrder = index + 1
                })
                .ToList();
        }

        var normalizedQuestionText = (Input.QuestionText ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalizedQuestionText))
        {
            ModelState.AddModelError(string.Empty, "Nội dung câu hỏi là bắt buộc.");
            return null;
        }

        var sortOrder = Input.SortOrder > 0
            ? Input.SortOrder
            : GetNextSortOrder();

        return new CreateQuestionDto
        {
            QuizId = QuizId,
            QuestionText = normalizedQuestionText,
            QuestionType = type,
            Point = Input.Point,
            SortOrder = sortOrder,
            Options = options
        };
    }

    private void PopulateInputForEdit(QuizQuestionDto question)
    {
        Input.QuestionId = question.Id;
        Input.QuestionText = question.QuestionText;
        Input.QuestionType = question.QuestionType;
        Input.Point = question.Point;
        Input.SortOrder = question.SortOrder;

        if (question.QuestionType == "FillInBlank")
        {
            Input.FillInAnswer = question.Options.FirstOrDefault(o => o.IsCorrect)?.OptionText ?? string.Empty;
            return;
        }

        var ordered = question.Options.OrderBy(o => o.SortOrder).ToList();
        Input.OptionA = ordered.ElementAtOrDefault(0)?.OptionText ?? string.Empty;
        Input.OptionB = ordered.ElementAtOrDefault(1)?.OptionText ?? string.Empty;
        Input.OptionC = ordered.ElementAtOrDefault(2)?.OptionText ?? string.Empty;
        Input.OptionD = ordered.ElementAtOrDefault(3)?.OptionText ?? string.Empty;

        var correctIndex = ordered.FindIndex(o => o.IsCorrect);
        Input.CorrectOptionSlot = correctIndex >= 0 ? correctIndex + 1 : 1;
    }

    private void PrepareInputForCreate()
    {
        Input = new QuestionInputModel
        {
            QuestionType = "MultipleChoice",
            Point = 10,
            SortOrder = GetNextSortOrder(),
            CorrectOptionSlot = 1
        };
    }

    private int GetNextSortOrder()
    {
        if (Quiz?.Questions is null || !Quiz.Questions.Any())
        {
            return 1;
        }

        return Quiz.Questions.Max(q => q.SortOrder) + 1;
    }

    private async Task<bool> LoadQuizForAccessAsync(int quizId, (bool Ok, string Role, int UserId, HashSet<int> AllowedCourseIds) access)
    {
        var quiz = await _quizService.GetDetailAsync(quizId);
        if (quiz is null)
        {
            TempData["Error"] = "Không tìm thấy quiz.";
            return false;
        }

        if (access.Role != "Admin" && !access.AllowedCourseIds.Contains(quiz.CourseId))
        {
            TempData["Error"] = "Bạn không có quyền quản lý quiz này.";
            return false;
        }

        Quiz = quiz;
        return true;
    }

    private async Task<(bool Ok, string Role, int UserId, HashSet<int> AllowedCourseIds)> ResolveAccessAsync()
    {
        var role = HttpContext.Session.GetString("UserRole");
        var userIdRaw = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var userId))
        {
            Response.Redirect("/Auth/Login");
            return (false, string.Empty, 0, new HashSet<int>());
        }

        if (role == "Admin")
        {
            return (true, role, userId, new HashSet<int>());
        }

        if (role != "Teacher")
        {
            Response.Redirect("/");
            return (false, string.Empty, 0, new HashSet<int>());
        }

        var classes = await _classService.GetByTeacherIdAsync(userId);
        var allowedCourseIds = classes.Select(c => c.CourseId).ToHashSet();
        return (true, role, userId, allowedCourseIds);
    }

    public class QuestionInputModel
    {
        public int? QuestionId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string QuestionType { get; set; } = "MultipleChoice";

        [Range(1, 1000)]
        public int Point { get; set; } = 10;

        [Range(0, 1000)]
        public int SortOrder { get; set; } = 0;

        [MaxLength(500)]
        public string? OptionA { get; set; }

        [MaxLength(500)]
        public string? OptionB { get; set; }

        [MaxLength(500)]
        public string? OptionC { get; set; }

        [MaxLength(500)]
        public string? OptionD { get; set; }

        [Range(1, 4)]
        public int CorrectOptionSlot { get; set; } = 1;

        [MaxLength(500)]
        public string? FillInAnswer { get; set; }
    }
}
