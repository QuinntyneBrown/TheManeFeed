using Microsoft.EntityFrameworkCore;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;
using TheManeFeed.Infrastructure.Data;

namespace TheManeFeed.Infrastructure.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly TheManeFeedDbContext _db;

    public SearchRepository(TheManeFeedDbContext db) => _db = db;

    public async Task<List<SearchHistory>> GetUserHistoryAsync(int userId, int limit = 10)
    {
        return await _db.SearchHistories
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SearchedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<SearchHistory> AddHistoryAsync(SearchHistory history)
    {
        _db.SearchHistories.Add(history);
        await _db.SaveChangesAsync();
        return history;
    }

    public async Task ClearUserHistoryAsync(int userId)
    {
        var histories = await _db.SearchHistories
            .Where(s => s.UserId == userId)
            .ToListAsync();

        _db.SearchHistories.RemoveRange(histories);
        await _db.SaveChangesAsync();
    }

    public async Task<List<TrendingSearch>> GetTrendingSearchesAsync(int limit = 10)
    {
        return await _db.TrendingSearches
            .OrderByDescending(t => t.SearchCount)
            .Take(limit)
            .ToListAsync();
    }
}
