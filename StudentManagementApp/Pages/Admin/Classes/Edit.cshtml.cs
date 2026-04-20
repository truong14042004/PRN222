using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Classes;

public class EditModel : PageModel
{
    private readonly IClassService _classService;
    private readonly IUserService _userService;

    public EditModel(IClassService classService, IUserService userService)
    {
        _classService = classService;
        _userService = userService;
    }

    [BindProperty]
    public EditClassInput Input { get; set; } = new();

    public IReadOnlyList<UserDto> Teachers { get; private set; } = Array.Empty<UserDto>();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Auth/Login");
        }

        var cls = await _classService.GetByIdAsync(id);
        if (cls is null)
        {
            TempData["Error"] = "Không tìm thấy lớp học.";
            return RedirectToPage("/Admin/Classes");
        }

        await LoadTeachersAsync();
        Input = new EditClassInput
        {
            Id = cls.Id,
            ClassName = cls.ClassName,
            CourseName = cls.CourseName,
            TeacherId = cls.TeacherId,
            StartDate = cls.StartDate,
            EndDate = cls.EndDate,
            Schedules = cls.Schedules
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .Select(s => new ScheduleInput
                {
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .ToList()
        };

        if (!Input.Schedules.Any())
        {
            Input.Schedules.Add(new ScheduleInput());
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsAdmin())
        {
            return RedirectToPage("/Auth/Login");
        }

        await LoadTeachersAsync();

        ValidateInput();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var original = await _classService.GetByIdAsync(Input.Id);
        if (original is null)
        {
            TempData["Error"] = "Không tìm thấy lớp học.";
            return RedirectToPage("/Admin/Classes");
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
                    schedule.EndTime,
                    Input.Id);

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

        await _classService.UpdateAsync(new UpdateClassDto
        {
            Id = Input.Id,
            ClassName = Input.ClassName.Trim(),
            TeacherId = Input.TeacherId,
            StartDate = Input.StartDate,
            EndDate = Input.EndDate,
            Status = original.Status,
            Schedules = schedules
        });

        TempData["Success"] = "Đã cập nhật lớp học.";
        return RedirectToPage("/Admin/Classes");
    }

    private bool IsAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    private async Task LoadTeachersAsync()
    {
        Teachers = (await _userService.GetAllAsync())
            .Where(u => u.Role == "Teacher" && u.IsActive)
            .OrderBy(u => u.FullName)
            .ToList();
    }

    private void ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(Input.ClassName))
        {
            ModelState.AddModelError("Input.ClassName", "Tên lớp học không được để trống.");
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
            var s = completeSchedules[i];
            if (!s.DayOfWeek.HasValue || !s.StartTime.HasValue || !s.EndTime.HasValue)
            {
                ModelState.AddModelError(string.Empty, $"Dòng lịch học #{i + 1} chưa nhập đủ thông tin.");
                continue;
            }

            if (s.StartTime >= s.EndTime)
            {
                ModelState.AddModelError(string.Empty, $"Giờ bắt đầu phải nhỏ hơn giờ kết thúc ở dòng #{i + 1}.");
            }
        }
    }

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

    public class EditClassInput
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int? TeacherId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<ScheduleInput> Schedules { get; set; } = new();
    }

    public class ScheduleInput
    {
        public int? DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}
