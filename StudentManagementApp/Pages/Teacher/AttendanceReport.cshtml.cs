using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Teacher;

public class AttendanceReportModel : PageModel
{
    private readonly IClassService _classService;
    private readonly IAttendanceService _attendanceService;

    public AttendanceReportModel(IClassService classService, IAttendanceService attendanceService)
    {
        _classService = classService;
        _attendanceService = attendanceService;
    }

    public ClassDto? Class { get; private set; }
    public IReadOnlyList<AttendanceDto> Attendances { get; private set; } = Array.Empty<AttendanceDto>();
    public DateOnly Date { get; private set; }

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

        Class = cls;
        Date = date ?? DateOnly.FromDateTime(DateTime.Today);
        Attendances = (await _attendanceService.GetByClassAndDateAsync(classId, Date))
            .OrderBy(a => a.StudentName)
            .ToList();

        return Page();
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
}
