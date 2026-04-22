using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public enum OrderItemType
{
    Course,
    PurchasableItem
}

public class OrderItem
{
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; } = null!;

    [Required]
    public OrderItemType ItemType { get; set; }

    [Required]
    public int ItemId { get; set; } // Could be CourseId or PurchasableItemId

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; } = 1;
}
