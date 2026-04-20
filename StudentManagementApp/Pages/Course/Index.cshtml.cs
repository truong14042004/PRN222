using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Course
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
            Courses = await _courseService.GetActiveCoursesAsync();
        }
    }
}
