using Microsoft.EntityFrameworkCore;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;
using TheManeFeed.Infrastructure.Data;

namespace TheManeFeed.Infrastructure.Repositories;

public class SavedArticleRepository : ISavedArticleRepository
{
    private readonly TheManeFeedDbContext _db;

    public SavedArticleRepository(TheManeFeedDbContext db) => _db = db;

    public async Task<List<SavedArticle>> GetByUserIdAsync(int userId, int limit = 50, int offset = 0)
    {
        return await _db.SavedArticles
            .Include(s => s.Article)
                .ThenInclude(a => a.Category)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SavedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<SavedArticle?> GetAsync(int userId, int articleId)
    {
        return await _db.SavedArticles
            .FirstOrDefaultAsync(s => s.UserId == userId && s.ArticleId == articleId);
    }

    public async Task<SavedArticle> AddAsync(SavedArticle savedArticle)
    {
        _db.SavedArticles.Add(savedArticle);
        await _db.SaveChangesAsync();
        return savedArticle;
    }

    public async Task RemoveAsync(int userId, int articleId)
    {
        var saved = await _db.SavedArticles
            .FirstOrDefaultAsync(s => s.UserId == userId && s.ArticleId == articleId);

        if (saved is not null)
        {
            _db.SavedArticles.Remove(saved);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<int> GetCountByUserIdAsync(int userId)
    {
        return await _db.SavedArticles.CountAsync(s => s.UserId == userId);
    }
}
