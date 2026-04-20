using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin;

public class ClassesModel : PageModel
{
    private readonly IClassService _classService;

    public ClassesModel(IClassService classService)
    {
        _classService = classService;
    }

    public IReadOnlyList<ClassDto> Classes { get; private set; } = Array.Empty<ClassDto>();
    public string UserName { get; private set; } = string.Empty;

    public async Task OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrWhiteSpace(userId))
        {
            Response.Redirect("/Auth/Login");
            return;
        }

        if (role != "Admin")
        {
            Response.Redirect("/");
            return;
        }

        UserName = HttpContext.Session.GetString("UserName") ?? "Admin";
        Classes = (await _classService.GetAllAsync())
            .OrderBy(c => c.StartDate)
            .ThenBy(c => c.ClassName)
            .ToList();
    }
}
