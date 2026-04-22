using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Teacher
{
    public class ScheduleModel : PageModel
    {
        private readonly IClassService _classService;

        public ScheduleModel(IClassService classService)
        {
            _classService = classService;
        }

        public List<TeacherDaySchedule> WeekDays { get; set; } = new();
        public List<TeacherSessionInfo> Sessions { get; set; } = new();
        public DateOnly WeekStart { get; set; }
        public DateOnly WeekEnd { get; set; }
        public DateOnly PreviousWeek { get; set; }
        public DateOnly NextWeek { get; set; }
        public IEnumerable<ClassDto> Classes { get; set; } = Enumerable.Empty<ClassDto>();

        public async Task OnGetAsync(DateOnly? date)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Teacher")
            {
                Response.Redirect("/");
                return;
            }

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
                WeekDays.Add(new TeacherDaySchedule
                {
                    Date = d,
                    DayOfWeek = d.DayOfWeek == System.DayOfWeek.Sunday ? 8 : (int)d.DayOfWeek + 1,
                    DayName = dayNames[(int)d.DayOfWeek]
                });
            }

            Classes = await _classService.GetByTeacherIdAsync(int.Parse(userId));
            var activeClasses = Classes.Where(c => c.StartDate <= WeekEnd && c.EndDate >= WeekStart).ToList();

            foreach (var cls in activeClasses)
            {
                if (cls.Schedules != null)
                {
                    foreach (var schedule in cls.Schedules)
                    {
                        Sessions.Add(new TeacherSessionInfo
                        {
                            DayOfWeek = schedule.DayOfWeek,
                            StartTime = schedule.StartTime.ToString(@"hh\:mm"),
                            EndTime = schedule.EndTime.ToString(@"hh\:mm"),
                            ClassId = cls.Id,
                            ClassName = cls.ClassName,
                            CourseName = cls.CourseName
                        });
                    }
                }
            }
        }
    }

    public class TeacherDaySchedule
    {
        public DateOnly Date { get; set; }
        public int DayOfWeek { get; set; }
        public string DayName { get; set; } = string.Empty;
    }

    public class TeacherSessionInfo
    {
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
    }
}
