using Microsoft.EntityFrameworkCore;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;
using TheManeFeed.Infrastructure.Data;

namespace TheManeFeed.Infrastructure.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly TheManeFeedDbContext _db;

    public ArticleRepository(TheManeFeedDbContext db) => _db = db;

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _db.Articles
            .Include(a => a.Category)
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Article?> GetByUrlAsync(string url)
    {
        return await _db.Articles.FirstOrDefaultAsync(a => a.Url == url);
    }

    public async Task<List<Article>> GetLatestAsync(int? categoryId = null, string? sourceName = null, int limit = 20, int offset = 0)
    {
        var query = _db.Articles
            .Include(a => a.Category)
            .Include(a => a.Author)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(a => a.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(sourceName))
            query = query.Where(a => a.SourceName == sourceName);

        return await query
            .OrderByDescending(a => a.PublishedAt ?? a.ScrapedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Article>> GetFeaturedAsync(int limit = 5)
    {
        return await _db.Articles
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => a.IsFeatured)
            .OrderByDescending(a => a.PublishedAt ?? a.ScrapedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Article>> GetTrendingAsync(int limit = 10)
    {
        return await _db.Articles
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => a.IsTrending)
            .OrderByDescending(a => a.ReadCount)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Article>> SearchAsync(string query, int limit = 20, int offset = 0)
    {
        return await _db.Articles
            .Include(a => a.Category)
            .Include(a => a.Author)
            .Where(a => a.Title.Contains(query) || (a.Summary != null && a.Summary.Contains(query)))
            .OrderByDescending(a => a.PublishedAt ?? a.ScrapedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Article> AddAsync(Article article)
    {
        _db.Articles.Add(article);
        await _db.SaveChangesAsync();
        return article;
    }

    public async Task UpdateAsync(Article article)
    {
        _db.Articles.Update(article);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> ExistsByUrlAsync(string url)
    {
        return await _db.Articles.AnyAsync(a => a.Url == url);
    }
}
