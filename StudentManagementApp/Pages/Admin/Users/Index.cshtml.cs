using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private const int PageSize = 15;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public string Tab { get; set; } = "students";
        public string? Q { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<UserDto> Users { get; set; } = Enumerable.Empty<UserDto>();

        public async Task OnGetAsync(string tab = "students", string? q = null, int page = 1)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            Tab = tab;
            Q = q;
            CurrentPage = page;

            var all = await _userService.GetAllAsync();
            var role = tab == "teachers" ? "Teacher" : "Student";
            var users = all.Where(u => u.Role == role);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                users = users.Where(u =>
                    u.FullName.ToLower().Contains(lower) ||
                    u.Email.ToLower().Contains(lower) ||
                    (u.Phone != null && u.Phone.Contains(lower)));
            }

            TotalCount = users.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            CurrentPage = Math.Max(1, Math.Min(page, TotalPages == 0 ? 1 : TotalPages));

            Users = users.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
        }
    }
}
