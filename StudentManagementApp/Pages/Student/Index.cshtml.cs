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

        public IndexModel(IEnrollmentService enrollmentService, IUserService userService)
        {
            _enrollmentService = enrollmentService;
            _userService = userService;
        }

        public IEnumerable<EnrollmentDto> Enrollments { get; set; } = Enumerable.Empty<EnrollmentDto>();
        public decimal WalletBalance { get; set; }

        public async Task OnGetAsync()
        {
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

            Enrollments = await _enrollmentService.GetByStudentIdAsync(userId);
            var user = await _userService.GetByIdAsync(userId);
            WalletBalance = user?.WalletBalance ?? 0;
        }
    }
}
