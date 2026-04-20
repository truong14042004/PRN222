using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IEnumerable<CourseDto> Courses { get; set; } = Enumerable.Empty<CourseDto>();

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            Courses = await _courseService.GetAllAsync();
        }
    }
}
