using Microsoft.EntityFrameworkCore;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;
using TheManeFeed.Infrastructure.Data;

namespace TheManeFeed.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly TheManeFeedDbContext _db;

    public CategoryRepository(TheManeFeedDbContext db) => _db = db;

    public async Task<List<Category>> GetAllAsync()
    {
        return await _db.Categories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _db.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _db.Categories.FindAsync(id);
    }
}
