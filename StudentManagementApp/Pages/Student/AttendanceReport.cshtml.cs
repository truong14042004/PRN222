using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student
{
    public class AttendanceReportModel : PageModel
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IClassService _classService;
        private readonly IAttendanceService _attendanceService;

        public AttendanceReportModel(IEnrollmentService enrollmentService, IClassService classService, IAttendanceService attendanceService)
        {
            _enrollmentService = enrollmentService;
            _classService = classService;
            _attendanceService = attendanceService;
        }

        public IEnumerable<EnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<EnrollmentDto>();
        public ClassDto? Class { get; set; }
        public List<DateOnly> AllSessions { get; set; } = new();
        public Dictionary<DateOnly, AttendanceDto> AttByDate { get; set; } = new();
        public DateOnly Today { get; set; }
        public int PresentCount { get; set; }
        public int PastSessions { get; set; }
        public int? SelectedClassId { get; set; }

        public async Task OnGetAsync(int? classId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Student")
            {
                Response.Redirect("/");
                return;
            }

            Enrollments = await _enrollmentService.GetByStudentIdAsync(int.Parse(userId));
            var confirmed = Enrollments.Where(e => e.Status == "Confirmed").ToList();
            SelectedClassId = classId;

            if (classId != null && confirmed.Any(e => e.ClassId == classId))
            {
                Class = await _classService.GetByIdAsync(classId.Value);
                if (Class != null)
                {
                    Today = DateOnly.FromDateTime(DateTime.Today);
                    var sessionDays = Class.Schedules.Select(s => s.DayOfWeek).ToHashSet();

                    for (var d = Class.StartDate; d <= Class.EndDate; d = d.AddDays(1))
                    {
                        int dotnetDow = (int)d.DayOfWeek;
                        int appDow = dotnetDow == 0 ? 8 : dotnetDow + 1;
                        if (sessionDays.Contains(appDow))
                            AllSessions.Add(d);
                    }

                    var attendanceRecords = await _attendanceService.GetByClassAndStudentAsync(classId.Value, int.Parse(userId));
                    AttByDate = attendanceRecords.ToDictionary(a => a.Date);
                    PresentCount = AttByDate.Values.Count(a => a.IsPresent);
                    PastSessions = AllSessions.Count(d => d <= Today);
                }
            }
        }
    }
}
