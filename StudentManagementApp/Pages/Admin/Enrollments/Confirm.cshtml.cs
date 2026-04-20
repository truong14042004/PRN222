using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Enrollments
{
    public class ConfirmModel : PageModel
    {
        private readonly IEnrollmentService _enrollmentService;

        public ConfirmModel(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            await _enrollmentService.ConfirmAsync(id);
            TempData["Success"] = "Đã xác nhận đăng ký.";
            return RedirectToPage("/Admin/Enrollments");
        }
    }
}
