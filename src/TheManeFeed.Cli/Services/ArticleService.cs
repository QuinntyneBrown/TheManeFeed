using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheManeFeed.Cli.Models;
using TheManeFeed.Core.Entities;
using TheManeFeed.Infrastructure.Data;

namespace TheManeFeed.Cli.Services;

public class ArticleService : IArticleService
{
    private readonly TheManeFeedDbContext _db;
    private readonly ILogger<ArticleService> _logger;

    public ArticleService(TheManeFeedDbContext db, ILogger<ArticleService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<int> SaveArticlesAsync(IEnumerable<ScrapeResult> results)
    {
        var saved = 0;
        var seenUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var result in results)
        {
            var normalizedUrl = NormalizeUrl(result.Url);

            if (!seenUrls.Add(normalizedUrl))
            {
                _logger.LogDebug("Skipping batch duplicate: {Url}", normalizedUrl);
                continue;
            }

            var exists = await _db.Articles.AnyAsync(a => a.Url == normalizedUrl);
            if (exists)
            {
                _logger.LogDebug("Skipping duplicate: {Url}", normalizedUrl);
                continue;
            }

            var now = DateTime.UtcNow;
            _db.Articles.Add(new Article
            {
                Url = normalizedUrl,
                Title = result.Title,
                Summary = result.Summary,
                ImageUrl = result.ImageUrl,
                SourceName = result.SourceName,
                PublishedAt = result.PublishedAt,
                CreatedAt = now,
                ScrapedAt = now
            });

            saved++;
        }

        if (saved > 0)
            await _db.SaveChangesAsync();

        _logger.LogInformation("Saved {Count} new articles", saved);
        return saved;
    }

    public async Task<List<Article>> GetArticlesAsync(string? sourceName = null, int limit = 50)
    {
        var query = _db.Articles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(sourceName))
            query = query.Where(a => a.SourceName == sourceName);

        return await query
            .OrderByDescending(a => a.ScrapedAt)
            .Take(limit)
            .ToListAsync();
    }

    private static string NormalizeUrl(string url)
    {
        url = url.Trim().TrimEnd('/');

        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return $"{uri.Scheme}://{uri.Host.ToLowerInvariant()}{uri.AbsolutePath.TrimEnd('/')}";
        }

        return url.ToLowerInvariant();
    }
}
