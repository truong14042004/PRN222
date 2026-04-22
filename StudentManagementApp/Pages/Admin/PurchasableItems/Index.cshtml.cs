using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.PurchasableItems
{
    public class IndexModel : PageModel
    {
        private readonly IPurchasableItemService _itemService;

        public IndexModel(IPurchasableItemService itemService)
        {
            _itemService = itemService;
        }

        public IEnumerable<PurchasableItemDto> Items { get; set; } = Enumerable.Empty<PurchasableItemDto>();

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                Response.Redirect("/Auth/Login");
                return;
            }

            Items = await _itemService.GetAllAsync(includeInactive: true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            await _itemService.DeleteAsync(id);
            TempData["Success"] = "Đã xóa vật phẩm thành công.";
            return RedirectToPage();
        }
    }
}
