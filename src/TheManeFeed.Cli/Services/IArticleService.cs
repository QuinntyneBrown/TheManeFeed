using TheManeFeed.Cli.Models;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Cli.Services;

public interface IArticleService
{
    Task<int> SaveArticlesAsync(IEnumerable<ScrapeResult> results);
    Task<List<Article>> GetArticlesAsync(string? sourceName = null, int limit = 50);
}
