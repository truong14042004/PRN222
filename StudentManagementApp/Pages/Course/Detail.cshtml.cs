using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Course
{
    public class DetailModel : PageModel
    {
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;
        private readonly IEnrollmentService _enrollmentService;

        public DetailModel(ICourseService courseService, IClassService classService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _classService = classService;
            _enrollmentService = enrollmentService;
        }

        public CourseDto? Course { get; set; }
        public IEnumerable<ClassDto> Classes { get; set; } = Enumerable.Empty<ClassDto>();

        public async Task OnGetAsync(int id)
        {
            Course = await _courseService.GetByIdAsync(id);
            if (Course != null)
            {
                Classes = await _classService.GetByCourseIdAsync(id);
            }
        }

        public async Task<IActionResult> OnPostEnrollAsync(int classId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId is null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để đăng ký khóa học.";
                return RedirectToPage("/Auth/Login");
            }

            var cls = await _classService.GetByIdAsync(classId);
            if (cls is null)
            {
                TempData["Error"] = "Lớp học không tồn tại.";
                return RedirectToPage();
            }

            if (cls.Status != "Upcoming")
            {
                TempData["Error"] = cls.Status == "Ongoing"
                    ? "Lớp này đang học, không thể đăng ký mới."
                    : "Lớp này đã kết thúc, không thể đăng ký.";
                return RedirectToPage(new { id = cls.CourseId });
            }

            try
            {
                await _enrollmentService.EnrollAsync(int.Parse(userId), classId);
                TempData["Success"] = "Đăng ký lớp học thành công! Vui lòng chờ xác nhận từ trung tâm.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage(new { id = cls.CourseId });
        }
    }
}
