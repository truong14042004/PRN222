using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student
{
    public class IndexModel : PageModel
    {
        private readonly IEnrollmentService _enrollmentService;

        public IndexModel(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public IEnumerable<EnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<EnrollmentDto>();

        public async Task OnGetAsync()
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
        }
    }
}
