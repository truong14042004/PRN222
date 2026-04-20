using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IEnumerable<CourseDto> Courses { get; set; } = Enumerable.Empty<CourseDto>();

        public async Task OnGetAsync(string? q)
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                courses = courses.Where(c =>
                    c.Name.ToLower().Contains(lower) ||
                    c.Level.ToLower().Contains(lower));
                ViewData["Q"] = q;
            }

            Courses = courses;
        }
    }
}
