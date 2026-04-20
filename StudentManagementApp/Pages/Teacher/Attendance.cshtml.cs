using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Teacher;

public class AttendanceModel : PageModel
{
    private readonly IClassService _classService;
    private readonly IEnrollmentService _enrollmentService;
    private readonly IAttendanceService _attendanceService;

    public AttendanceModel(
        IClassService classService,
        IEnrollmentService enrollmentService,
        IAttendanceService attendanceService)
    {
        _classService = classService;
        _enrollmentService = enrollmentService;
        _attendanceService = attendanceService;
    }

    [BindProperty]
    public int ClassId { get; set; }

    [BindProperty]
    public DateOnly Date { get; set; }

    [BindProperty]
    public List<RowInput> Rows { get; set; } = new();

    public ClassDto? Class { get; private set; }

    public async Task<IActionResult> OnGetAsync(int classId, DateOnly? date)
    {
        var access = ResolveTeacherAccess();
        if (!access.Ok)
        {
            return access.Result!;
        }

        var cls = await _classService.GetByIdAsync(classId);
        if (cls is null || cls.TeacherId != access.TeacherId)
        {
            return RedirectToPage("/Teacher/Index");
        }

        ClassId = classId;
        Date = date ?? DateOnly.FromDateTime(DateTime.Today);
        Class = cls;

        var enrollments = await _enrollmentService.GetByClassIdAsync(classId);
        var students = enrollments
            .Where(e => e.Status == "Confirmed")
            .OrderBy(e => e.StudentName)
            .ToList();

        var existing = (await _attendanceService.GetByClassAndDateAsync(classId, Date))
            .ToDictionary(a => a.StudentId);

        Rows = students.Select(s => new RowInput
        {
            StudentId = s.StudentId,
            StudentName = s.StudentName,
            IsPresent = existing.TryGetValue(s.StudentId, out var att) && att.IsPresent,
            Note = existing.TryGetValue(s.StudentId, out var oldAtt) ? oldAtt.Note : null
        }).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var access = ResolveTeacherAccess();
        if (!access.Ok)
        {
            return access.Result!;
        }

        var cls = await _classService.GetByIdAsync(ClassId);
        if (cls is null || cls.TeacherId != access.TeacherId)
        {
            return RedirectToPage("/Teacher/Index");
        }

        var dto = new SaveAttendanceDto
        {
            ClassId = ClassId,
            Date = Date,
            Items = Rows.Select(r => new AttendanceItemDto
            {
                StudentId = r.StudentId,
                IsPresent = r.IsPresent,
                Note = string.IsNullOrWhiteSpace(r.Note) ? null : r.Note.Trim()
            }).ToList()
        };

        await _attendanceService.SaveAttendanceAsync(dto);
        TempData["Success"] = "Đã lưu điểm danh.";

        return RedirectToPage("/Teacher/Attendance", new { classId = ClassId, date = Date.ToString("yyyy-MM-dd") });
    }

    private (bool Ok, int TeacherId, IActionResult? Result) ResolveTeacherAccess()
    {
        var userIdRaw = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var teacherId))
        {
            return (false, 0, RedirectToPage("/Auth/Login"));
        }

        if (role != "Teacher")
        {
            return (false, 0, RedirectToPage("/"));
        }

        return (true, teacherId, null);
    }

    public class RowInput
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public bool IsPresent { get; set; }
        public string? Note { get; set; }
    }
}
