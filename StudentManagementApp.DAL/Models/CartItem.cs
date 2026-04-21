using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class CartItem
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Required]
    public OrderItemType ItemType { get; set; }

    [Required]
    public int ItemId { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.Now;
}
