using Microsoft.EntityFrameworkCore;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.BLL.Services;

public interface ICartService
{
    Task<CartDto> GetCartAsync(int userId);
    Task AddToCartAsync(int userId, OrderItemType itemType, int itemId);
    Task RemoveFromCartAsync(int userId, int cartItemId);
    Task ClearCartAsync(int userId);
}

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> GetCartAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.AddedAt)
            .ToListAsync();

        var cartDto = new CartDto();

        foreach (var item in cartItems)
        {
            if (item.ItemType == OrderItemType.Course)
            {
                var course = await _context.Courses.FindAsync(item.ItemId);
                if (course != null)
                {
                    cartDto.Items.Add(new CartItemDto
                    {
                        ItemType = "Course",
                        ItemId = course.Id,
                        Name = course.Name,
                        Price = course.TuitionFee,
                        ImageUrl = course.ThumbnailUrl
                    });
                }
            }
            else if (item.ItemType == OrderItemType.PurchasableItem)
            {
                var product = await _context.PurchasableItems.FindAsync(item.ItemId);
                if (product != null)
                {
                    cartDto.Items.Add(new CartItemDto
                    {
                        ItemType = "PurchasableItem",
                        ItemId = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl
                    });
                }
            }
        }

        return cartDto;
    }

    public async Task AddToCartAsync(int userId, OrderItemType itemType, int itemId)
    {
        var exists = await _context.CartItems
            .AnyAsync(c => c.UserId == userId && c.ItemType == itemType && c.ItemId == itemId);

        if (!exists)
        {
            _context.CartItems.Add(new CartItem
            {
                UserId = userId,
                ItemType = itemType,
                ItemId = itemId
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFromCartAsync(int userId, int cartItemId)
    {
        var item = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
        
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int userId)
    {
        var items = await _context.CartItems.Where(c => c.UserId == userId).ToListAsync();
        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
}
