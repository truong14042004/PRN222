using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin;

public class QuizEditModel : PageModel
{
    private readonly IQuizService _quizService;
    private readonly ICourseService _courseService;
    private readonly IClassService _classService;

    public QuizEditModel(IQuizService quizService, ICourseService courseService, IClassService classService)
    {
        _quizService = quizService;
        _courseService = courseService;
        _classService = classService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public List<CourseDto> AvailableCourses { get; private set; } = new();
    public string CurrentRole { get; private set; } = string.Empty;
    public bool IsEdit => Input.Id.HasValue;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        CurrentRole = access.Role;
        AvailableCourses = await GetAllowedCoursesAsync(access);
        if (!AvailableCourses.Any())
        {
            TempData["Error"] = "Không có khóa học phù hợp để quản lý quiz.";
            return RedirectToPage("/Admin/Quizzes");
        }

        if (!id.HasValue)
        {
            Input.TimeLimitMinutes = 30;
            Input.IsActive = true;
            Input.CourseId = AvailableCourses.First().Id;
            return Page();
        }

        var quiz = await _quizService.GetByIdAsync(id.Value);
        if (quiz is null)
        {
            TempData["Error"] = "Không tìm thấy quiz.";
            return RedirectToPage("/Admin/Quizzes");
        }

        if (!CanManageCourse(access, quiz.CourseId))
        {
            TempData["Error"] = "Bạn không có quyền sửa quiz này.";
            return RedirectToPage("/Admin/Quizzes");
        }

        Input = new InputModel
        {
            Id = quiz.Id,
            CourseId = quiz.CourseId,
            Title = quiz.Title,
            Description = quiz.Description,
            TimeLimitMinutes = quiz.TimeLimitMinutes,
            StartAt = quiz.StartAt,
            EndAt = quiz.EndAt,
            IsActive = quiz.IsActive
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var access = await ResolveAccessAsync();
        if (!access.Ok)
        {
            return new EmptyResult();
        }

        CurrentRole = access.Role;
        AvailableCourses = await GetAllowedCoursesAsync(access);
        if (!AvailableCourses.Any())
        {
            ModelState.AddModelError(string.Empty, "Không có khóa học phù hợp để quản lý quiz.");
            return Page();
        }

        if (!CanManageCourse(access, Input.CourseId))
        {
            ModelState.AddModelError(string.Empty, "Bạn không có quyền sử dụng khóa học này.");
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var dto = new CreateQuizDto
            {
                CourseId = Input.CourseId,
                Title = Input.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(Input.Description) ? null : Input.Description.Trim(),
                TimeLimitMinutes = Input.TimeLimitMinutes,
                StartAt = Input.StartAt,
                EndAt = Input.EndAt,
                IsActive = Input.IsActive
            };

            if (Input.Id.HasValue)
            {
                await _quizService.UpdateAsync(Input.Id.Value, dto);
                TempData["Success"] = "Cập nhật quiz thành công.";
            }
            else
            {
                await _quizService.CreateAsync(dto);
                TempData["Success"] = "Tạo quiz thành công.";
            }

            return RedirectToPage("/Admin/Quizzes");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
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

    private async Task<List<CourseDto>> GetAllowedCoursesAsync((bool Ok, string Role, int UserId, HashSet<int> AllowedCourseIds) access)
    {
        var courses = await _courseService.GetAllAsync();
        if (access.Role == "Admin")
        {
            return courses.Where(c => c.IsActive).OrderBy(c => c.Name).ToList();
        }

        return courses
            .Where(c => c.IsActive && access.AllowedCourseIds.Contains(c.Id))
            .OrderBy(c => c.Name)
            .ToList();
    }

    private static bool CanManageCourse((bool Ok, string Role, int UserId, HashSet<int> AllowedCourseIds) access, int courseId)
    {
        if (access.Role == "Admin")
        {
            return true;
        }

        return access.AllowedCourseIds.Contains(courseId);
    }

    public class InputModel
    {
        public int? Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(1, 300)]
        public int TimeLimitMinutes { get; set; } = 30;

        [DataType(DataType.DateTime)]
        public DateTime? StartAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
