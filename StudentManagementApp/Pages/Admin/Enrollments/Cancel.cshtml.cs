using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Enrollments
{
    public class CancelModel : PageModel
    {
        private readonly IEnrollmentService _enrollmentService;

        public CancelModel(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            await _enrollmentService.DeleteAsync(id);
            TempData["Success"] = "Đã hủy đăng ký.";
            return RedirectToPage("/Admin/Enrollments");
        }
    }
}
