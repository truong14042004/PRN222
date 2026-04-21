namespace StudentManagementApp.BLL.DTOs;

public class CartDto
{
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public decimal TotalAmount => Items.Sum(i => i.Price);
}

public class CartItemDto
{
    public string ItemType { get; set; } = null!; // "Course" or "PurchasableItem"
    public int ItemId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
