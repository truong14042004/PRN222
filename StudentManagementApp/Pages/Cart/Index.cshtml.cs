using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Data;

namespace StudentManagementApp.Pages.Cart;

public class IndexModel : PageModel
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly AppDbContext _context;

    public IndexModel(ICartService cartService, IOrderService orderService, IPaymentService paymentService, AppDbContext context)
    {
        _cartService = cartService;
        _orderService = orderService;
        _paymentService = paymentService;
        _context = context;
    }

    public CartDto Cart { get; set; } = new CartDto();
    public decimal WalletBalance { get; set; }

    public async Task OnGetAsync()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (!string.IsNullOrEmpty(userIdStr))
        {
            int userId = int.Parse(userIdStr);
            Cart = await _cartService.GetCartAsync(userId);
            
            var user = await _context.Users.FindAsync(userId);
            WalletBalance = user?.WalletBalance ?? 0;
        }
    }

    public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Auth/Login");

        await _cartService.RemoveFromCartAsync(int.Parse(userIdStr), cartItemId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCheckoutAsync(bool useBalance)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return RedirectToPage("/Auth/Login");

        int userId = int.Parse(userIdStr);
        try
        {
            var order = await _orderService.CreateOrderAsync(userId, useBalance);

            if (order.PayOSAmount <= 0)
            {
                // Paid fully by wallet
                await _orderService.CompleteOrderAsync(order.OrderCode);
                TempData["Success"] = "Thanh toán thành công bằng ví điện tử!";
                return RedirectToPage("/Student/Index");
            }

            // Need PayOS payment
            string domain = Request.Scheme + "://" + Request.Host;
            var paymentResult = await _paymentService.CreatePaymentLink(
                order.OrderCode, 
                (int)order.PayOSAmount, 
                "Thanh toan don hang #" + order.OrderCode,
                domain + "/Cart/Success?orderCode=" + order.OrderCode,
                domain + "/Cart/Cancel?orderCode=" + order.OrderCode
            );

            return Redirect(paymentResult.checkoutUrl);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToPage();
        }
    }
}
