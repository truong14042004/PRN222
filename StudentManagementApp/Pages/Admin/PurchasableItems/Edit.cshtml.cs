using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.PurchasableItems
{
    public class EditModel : PageModel
    {
        private readonly IPurchasableItemService _itemService;

        public EditModel(IPurchasableItemService itemService)
        {
            _itemService = itemService;
        }

        [BindProperty]
        public PurchasableItemDto Input { get; set; } = new PurchasableItemDto();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToPage("/Auth/Login");
            }

            var item = await _itemService.GetByIdAsync(id);
            if (item == null)
            {
                return RedirectToPage("./Index");
            }

            Input = item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _itemService.UpdateAsync(Input);
            TempData["Success"] = "Đã cập nhật vật phẩm thành công.";
            return RedirectToPage("./Index");
        }
    }
}
