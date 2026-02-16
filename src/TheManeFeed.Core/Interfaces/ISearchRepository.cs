using TheManeFeed.Core.Entities;

namespace TheManeFeed.Core.Interfaces;

public interface ISearchRepository
{
    Task<List<SearchHistory>> GetUserHistoryAsync(int userId, int limit = 10);
    Task<SearchHistory> AddHistoryAsync(SearchHistory history);
    Task ClearUserHistoryAsync(int userId);
    Task<List<TrendingSearch>> GetTrendingSearchesAsync(int limit = 10);
}
