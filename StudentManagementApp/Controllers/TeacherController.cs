using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IClassService _classService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IAttendanceService _attendanceService;

        public TeacherController(IClassService classService, IEnrollmentService enrollmentService, IAttendanceService attendanceService)
        {
            _classService = classService;
            _enrollmentService = enrollmentService;
            _attendanceService = attendanceService;
        }

        private IActionResult? RequireTeacher()
        {
            if (HttpContext.Session.GetString("UserRole") != "Teacher")
                return RedirectToAction("Login", "Auth");
            return null;
        }

        private int GetTeacherId() => int.Parse(HttpContext.Session.GetString("UserId")!);

        // GET: /Teacher
        public async Task<IActionResult> Index()
        {
            if (RequireTeacher() is { } r) return r;
            var classes = await _classService.GetByTeacherIdAsync(GetTeacherId());
            return View(classes);
        }

        // GET: /Teacher/Schedule?date=2026-04-21
        public async Task<IActionResult> Schedule(DateOnly? date)
        {
            if (RequireTeacher() is { } r) return r;
            var selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);

            // Start of week (Monday)
            int dow = (int)selectedDate.DayOfWeek;
            int daysToMonday = dow == 0 ? -6 : 1 - dow;
            var weekStart = selectedDate.AddDays(daysToMonday);
            var weekEnd = weekStart.AddDays(6);

            var classes = await _classService.GetByTeacherIdAsync(GetTeacherId());
            // Only show classes that are not finished by the selected week
            var active = classes.Where(c => c.StartDate <= weekEnd && c.EndDate >= weekStart).ToList();

            ViewBag.WeekStart = weekStart;
            ViewBag.WeekEnd = weekEnd;
            ViewBag.SelectedDate = selectedDate;
            return View(active);
        }

        // GET: /Teacher/Attendance/5
        public async Task<IActionResult> Attendance(int classId)
        {
            if (RequireTeacher() is { } r) return r;
            var cls = await _classService.GetByIdAsync(classId);
            if (cls is null) return NotFound();
            if (cls.TeacherId != GetTeacherId()) return Forbid();

            var today = DateOnly.FromDateTime(DateTime.Today);

            // Block future dates — only allow today
            int dotnetDow = (int)DateTime.Today.DayOfWeek;
            int appDow = dotnetDow == 0 ? 8 : dotnetDow + 1;
            bool hasSession = cls.Schedules.Any(s => s.DayOfWeek == appDow);

            var enrollments = await _enrollmentService.GetByClassIdAsync(classId);
            var confirmed = enrollments.Where(e => e.Status == "Confirmed").ToList();

            var existing = hasSession
                ? await _attendanceService.GetByClassAndDateAsync(classId, today)
                : Enumerable.Empty<BLL.DTOs.AttendanceDto>();

            ViewBag.Class = cls;
            ViewBag.Date = today;
            ViewBag.HasSession = hasSession;
            ViewBag.Existing = existing.ToDictionary(a => a.StudentId);
            return View(confirmed);
        }

        // POST: /Teacher/SaveAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttendance(SaveAttendanceDto dto)
        {
            if (RequireTeacher() is { } r) return r;
            var cls = await _classService.GetByIdAsync(dto.ClassId);
            if (cls is null || cls.TeacherId != GetTeacherId()) return Forbid();

            // Only allow saving on a scheduled day
            int dotnetDow = (int)DateTime.Today.DayOfWeek;
            int appDow = dotnetDow == 0 ? 8 : dotnetDow + 1;
            if (!cls.Schedules.Any(s => s.DayOfWeek == appDow))
            {
                TempData["Error"] = "Hôm nay không có buổi học của lớp này.";
                return RedirectToAction("Attendance", new { classId = dto.ClassId });
            }

            await _attendanceService.SaveAttendanceAsync(dto);
            TempData["Success"] = "Điểm danh đã được lưu.";
            return RedirectToAction("Attendance", new { classId = dto.ClassId });
        }

        // GET: /Teacher/AttendanceReport/5
        public async Task<IActionResult> AttendanceReport(int classId)
        {
            if (RequireTeacher() is { } r) return r;
            var cls = await _classService.GetByIdAsync(classId);
            if (cls is null) return NotFound();
            if (cls.TeacherId != GetTeacherId()) return Forbid();

            var enrollments = await _enrollmentService.GetByClassIdAsync(classId);
            var confirmed = enrollments.Where(e => e.Status == "Confirmed").ToList();

            // Get all attendance records for this class
            var allAttendance = new List<BLL.DTOs.AttendanceDto>();
            foreach (var e in confirmed)
            {
                var records = await _attendanceService.GetByStudentIdAsync(e.StudentId);
                allAttendance.AddRange(records.Where(a => a.ClassId == classId));
            }

            // Group by date
            var byDate = allAttendance
                .GroupBy(a => a.Date)
                .OrderByDescending(g => g.Key)
                .ToList();

            ViewBag.Class = cls;
            ViewBag.Students = confirmed;
            ViewBag.ByDate = byDate;
            return View();
        }
    }
}
