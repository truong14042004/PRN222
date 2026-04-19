using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUserService _userService;
        private readonly IAttendanceService _attendanceService;
        private readonly IClassService _classService;

        public StudentController(IEnrollmentService enrollmentService, IUserService userService,
            IAttendanceService attendanceService, IClassService classService)
        {
            _enrollmentService = enrollmentService;
            _userService = userService;
            _attendanceService = attendanceService;
            _classService = classService;
        }

        private int? GetStudentId()
        {
            var id = HttpContext.Session.GetString("UserId");
            return id is null ? null : int.Parse(id);
        }

        private IActionResult RedirectIfNotStudent()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Student") return RedirectToAction("Login", "Auth");
            return null!;
        }

        // GET: /Student
        public async Task<IActionResult> Index()
        {
            var redirect = RedirectIfNotStudent();
            if (redirect != null) return redirect;

            var studentId = GetStudentId()!.Value;
            var enrollments = await _enrollmentService.GetByStudentIdAsync(studentId);
            return View(enrollments);
        }

        // GET: /Student/Schedule?date=2026-04-21
        public async Task<IActionResult> Schedule(DateOnly? date)
        {
            var redirect = RedirectIfNotStudent();
            if (redirect != null) return redirect;

            var selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);

            int dow = (int)selectedDate.DayOfWeek;
            int daysToMonday = dow == 0 ? -6 : 1 - dow;
            var weekStart = selectedDate.AddDays(daysToMonday);
            var weekEnd = weekStart.AddDays(6);

            var studentId = GetStudentId()!.Value;
            var enrollments = await _enrollmentService.GetByStudentIdAsync(studentId);
            // Confirmed + not finished by this week
            var active = enrollments
                .Where(e => e.Status == "Confirmed" && e.Class != null && e.Class.StartDate <= weekEnd && e.Class.EndDate >= weekStart)
                .ToList();

            ViewBag.WeekStart = weekStart;
            ViewBag.WeekEnd = weekEnd;
            ViewBag.SelectedDate = selectedDate;
            return View(active);
        }

        // GET: /Student/AttendanceReport?classId=5
        public async Task<IActionResult> AttendanceReport(int? classId)
        {
            var redirect = RedirectIfNotStudent();
            if (redirect != null) return redirect;

            var studentId = GetStudentId()!.Value;
            var enrollments = await _enrollmentService.GetByStudentIdAsync(studentId);
            var confirmed = enrollments.Where(e => e.Status == "Confirmed").ToList();

            ViewBag.Enrollments = confirmed;
            ViewBag.SelectedClassId = classId;

            if (classId == null || !confirmed.Any(e => e.ClassId == classId))
                return View();

            var cls = await _classService.GetByIdAsync(classId.Value);
            if (cls == null) return View();

            // Generate all session dates from StartDate to EndDate based on schedules
            var today = DateOnly.FromDateTime(DateTime.Today);
            var sessionDays = cls.Schedules.Select(s => s.DayOfWeek).ToHashSet(); // app convention: 2=Mon...8=Sun

            var allSessions = new List<DateOnly>();
            for (var d = cls.StartDate; d <= cls.EndDate; d = d.AddDays(1))
            {
                int dotnetDow = (int)d.DayOfWeek; // 0=Sun,1=Mon,...6=Sat
                int appDow = dotnetDow == 0 ? 8 : dotnetDow + 1;
                if (sessionDays.Contains(appDow))
                    allSessions.Add(d);
            }

            // Get actual attendance records for this student in this class
            var attendanceRecords = await _attendanceService.GetByClassAndStudentAsync(classId.Value, studentId);
            var attByDate = attendanceRecords.ToDictionary(a => a.Date);

            int presentCount = attByDate.Values.Count(a => a.IsPresent);
            int pastSessions = allSessions.Count(d => d <= today);

            ViewBag.Class = cls;
            ViewBag.AllSessions = allSessions;
            ViewBag.AttByDate = attByDate;
            ViewBag.Today = today;
            ViewBag.PresentCount = presentCount;
            ViewBag.PastSessions = pastSessions;

            return View();
        }
    }
}

