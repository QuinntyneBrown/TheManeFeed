using TheManeFeed.Core.Entities;

namespace TheManeFeed.Core.Interfaces;

public interface IArticleRepository
{
    Task<Article?> GetByIdAsync(int id);
    Task<Article?> GetByUrlAsync(string url);
    Task<List<Article>> GetLatestAsync(int? categoryId = null, string? sourceName = null, int limit = 20, int offset = 0);
    Task<List<Article>> GetFeaturedAsync(int limit = 5);
    Task<List<Article>> GetTrendingAsync(int limit = 10);
    Task<List<Article>> SearchAsync(string query, int limit = 20, int offset = 0);
    Task<Article> AddAsync(Article article);
    Task UpdateAsync(Article article);
    Task<bool> ExistsByUrlAsync(string url);
}
