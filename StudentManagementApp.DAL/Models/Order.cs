using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public enum OrderStatus
{
    Pending,
    Paid,
    Cancelled
}

public class Order
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceUsed { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PayOSAmount { get; set; }

    /// <summary>
    /// PayOS requires orderCode as long
    /// </summary>
    public long OrderCode { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
