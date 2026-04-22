using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.Pages.Course;

public class DetailModel : PageModel
{
    private readonly ICourseService _courseService;
    private readonly IClassService _classService;
    private readonly IEnrollmentService _enrollmentService;
    private readonly ICartService _cartService;

    public DetailModel(
        ICourseService courseService,
        IClassService classService,
        IEnrollmentService enrollmentService,
        ICartService cartService)
    {
        _courseService = courseService;
        _classService = classService;
        _enrollmentService = enrollmentService;
        _cartService = cartService;
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

    public async Task<IActionResult> OnPostAddToCartAsync(int classId)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId is null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để thanh toán khóa học.";
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
                ? "Lớp này đang học, không thể thanh toán đăng ký mới."
                : "Lớp này đã kết thúc, không thể thanh toán đăng ký.";
            return RedirectToPage(new { id = cls.CourseId });
        }

        var parsedUserId = int.Parse(userId);
        try
        {
            var existingEnrollments = await _enrollmentService.GetByStudentIdAsync(parsedUserId);
            if (existingEnrollments.Any(e => e.ClassId == classId && e.Status != "Cancelled"))
            {
                TempData["Error"] = "Bạn đã đăng ký lớp này rồi.";
                return RedirectToPage(new { id = cls.CourseId });
            }

            await _cartService.AddToCartAsync(parsedUserId, OrderItemType.Course, classId);
            TempData["Success"] = "Đã thêm lớp học vào giỏ hàng. Hoàn tất thanh toán để ghi nhận đăng ký.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToPage(new { id = cls.CourseId });
    }
}
