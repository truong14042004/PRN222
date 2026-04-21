using Microsoft.EntityFrameworkCore;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.BLL.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(int userId, bool useBalance);
    Task<OrderDto?> GetByOrderCodeAsync(long orderCode);
    Task CompleteOrderAsync(long orderCode);
    Task CancelOrderAsync(long orderCode);
}

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly ICartService _cartService;

    public OrderService(AppDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    public async Task<OrderDto> CreateOrderAsync(int userId, bool useBalance)
    {
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");
        var cart = await _cartService.GetCartAsync(userId);

        if (!cart.Items.Any()) throw new Exception("Cart is empty");

        decimal totalAmount = cart.TotalAmount;
        decimal balanceUsed = 0;

        if (useBalance)
        {
            balanceUsed = Math.Min(totalAmount, user.WalletBalance);
        }

        decimal payOSAmount = totalAmount - balanceUsed;

        var order = new Order
        {
            UserId = userId,
            TotalAmount = totalAmount,
            BalanceUsed = balanceUsed,
            PayOSAmount = payOSAmount,
            OrderCode = DateTime.Now.Ticks, // Simple unique order code
            Status = OrderStatus.Pending
        };

        foreach (var item in cart.Items)
        {
            order.OrderItems.Add(new OrderItem
            {
                ItemType = item.ItemType == "Course" ? OrderItemType.Course : OrderItemType.PurchasableItem,
                ItemId = item.ItemId,
                Price = item.Price
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        await _cartService.ClearCartAsync(userId);

        return await MapToDto(order);
    }

    public async Task<OrderDto?> GetByOrderCodeAsync(long orderCode)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

        if (order == null) return null;
        return await MapToDto(order);
    }

    public async Task CompleteOrderAsync(long orderCode)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

        if (order == null || order.Status == OrderStatus.Paid) return;

        // Deduct balance if used
        if (order.BalanceUsed > 0)
        {
            if (order.User.WalletBalance < order.BalanceUsed)
                throw new Exception("Insufficient balance to complete order");
            
            order.User.WalletBalance -= order.BalanceUsed;
        }

        order.Status = OrderStatus.Paid;

        // Perform side effects (give ownership)
        // Note: For now we just mark as Paid as per user's "khúc sau tôi tính tiếp"
        
        await _context.SaveChangesAsync();
    }

    public async Task CancelOrderAsync(long orderCode)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
        if (order != null && order.Status == OrderStatus.Pending)
        {
            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
        }
    }

    private async Task<OrderDto> MapToDto(Order order)
    {
        var dto = new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            FullName = order.User?.FullName ?? "",
            TotalAmount = order.TotalAmount,
            BalanceUsed = order.BalanceUsed,
            PayOSAmount = order.PayOSAmount,
            OrderCode = order.OrderCode,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };

        foreach (var item in order.OrderItems)
        {
            string itemName = "Unknown";
            if (item.ItemType == OrderItemType.Course)
            {
                var course = await _context.Courses.FindAsync(item.ItemId);
                itemName = course?.Name ?? "Deleted Course";
            }
            else
            {
                var product = await _context.PurchasableItems.FindAsync(item.ItemId);
                itemName = product?.Name ?? "Deleted Item";
            }

            dto.OrderItems.Add(new OrderItemDto
            {
                Id = item.Id,
                ItemType = item.ItemType.ToString(),
                ItemId = item.ItemId,
                ItemName = itemName,
                Price = item.Price
            });
        }

        return dto;
    }
}
