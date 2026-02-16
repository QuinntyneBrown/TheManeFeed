using Microsoft.EntityFrameworkCore;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;
using TheManeFeed.Infrastructure.Data;

namespace TheManeFeed.Infrastructure.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly TheManeFeedDbContext _db;

    public CollectionRepository(TheManeFeedDbContext db) => _db = db;

    public async Task<List<Collection>> GetByUserIdAsync(int userId)
    {
        return await _db.Collections
            .Include(c => c.CollectionArticles)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Collection?> GetByIdAsync(int id)
    {
        return await _db.Collections
            .Include(c => c.CollectionArticles)
                .ThenInclude(ca => ca.Article)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Collection> AddAsync(Collection collection)
    {
        _db.Collections.Add(collection);
        await _db.SaveChangesAsync();
        return collection;
    }

    public async Task UpdateAsync(Collection collection)
    {
        _db.Collections.Update(collection);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var collection = await _db.Collections.FindAsync(id);
        if (collection is not null)
        {
            _db.Collections.Remove(collection);
            await _db.SaveChangesAsync();
        }
    }

    public async Task AddArticleAsync(CollectionArticle collectionArticle)
    {
        _db.CollectionArticles.Add(collectionArticle);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveArticleAsync(int collectionId, int articleId)
    {
        var ca = await _db.CollectionArticles
            .FirstOrDefaultAsync(x => x.CollectionId == collectionId && x.ArticleId == articleId);

        if (ca is not null)
        {
            _db.CollectionArticles.Remove(ca);
            await _db.SaveChangesAsync();
        }
    }
}
