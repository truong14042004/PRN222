using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Classes;

public class CreateModel : PageModel
{
    private readonly IClassService _classService;
    private readonly ICourseService _courseService;
    private readonly IUserService _userService;

    public CreateModel(IClassService classService, ICourseService courseService, IUserService userService)
    {
        _classService = classService;
        _courseService = courseService;
        _userService = userService;
    }

    [BindProperty]
    public CreateClassInput Input { get; set; } = CreateClassInput.CreateDefault();

    public IReadOnlyList<CourseDto> Courses { get; private set; } = Array.Empty<CourseDto>();
    public IReadOnlyList<UserDto> Teachers { get; private set; } = Array.Empty<UserDto>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Auth/Login");
        }

        await LoadLookupsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Auth/Login");
        }

        await LoadLookupsAsync();

        ValidateInput();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var schedules = Input.Schedules
            .Where(s => s.DayOfWeek.HasValue && s.StartTime.HasValue && s.EndTime.HasValue)
            .Select(s => new ClassScheduleDto
            {
                DayOfWeek = s.DayOfWeek!.Value,
                StartTime = s.StartTime!.Value,
                EndTime = s.EndTime!.Value
            })
            .ToList();

        if (Input.TeacherId.HasValue)
        {
            foreach (var schedule in schedules)
            {
                var hasConflict = await _classService.HasScheduleConflictAsync(
                    Input.TeacherId.Value,
                    schedule.DayOfWeek,
                    schedule.StartTime,
                    schedule.EndTime);

                if (hasConflict)
                {
                    ModelState.AddModelError(string.Empty, $"Giáo viên bị trùng lịch ở ngày {MapDay(schedule.DayOfWeek)} ({schedule.StartTime:HH\\:mm}-{schedule.EndTime:HH\\:mm}).");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _classService.CreateAsync(new CreateClassDto
        {
            ClassName = Input.ClassName.Trim(),
            CourseId = Input.CourseId,
            TeacherId = Input.TeacherId,
            StartDate = Input.StartDate,
            EndDate = Input.EndDate,
            Schedules = schedules
        });

        TempData["Success"] = "Đã tạo lớp học mới.";
        return RedirectToPage("/Admin/Classes");
    }

    private void ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(Input.ClassName))
        {
            ModelState.AddModelError("Input.ClassName", "Tên lớp học không được để trống.");
        }

        if (Input.CourseId <= 0)
        {
            ModelState.AddModelError("Input.CourseId", "Vui lòng chọn khóa học.");
        }

        if (Input.EndDate < Input.StartDate)
        {
            ModelState.AddModelError("Input.EndDate", "Ngày kết thúc phải sau hoặc bằng ngày bắt đầu.");
        }

        if (Input.Schedules is null || Input.Schedules.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Vui lòng thêm ít nhất 1 lịch học.");
            return;
        }

        var completeSchedules = Input.Schedules
            .Where(s => s.DayOfWeek.HasValue || s.StartTime.HasValue || s.EndTime.HasValue)
            .ToList();

        if (!completeSchedules.Any())
        {
            ModelState.AddModelError(string.Empty, "Vui lòng nhập ít nhất 1 lịch học đầy đủ.");
            return;
        }

        for (var i = 0; i < completeSchedules.Count; i++)
        {
            var schedule = completeSchedules[i];
            if (!schedule.DayOfWeek.HasValue || !schedule.StartTime.HasValue || !schedule.EndTime.HasValue)
            {
                ModelState.AddModelError(string.Empty, $"Dòng lịch học #{i + 1} chưa nhập đủ thông tin.");
                continue;
            }

            if (schedule.StartTime >= schedule.EndTime)
            {
                ModelState.AddModelError(string.Empty, $"Giờ bắt đầu phải nhỏ hơn giờ kết thúc ở dòng #{i + 1}.");
            }
        }
    }

    private async Task LoadLookupsAsync()
    {
        Courses = (await _courseService.GetAllAsync())
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToList();

        Teachers = (await _userService.GetAllAsync())
            .Where(u => u.Role == "Teacher" && u.IsActive)
            .OrderBy(u => u.FullName)
            .ToList();
    }

    private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    private static string MapDay(int day) => day switch
    {
        2 => "Thứ Hai",
        3 => "Thứ Ba",
        4 => "Thứ Tư",
        5 => "Thứ Năm",
        6 => "Thứ Sáu",
        7 => "Thứ Bảy",
        8 => "Chủ Nhật",
        _ => $"Ngày {day}"
    };

    public class CreateClassInput
    {
        public string ClassName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int? TeacherId { get; set; }
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today).AddMonths(2);
        public List<ScheduleInput> Schedules { get; set; } = new();

        public static CreateClassInput CreateDefault() => new()
        {
            Schedules = new List<ScheduleInput>
            {
                new(),
                new()
            }
        };
    }

    public class ScheduleInput
    {
        public int? DayOfWeek { get; set; } = 2;
        public TimeOnly? StartTime { get; set; } = new(18, 0);
        public TimeOnly? EndTime { get; set; } = new(19, 30);
    }
}
