using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Teacher
{
    public class IndexModel : PageModel
    {
        private readonly IClassService _classService;

        public IndexModel(IClassService classService)
        {
            _classService = classService;
        }

        public IEnumerable<ClassDto> Classes { get; set; } = Enumerable.Empty<ClassDto>();

        public async Task OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Teacher")
            {
                Response.Redirect("/");
                return;
            }

            Classes = await _classService.GetByTeacherIdAsync(int.Parse(userId));
        }
    }
}
