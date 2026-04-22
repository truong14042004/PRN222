using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Cart;

public class SuccessModel : PageModel
{
    private readonly IOrderService _orderService;

    public SuccessModel(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public OrderDto? Order { get; private set; }

    public async Task<IActionResult> OnGetAsync(long orderCode)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return RedirectToPage("/Auth/Login");
        }

        Order = await _orderService.GetByOrderCodeAsync(orderCode);
        if (Order is null || Order.UserId != userId)
        {
            TempData["Error"] = "Không tìm thấy đơn hàng phù hợp.";
            return RedirectToPage("/Cart");
        }

        if (Order.Status == "Pending")
        {
            await _orderService.CompleteOrderAsync(orderCode);
            Order = await _orderService.GetByOrderCodeAsync(orderCode) ?? Order;
        }

        return Page();
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        var raw = HttpContext.Session.GetString("UserId");
        return int.TryParse(raw, out userId);
    }
}
