using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.Classes;

public class DeleteModel : PageModel
{
    private readonly IClassService _classService;

    public DeleteModel(IClassService classService)
    {
        _classService = classService;
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin")
        {
            return RedirectToPage("/Auth/Login");
        }

        try
        {
            await _classService.DeleteAsync(id);
            TempData["Success"] = "Đã xóa lớp học.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToPage("/Admin/Classes");
    }
}
