using Microsoft.EntityFrameworkCore;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.BLL.Services;

public interface IPurchasableItemService
{
    Task<IEnumerable<PurchasableItemDto>> GetAllAsync();
    Task<PurchasableItemDto?> GetByIdAsync(int id);
}

public class PurchasableItemService : IPurchasableItemService
{
    private readonly AppDbContext _context;

    public PurchasableItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchasableItemDto>> GetAllAsync()
    {
        return await _context.PurchasableItems
            .Where(p => p.IsActive)
            .Select(p => new PurchasableItemDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive
            }).ToListAsync();
    }

    public async Task<PurchasableItemDto?> GetByIdAsync(int id)
    {
        var p = await _context.PurchasableItems.FindAsync(id);
        if (p == null) return null;

        return new PurchasableItemDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            IsActive = p.IsActive
        };
    }
}
