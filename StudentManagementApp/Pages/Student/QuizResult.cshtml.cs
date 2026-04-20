using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student;

public class QuizResultModel : PageModel
{
    private readonly IQuizResultService _quizResultService;

    public QuizResultModel(IQuizResultService quizResultService)
    {
        _quizResultService = quizResultService;
    }

    public QuizResultDto? Result { get; private set; }

    public async Task OnGetAsync(int id)
    {
        var access = await ResolveStudentAccessAsync();
        if (!access.Ok)
        {
            return;
        }

        var result = await _quizResultService.GetByIdAsync(id);
        if (result is null || result.StudentId != access.StudentId)
        {
            Response.Redirect("/Student/Quizzes");
            return;
        }

        Result = result;
    }

    private async Task<(bool Ok, int StudentId)> ResolveStudentAccessAsync()
    {
        await Task.CompletedTask;

        var userIdRaw = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrWhiteSpace(userIdRaw) || !int.TryParse(userIdRaw, out var userId))
        {
            Response.Redirect("/Auth/Login");
            return (false, 0);
        }

        if (role != "Student")
        {
            Response.Redirect("/");
            return (false, 0);
        }

        return (true, userId);
    }
}
