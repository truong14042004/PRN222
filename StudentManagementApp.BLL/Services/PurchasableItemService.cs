using Microsoft.EntityFrameworkCore;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.BLL.Services;

public interface IPurchasableItemService
{
    Task<IEnumerable<PurchasableItemDto>> GetAllAsync(bool includeInactive = false);
    Task<PurchasableItemDto?> GetByIdAsync(int id);
    Task CreateAsync(PurchasableItemDto dto);
    Task UpdateAsync(PurchasableItemDto dto);
    Task DeleteAsync(int id);
}

public class PurchasableItemService : IPurchasableItemService
{
    private readonly AppDbContext _context;

    public PurchasableItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchasableItemDto>> GetAllAsync(bool includeInactive = false)
    {
        var query = _context.PurchasableItems.AsQueryable();
        if (!includeInactive)
        {
            query = query.Where(p => p.IsActive);
        }

        return await query
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

    public async Task CreateAsync(PurchasableItemDto dto)
    {
        var item = new PurchasableItem
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            IsActive = true,
            CreatedAt = DateTime.Now
        };
        _context.PurchasableItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PurchasableItemDto dto)
    {
        var item = await _context.PurchasableItems.FindAsync(dto.Id);
        if (item != null)
        {
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.ImageUrl = dto.ImageUrl;
            item.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.PurchasableItems.FindAsync(id);
        if (item != null)
        {
            _context.PurchasableItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
