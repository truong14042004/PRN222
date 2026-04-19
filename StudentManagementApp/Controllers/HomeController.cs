using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.Models;

namespace StudentManagementApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;

        public HomeController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(string? q)
        {
            var courses = await _courseService.GetActiveCoursesAsync();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                courses = courses.Where(c =>
                    c.Name.ToLower().Contains(lower) ||
                    c.Level.ToLower().Contains(lower));
            }
            ViewBag.Q = q;
            return View(courses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
