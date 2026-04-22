using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student
{
    public class IndexModel : PageModel
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IUserService _userService;

        public IndexModel(IEnrollmentService enrollmentService, IUserService userService)
        {
            _enrollmentService = enrollmentService;
            _userService = userService;
        }

        public IEnumerable<EnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<EnrollmentDto>();
        public decimal WalletBalance { get; set; }

        // Schedule properties
        public List<DaySchedule> WeekDays { get; set; } = new();
        public List<SessionInfo> Sessions { get; set; } = new();
        public DateOnly WeekStart { get; set; }
        public DateOnly WeekEnd { get; set; }
        public DateOnly PreviousWeek { get; set; }
        public DateOnly NextWeek { get; set; }

        public async Task OnGetAsync(DateOnly? date)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (userIdStr == null)
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            var userId = int.Parse(userIdStr);
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Student")
            {
                Response.Redirect("/");
                return;
            }

            Enrollments = await _enrollmentService.GetByStudentIdAsync(userId);
            var user = await _userService.GetByIdAsync(userId);
            WalletBalance = user?.WalletBalance ?? 0;

            // Schedule logic
            var selectedDate = date ?? DateOnly.FromDateTime(DateTime.Today);
            int dow = (int)selectedDate.DayOfWeek;
            int daysToMonday = dow == 0 ? -6 : 1 - dow;
            WeekStart = selectedDate.AddDays(daysToMonday);
            WeekEnd = WeekStart.AddDays(6);
            PreviousWeek = WeekStart.AddDays(-7);
            NextWeek = WeekStart.AddDays(7);

            var dayNames = new[] { "Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy" };
            for (int i = 0; i < 7; i++)
            {
                var d = WeekStart.AddDays(i);
                WeekDays.Add(new DaySchedule
                {
                    Date = d,
                    DayOfWeek = d.DayOfWeek == System.DayOfWeek.Sunday ? 8 : (int)d.DayOfWeek + 1,
                    DayName = dayNames[(int)d.DayOfWeek]
                });
            }

            var activeEnrollments = Enrollments
                .Where(e => e.Status == "Confirmed" && e.Class != null && e.Class.StartDate <= WeekEnd && e.Class.EndDate >= WeekStart)
                .ToList();

            foreach (var e in activeEnrollments)
            {
                if (e.Class?.Schedules != null)
                {
                    foreach (var schedule in e.Class.Schedules)
                    {
                        Sessions.Add(new SessionInfo
                        {
                            DayOfWeek = schedule.DayOfWeek,
                            StartTime = schedule.StartTime.ToString(@"hh\:mm"),
                            EndTime = schedule.EndTime.ToString(@"hh\:mm"),
                            ClassName = e.ClassName,
                            CourseName = e.CourseName
                        });
                    }
                }
            }
        }
    }

    public class DaySchedule
    {
        public DateOnly Date { get; set; }
        public int DayOfWeek { get; set; }
        public string DayName { get; set; } = string.Empty;
    }

    public class SessionInfo
    {
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
    }
}
