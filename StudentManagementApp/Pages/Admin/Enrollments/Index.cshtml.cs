using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly IEnrollmentService _enrollmentService;

        public IndexModel(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        public bool Pending { get; set; }
        public IEnumerable<EnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<EnrollmentDto>();

        public async Task OnGetAsync(bool pending = false)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            Pending = pending;
            var all = await _enrollmentService.GetAllAsync();
            Enrollments = pending ? all.Where(e => e.Status == "Registered").OrderByDescending(e => e.EnrolledAt) : all.OrderByDescending(e => e.EnrolledAt);
        }
    }
}
