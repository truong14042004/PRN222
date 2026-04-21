using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Student;

public class OrdersModel : PageModel
{
    private readonly IOrderService _orderService;

    public OrdersModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public List<OrderDto> Orders { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            return RedirectToPage("/Auth/Login");
        }

        Orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Page();
    }
}
