using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;

        public IndexModel(IUserService userService, ICourseService courseService, IClassService classService)
        {
            _userService = userService;
            _courseService = courseService;
            _classService = classService;
        }

        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public int CourseCount { get; set; }
        public int ClassCount { get; set; }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            var users = await _userService.GetAllAsync();
            var courses = await _courseService.GetAllAsync();
            var classes = await _classService.GetAllAsync();

            StudentCount = users.Count(u => u.Role == "Student");
            TeacherCount = users.Count(u => u.Role == "Teacher");
            CourseCount = courses.Count();
            ClassCount = classes.Count();
        }
    }
}
