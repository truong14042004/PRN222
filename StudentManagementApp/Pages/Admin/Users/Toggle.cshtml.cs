using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Users
{
    public class ToggleModel : PageModel
    {
        private readonly IUserService _userService;

        public ToggleModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnPostAsync(int id, string tab = "teachers")
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            var currentId = int.Parse(HttpContext.Session.GetString("UserId")!);
            if (id == currentId)
            {
                TempData["Error"] = "Không thể vô hiệu hóa tài khoản của chính mình.";
                return RedirectToPage(new { tab });
            }

            var user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToPage(new { tab });
            }

            await _userService.ToggleActiveAsync(id);
            TempData["Success"] = user.IsActive ? "Đã vô hiệu hóa tài khoản." : "Đã kích hoạt tài khoản.";
            return RedirectToPage(new { tab });
        }
    }
}
