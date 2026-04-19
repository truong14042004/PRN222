using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;
        private readonly IEnrollmentService _enrollmentService;

        private const int PageSize = 15;

        public AdminController(IUserService userService, IAuthService authService,
            ICourseService courseService, IClassService classService, IEnrollmentService enrollmentService)
        {
            _userService = userService;
            _authService = authService;
            _courseService = courseService;
            _classService = classService;
            _enrollmentService = enrollmentService;
        }

        private IActionResult? RequireAdmin()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Auth");
            return null;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            if (RequireAdmin() is { } r) return r;
            var users = await _userService.GetAllAsync();
            var courses = await _courseService.GetAllAsync();
            var classes = await _classService.GetAllAsync();
            ViewBag.StudentCount = users.Count(u => u.Role == "Student");
            ViewBag.TeacherCount = users.Count(u => u.Role == "Teacher");
            ViewBag.CourseCount = courses.Count();
            ViewBag.ClassCount = classes.Count();
            return View();
        }

        // ==================== USERS ====================

        // GET: /Admin/Users?tab=students|teachers&q=...&page=1
        public async Task<IActionResult> Users(string? q, string tab = "students", int page = 1)
        {
            if (RequireAdmin() is { } r) return r;
            var all = await _userService.GetAllAsync();
            var role = tab == "teachers" ? "Teacher" : "Student";
            var users = all.Where(u => u.Role == role);
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                users = users.Where(u =>
                    u.FullName.ToLower().Contains(lower) ||
                    u.Email.ToLower().Contains(lower) ||
                    (u.Phone != null && u.Phone.Contains(lower)));
            }

            var totalCount = users.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));
            var pagedUsers = users.Skip((page - 1) * PageSize).Take(PageSize);

            ViewBag.Q = q;
            ViewBag.Tab = tab;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            return View(pagedUsers);
        }

        // POST: /Admin/ToggleUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUser(int id, string tab = "teachers")
        {
            if (RequireAdmin() is { } r) return r;
            var currentId = int.Parse(HttpContext.Session.GetString("UserId")!);
            if (id == currentId)
            {
                TempData["Error"] = "Không thể vô hiệu hóa tài khoản của chính mình.";
                return RedirectToAction("Users", new { tab });
            }
            var user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("Users", new { tab });
            }
            await _userService.ToggleActiveAsync(id);
            TempData["Success"] = (user.IsActive) ? "Đã vô hiệu hóa tài khoản." : "Đã kích hoạt tài khoản.";
            return RedirectToAction("Users", new { tab });
        }

        // GET: /Admin/CreateTeacher
        public IActionResult CreateTeacher()
        {
            if (RequireAdmin() is { } r) return r;
            return View();
        }

        // POST: /Admin/CreateTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(CreateUserDto dto)
        {
            if (RequireAdmin() is { } r) return r;
            if (!ModelState.IsValid) return View(dto);
            dto.Role = "Teacher";
            try
            {
                await _authService.RegisterAsync(dto);
                TempData["Success"] = "Tạo tài khoản giáo viên thành công.";
                return RedirectToAction("Users", new { tab = "teachers" });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // POST: /Admin/UpdateEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmail(int userId, string newEmail, string tab = "students")
        {
            if (RequireAdmin() is { } r) return r;

            if (string.IsNullOrWhiteSpace(newEmail) || !System.Net.Mail.MailAddress.TryCreate(newEmail, out _))
            {
                TempData["Error"] = "Email không hợp lệ.";
                return RedirectToAction("Users", new { tab });
            }

            var all = await _userService.GetAllAsync();
            if (all.Any(u => u.Id != userId && u.Email.Equals(newEmail, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = "Email này đã được sử dụng bởi tài khoản khác.";
                return RedirectToAction("Users", new { tab });
            }

            await _userService.UpdateEmailAsync(userId, newEmail);
            TempData["Success"] = "Cập nhật email thành công.";
            return RedirectToAction("Users", new { tab });
        }

        // ==================== COURSES ====================

        // GET: /Admin/Courses?q=...&page=1
        public async Task<IActionResult> Courses(string? q, int page = 1)
        {
            if (RequireAdmin() is { } r) return r;
            var courses = await _courseService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                courses = courses.Where(c =>
                    c.Name.ToLower().Contains(lower) ||
                    c.Level.ToLower().Contains(lower));
            }

            var totalCount = courses.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));
            var paged = courses.Skip((page - 1) * PageSize).Take(PageSize);

            ViewBag.Q = q;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            return View(paged);
        }

        // GET: /Admin/CreateCourse
        public IActionResult CreateCourse()
        {
            if (RequireAdmin() is { } r) return r;
            return View();
        }

        // POST: /Admin/CreateCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CreateCourseDto dto, IFormFile? thumbnail)
        {
            if (RequireAdmin() is { } r) return r;
            if (!ModelState.IsValid) return View(dto);
            if (thumbnail != null && thumbnail.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(thumbnail.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/courses", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = new FileStream(path, FileMode.Create);
                await thumbnail.CopyToAsync(stream);
                dto.ThumbnailUrl = $"/images/courses/{fileName}";
            }
            await _courseService.CreateAsync(dto);
            TempData["Success"] = "Tạo khóa học thành công.";
            return RedirectToAction("Courses");
        }

        // GET: /Admin/EditCourse/5
        public async Task<IActionResult> EditCourse(int id)
        {
            if (RequireAdmin() is { } r) return r;
            var course = await _courseService.GetByIdAsync(id);
            if (course is null) return NotFound();
            return View(new UpdateCourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Level = course.Level,
                Description = course.Description,
                TuitionFee = course.TuitionFee,
                ThumbnailUrl = course.ThumbnailUrl,
                IsActive = course.IsActive
            });
        }

        // POST: /Admin/EditCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(UpdateCourseDto dto, IFormFile? thumbnail)
        {
            if (RequireAdmin() is { } r) return r;
            if (!ModelState.IsValid) return View(dto);
            if (thumbnail != null && thumbnail.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(thumbnail.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/courses", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using var stream = new FileStream(path, FileMode.Create);
                await thumbnail.CopyToAsync(stream);
                dto.ThumbnailUrl = $"/images/courses/{fileName}";
            }
            await _courseService.UpdateAsync(dto);
            TempData["Success"] = "Cập nhật khóa học thành công.";
            return RedirectToAction("Courses");
        }

        // ==================== CLASSES ====================

        // GET: /Admin/Classes?q=...&page=1
        public async Task<IActionResult> Classes(string? q, int page = 1)
        {
            if (RequireAdmin() is { } r) return r;
            var classes = await _classService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                classes = classes.Where(c =>
                    c.ClassName.ToLower().Contains(lower) ||
                    c.CourseName.ToLower().Contains(lower) ||
                    (c.TeacherName != null && c.TeacherName.ToLower().Contains(lower)));
            }

            var totalCount = classes.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));
            var paged = classes.Skip((page - 1) * PageSize).Take(PageSize);

            ViewBag.Q = q;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            return View(paged);
        }

        // GET: /Admin/CreateClass
        public async Task<IActionResult> CreateClass()
        {
            if (RequireAdmin() is { } r) return r;
            ViewBag.Courses = await _courseService.GetActiveCoursesAsync();
            ViewBag.Teachers = (await _userService.GetAllAsync()).Where(u => u.Role == "Teacher" && u.IsActive);
            return View();
        }

        // POST: /Admin/CreateClass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(CreateClassDto dto)
        {
            if (RequireAdmin() is { } r) return r;

            if (dto.StartDate < DateOnly.FromDateTime(DateTime.Today))
            {
                TempData["Error"] = "Ngày bắt đầu không được là ngày trong quá khứ.";
                ViewBag.Courses = await _courseService.GetActiveCoursesAsync();
                ViewBag.Teachers = (await _userService.GetAllAsync()).Where(u => u.Role == "Teacher" && u.IsActive);
                return View(dto);
            }

            if (dto.StartDate >= dto.EndDate)
            {
                TempData["Error"] = "Ngày kết thúc phải sau ngày bắt đầu.";
                ViewBag.Courses = await _courseService.GetActiveCoursesAsync();
                ViewBag.Teachers = (await _userService.GetAllAsync()).Where(u => u.Role == "Teacher" && u.IsActive);
                return View(dto);
            }

            if (!dto.Schedules.Any())
            {
                TempData["Error"] = "Vui lòng thêm ít nhất một lịch học.";
                ViewBag.Courses = await _courseService.GetActiveCoursesAsync();
                ViewBag.Teachers = (await _userService.GetAllAsync()).Where(u => u.Role == "Teacher" && u.IsActive);
                return View(dto);
            }

            foreach (var s in dto.Schedules)
            {
                if (s.EndTime <= s.StartTime)
                {
                    TempData["Error"] = "Giờ kết thúc phải sau giờ bắt đầu trong mỗi buổi học.";
                    ViewBag.Courses = await _courseService.GetActiveCoursesAsync();
                    ViewBag.Teachers = (await _userService.GetAllAsync()).Where(u => u.Role == "Teacher" && u.IsActive);
                    return View(dto);
                }
            }

            if (dto.TeacherId.HasValue && dto.Schedules.Any())
            {
                foreach (var s in dto.Schedules)
                {
                    var conflict = await _classService.HasScheduleConflictAsync(dto.TeacherId.Value, s.DayOfWeek, s.StartTime, s.EndTime);
                    if (conflict)
                    {
                        TempData["Error"] = $"Giáo viên đã có lịch dạy vào {s.DayOfWeek} {s.StartTime:hh\\:mm}-{s.EndTime:hh\\:mm}.";
                        ViewBag.Courses = await _courseService.GetActiveCoursesAsync();
                        ViewBag.Teachers = (await _userService.GetAllAsync()).Where(u => u.Role == "Teacher" && u.IsActive);
                        return View(dto);
                    }
                }
            }

            await _classService.CreateAsync(dto);
            TempData["Success"] = "Tạo lớp học thành công.";
            return RedirectToAction("Classes");
        }

        // ==================== ENROLLMENTS ====================

        // GET: /Admin/Enrollments?q=...&pending=true&page=1
        public async Task<IActionResult> Enrollments(string? q, bool pending = false, int page = 1)
        {
            if (RequireAdmin() is { } r) return r;
            var allEnrollments = await _enrollmentService.GetAllAsync();
            var result = allEnrollments.OrderByDescending(e => e.EnrolledAt).AsEnumerable();
            if (pending)
                result = result.Where(e => e.Status == "Registered");
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                result = result.Where(e =>
                    e.StudentName.ToLower().Contains(lower) ||
                    e.ClassName.ToLower().Contains(lower) ||
                    e.CourseName.ToLower().Contains(lower));
            }

            var list = result.ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages));
            var paged = list.Skip((page - 1) * PageSize).Take(PageSize);

            ViewBag.Q = q;
            ViewBag.Pending = pending;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            return View(paged);
        }

        // POST: /Admin/ConfirmEnrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEnrollment(int id)
        {
            if (RequireAdmin() is { } r) return r;
            await _enrollmentService.ConfirmAsync(id);
            TempData["Success"] = "Đã xác nhận đăng ký.";
            return RedirectToAction("Enrollments");
        }

        // POST: /Admin/CancelEnrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrollment(int id)
        {
            if (RequireAdmin() is { } r) return r;
            await _enrollmentService.DeleteAsync(id);
            TempData["Success"] = "Đã hủy đăng ký.";
            return RedirectToAction("Enrollments");
        }
    }
}
