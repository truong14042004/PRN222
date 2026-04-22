using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace StudentManagementApp.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;
        private readonly AppDbContext _dbContext;

        public IndexModel(
            IUserService userService,
            ICourseService courseService,
            IClassService classService,
            AppDbContext dbContext)
        {
            _userService = userService;
            _courseService = courseService;
            _classService = classService;
            _dbContext = dbContext;
        }

        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public int CourseCount { get; set; }
        public int ClassCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<string> RevenueMonthLabels { get; private set; } = new();
        public List<decimal> RevenueMonthValues { get; private set; } = new();

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
            TotalRevenue = await _dbContext.Orders
                .Where(o => o.Status == OrderStatus.Paid)
                .Select(o => o.TotalAmount)
                .SumAsync();

            await BuildRevenueChartAsync();
        }

        private async Task BuildRevenueChartAsync()
        {
            var now = DateTime.Now;
            var startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-5);
            var endMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);

            var paidOrders = await _dbContext.Orders
                .Where(o => o.Status == OrderStatus.Paid && o.CreatedAt >= startMonth && o.CreatedAt < endMonth)
                .Select(o => new { o.CreatedAt, o.TotalAmount })
                .ToListAsync();

            var monthMap = paidOrders
                .GroupBy(o => new DateTime(o.CreatedAt.Year, o.CreatedAt.Month, 1))
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalAmount));

            for (var i = 0; i < 6; i++)
            {
                var month = startMonth.AddMonths(i);
                RevenueMonthLabels.Add(month.ToString("MM/yyyy", CultureInfo.InvariantCulture));
                RevenueMonthValues.Add(monthMap.TryGetValue(month, out var value) ? value : 0m);
            }
        }
    }
}
