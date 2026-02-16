using TheManeFeed.Core.Entities;

namespace TheManeFeed.Core.Interfaces;

public interface ISavedArticleRepository
{
    Task<List<SavedArticle>> GetByUserIdAsync(int userId, int limit = 50, int offset = 0);
    Task<SavedArticle?> GetAsync(int userId, int articleId);
    Task<SavedArticle> AddAsync(SavedArticle savedArticle);
    Task RemoveAsync(int userId, int articleId);
    Task<int> GetCountByUserIdAsync(int userId);
}
