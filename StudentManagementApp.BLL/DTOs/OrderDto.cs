namespace StudentManagementApp.BLL.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal BalanceUsed { get; set; }
    public decimal PayOSAmount { get; set; }
    public long OrderCode { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}

public class OrderItemDto
{
    public int Id { get; set; }
    public string ItemType { get; set; } = null!;
    public int ItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public decimal Price { get; set; }
}
