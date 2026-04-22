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
    Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
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
            OrderCode = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")), // Use date format to stay under PayOS limit (2^53-1)
            Status = OrderStatus.Pending
        };

        foreach (var item in cart.Items)
        {
            if (item.ItemType == "PurchasableItem")
            {
                var product = await _context.PurchasableItems.FindAsync(item.ItemId);
                if (product == null || product.Quantity <= 0)
                {
                    throw new Exception($"Sản phẩm '{item.Name}' đã hết hàng.");
                }
            }

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
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);

            if (order == null || order.Status == OrderStatus.Paid) 
            {
                await transaction.RollbackAsync();
                return;
            }

            // Deduct balance if used
            if (order.BalanceUsed > 0)
            {
                if (order.User.WalletBalance < order.BalanceUsed)
                    throw new Exception("Insufficient balance to complete order");
                
                order.User.WalletBalance -= order.BalanceUsed;
            }

            await EnsureCourseEnrollmentsAsync(order);
            
            // Deduct stock for purchasable items
            foreach (var item in order.OrderItems.Where(i => i.ItemType == OrderItemType.PurchasableItem))
            {
                var product = await _context.PurchasableItems.FindAsync(item.ItemId);
                if (product != null)
                {
                    if (product.Quantity <= 0) 
                        throw new Exception($"Sản phẩm '{product.Name}' đã hết hàng trong lúc xử lý thanh toán.");
                    
                    product.Quantity -= 1;
                }
            }

            order.Status = OrderStatus.Paid;
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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

    public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        var dtos = new List<OrderDto>();
        foreach (var order in orders)
        {
            dtos.Add(await MapToDto(order));
        }
        return dtos;
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
                var classItem = await _context.Classes.FindAsync(item.ItemId);
                if (classItem != null)
                {
                    var course = await _context.Courses.FindAsync(classItem.CourseId);
                    itemName = course is null
                        ? classItem.ClassName
                        : $"{course.Name} - {classItem.ClassName}";
                }
                else
                {
                    itemName = "Deleted Class";
                }
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

    private async Task EnsureCourseEnrollmentsAsync(Order order)
    {
        var classIds = order.OrderItems
            .Where(i => i.ItemType == OrderItemType.Course)
            .Select(i => i.ItemId)
            .Distinct()
            .ToList();

        if (!classIds.Any())
        {
            return;
        }

        var classes = await _context.Classes
            .Where(c => classIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id);

        var existingEnrollments = await _context.Enrollments
            .Where(e => e.StudentId == order.UserId && classIds.Contains(e.ClassId))
            .ToListAsync();

        foreach (var classId in classIds)
        {
            if (!classes.ContainsKey(classId))
            {
                continue;
            }

            var existing = existingEnrollments.FirstOrDefault(e => e.ClassId == classId);
            if (existing is null)
            {
                _context.Enrollments.Add(new Enrollment
                {
                    StudentId = order.UserId,
                    ClassId = classId,
                    Status = "Registered"
                });
                continue;
            }

            if (existing.Status == "Cancelled")
            {
                existing.Status = "Registered";
                existing.EnrolledAt = DateTime.Now;
                existing.ConfirmationCode = null;
                existing.ConfirmedAt = null;
            }
        }
    }
}
