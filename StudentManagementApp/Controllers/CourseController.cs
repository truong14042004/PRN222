using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;
        private readonly IEnrollmentService _enrollmentService;

        public CourseController(ICourseService courseService, IClassService classService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _classService = classService;
            _enrollmentService = enrollmentService;
        }

        // GET: /Course
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            return View(courses);
        }

        // GET: /Course/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course is null) return NotFound();

            var classes = await _classService.GetByCourseIdAsync(id);
            ViewBag.Classes = classes;
            return View(course);
        }

        // POST: /Course/Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int classId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId is null)
            {
                TempData["LoginError"] = "Vui lòng đăng nhập để đăng ký khóa học.";
                return RedirectToAction("Login", "Auth");
            }

            var cls = await _classService.GetByIdAsync(classId);
            if (cls is null)
            {
                TempData["Error"] = "Lớp học không tồn tại.";
                return RedirectToAction("Index");
            }

            if (cls.Status != "Upcoming")
            {
                TempData["Error"] = cls.Status == "Ongoing"
                    ? "Lớp này đang học, không thể đăng ký mới."
                    : "Lớp này đã kết thúc, không thể đăng ký.";
                return RedirectToAction("Detail", new { id = cls.CourseId });
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

            return RedirectToAction("Detail", new { id = cls.CourseId });
        }
    }
}
