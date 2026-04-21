using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.Pages.PurchasableItems;

public class IndexModel : PageModel
{
    private readonly IPurchasableItemService _purchasableItemService;
    private readonly ICartService _cartService;

    public IndexModel(IPurchasableItemService purchasableItemService, ICartService cartService)
    {
        _purchasableItemService = purchasableItemService;
        _cartService = cartService;
    }

    public IEnumerable<PurchasableItemDto> Items { get; set; } = new List<PurchasableItemDto>();

    public async Task OnGetAsync()
    {
        Items = await _purchasableItemService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int itemId)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
        {
            return RedirectToPage("/Auth/Login");
        }

        int userId = int.Parse(userIdStr);
        await _cartService.AddToCartAsync(userId, OrderItemType.PurchasableItem, itemId);
        
        TempData["Success"] = "Đã thêm vào giỏ hàng!";
        return RedirectToPage();
    }
}
