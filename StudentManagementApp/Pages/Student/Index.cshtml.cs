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
        private readonly IClassService _classService;
        private readonly IAttendanceService _attendanceService;
        private readonly IQuizService _quizService;
        private readonly IQuizResultService _quizResultService;

        public IndexModel(
            IEnrollmentService enrollmentService, 
            IUserService userService,
            IClassService classService,
            IAttendanceService attendanceService,
            IQuizService quizService,
            IQuizResultService quizResultService)
        {
            _enrollmentService = enrollmentService;
            _userService = userService;
            _classService = classService;
            _attendanceService = attendanceService;
            _quizService = quizService;
            _quizResultService = quizResultService;
        }

        public string ActiveTab { get; set; } = "overview";
        public IEnumerable<EnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<EnrollmentDto>();
        public decimal WalletBalance { get; set; }

        // Schedule properties
        public List<DaySchedule> WeekDays { get; set; } = new();
        public List<SessionInfo> Sessions { get; set; } = new();
        public DateOnly WeekStart { get; set; }
        public DateOnly WeekEnd { get; set; }
        public DateOnly PreviousWeek { get; set; }
        public DateOnly NextWeek { get; set; }

        // Attendance properties
        public ClassDto? SelectedClass { get; set; }
        public List<DateOnly> AllSessions { get; set; } = new();
        public Dictionary<DateOnly, AttendanceDto> AttByDate { get; set; } = new();
        public int PresentCount { get; set; }
        public int PastSessions { get; set; }
        public int? SelectedClassId { get; set; }

        // Quiz properties
        public IReadOnlyList<QuizDto> Quizzes { get; private set; } = Array.Empty<QuizDto>();
        public IReadOnlyList<QuizResultDto> Results { get; private set; } = Array.Empty<QuizResultDto>();
        public IReadOnlyList<CourseQuizProgressItem> CourseQuizProgresses { get; private set; } = Array.Empty<CourseQuizProgressItem>();

        public async Task OnGetAsync(DateOnly? date, string tab = "overview", int? classId = null)
        {
            ActiveTab = tab;
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

            // 1. Core Info
            Enrollments = await _enrollmentService.GetByStudentIdAsync(userId);
            var user = await _userService.GetByIdAsync(userId);
            WalletBalance = user?.WalletBalance ?? 0;

            // 2. Schedule Logic
            await LoadScheduleAsync(date);

            // 3. Attendance Logic
            if (ActiveTab == "attendance")
            {
                await LoadAttendanceAsync(userId, classId);
            }

            // 4. Quiz Logic
            if (ActiveTab == "quizzes")
            {
                await LoadQuizzesAsync(userId);
            }
        }

        private async Task LoadScheduleAsync(DateOnly? date)
        {
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

        private async Task LoadAttendanceAsync(int userId, int? classId)
        {
            var confirmed = Enrollments.Where(e => e.Status == "Confirmed").ToList();
            SelectedClassId = classId ?? confirmed.FirstOrDefault()?.ClassId;

            if (SelectedClassId != null && confirmed.Any(e => e.ClassId == SelectedClassId))
            {
                SelectedClass = await _classService.GetByIdAsync(SelectedClassId.Value);
                if (SelectedClass != null)
                {
                    var today = DateOnly.FromDateTime(DateTime.Today);
                    var sessionDays = SelectedClass.Schedules.Select(s => s.DayOfWeek).ToHashSet();

                    for (var d = SelectedClass.StartDate; d <= SelectedClass.EndDate; d = d.AddDays(1))
                    {
                        int dotnetDow = (int)d.DayOfWeek;
                        int appDow = dotnetDow == 0 ? 8 : dotnetDow + 1;
                        if (sessionDays.Contains(appDow))
                            AllSessions.Add(d);
                    }

                    var attendanceRecords = await _attendanceService.GetByClassAndStudentAsync(SelectedClassId.Value, userId);
                    AttByDate = attendanceRecords.ToDictionary(a => a.Date);
                    PresentCount = AttByDate.Values.Count(a => a.IsPresent);
                    PastSessions = AllSessions.Count(d => d <= today);
                }
            }
        }

        private async Task LoadQuizzesAsync(int userId)
        {
            var allowedCourseIds = Enrollments
                .Where(e => e.Status == "Confirmed")
                .Select(e => e.Class?.CourseId ?? 0)
                .Where(id => id > 0)
                .ToHashSet();

            if (!allowedCourseIds.Any()) return;

            var allQuizzes = await _quizService.GetAllAsync();
            Quizzes = allQuizzes
                .Where(q => q.IsActive && allowedCourseIds.Contains(q.CourseId))
                .OrderBy(q => q.CourseName)
                .ThenBy(q => q.Title)
                .ToList();

            Results = (await _quizResultService.GetByStudentIdAsync(userId))
                .OrderByDescending(r => r.CompletedAt)
                .ToList();

            var quizzesById = Quizzes.ToDictionary(q => q.Id);
            var completedQuizIds = Results
                .Select(r => r.QuizId)
                .Distinct()
                .Where(id => quizzesById.ContainsKey(id))
                .ToHashSet();

            CourseQuizProgresses = Quizzes
                .GroupBy(q => new { q.CourseId, q.CourseName })
                .Select(g => {
                    var total = g.Count();
                    var completed = g.Count(q => completedQuizIds.Contains(q.Id));
                    return new CourseQuizProgressItem {
                        CourseId = g.Key.CourseId,
                        CourseName = g.Key.CourseName,
                        CompletedQuizCount = completed,
                        TotalQuizCount = total
                    };
                })
                .OrderBy(x => x.CourseName)
                .ToList();
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

    public class CourseQuizProgressItem
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CompletedQuizCount { get; set; }
        public int TotalQuizCount { get; set; }
        public decimal Percentage => TotalQuizCount <= 0
            ? 0
            : Math.Round((decimal)CompletedQuizCount * 100m / TotalQuizCount, 1);
    }
}
