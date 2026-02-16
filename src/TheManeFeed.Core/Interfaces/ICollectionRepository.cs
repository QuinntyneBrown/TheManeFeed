using TheManeFeed.Core.Entities;

namespace TheManeFeed.Core.Interfaces;

public interface ICollectionRepository
{
    Task<List<Collection>> GetByUserIdAsync(int userId);
    Task<Collection?> GetByIdAsync(int id);
    Task<Collection> AddAsync(Collection collection);
    Task UpdateAsync(Collection collection);
    Task DeleteAsync(int id);
    Task AddArticleAsync(CollectionArticle collectionArticle);
    Task RemoveArticleAsync(int collectionId, int articleId);
}
