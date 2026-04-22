using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Pages.Admin.PurchasableItems
{
    public class CreateModel : PageModel
    {
        private readonly IPurchasableItemService _itemService;

        public CreateModel(IPurchasableItemService itemService)
        {
            _itemService = itemService;
        }

        [BindProperty]
        public PurchasableItemDto Input { get; set; } = new PurchasableItemDto();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                Response.Redirect("/Auth/Login");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _itemService.CreateAsync(Input);
            TempData["Success"] = "Đã thêm vật phẩm mới thành công.";
            return RedirectToPage("./Index");
        }
    }
}
