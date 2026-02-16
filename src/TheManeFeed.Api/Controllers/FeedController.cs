using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

/// <summary>
/// Provides the aggregated home feed combining featured, latest, and trending content.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FeedController : ControllerBase
{
    private readonly IArticleRepository _articles;
    private readonly ICategoryRepository _categories;

    public FeedController(IArticleRepository articles, ICategoryRepository categories)
    {
        _articles = articles;
        _categories = categories;
    }

    /// <summary>
    /// Get the home feed with featured articles, latest articles, trending articles, and categories.
    /// </summary>
    /// <returns>An aggregated feed object with categories, featured, latest, and trending sections.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHomeFeed()
    {
        var featured = await _articles.GetFeaturedAsync(3);
        var latest = await _articles.GetLatestAsync(limit: 10);
        var trending = await _articles.GetTrendingAsync(5);
        var categories = await _categories.GetAllAsync();

        return Ok(new
        {
            Categories = categories.Select(c => new { c.Id, c.Name, c.Slug, c.Color }),
            Featured = featured.Select(MapArticle),
            Latest = latest.Select(MapArticle),
            Trending = trending.Select(a => new
            {
                a.Id,
                a.Title,
                a.ReadCount,
                a.IsTrending,
                Category = a.Category?.Name
            })
        });
    }

    private static object MapArticle(Core.Entities.Article a) => new
    {
        a.Id,
        a.Title,
        a.Summary,
        a.ImageUrl,
        a.SourceName,
        a.Url,
        Category = a.Category?.Name,
        CategorySlug = a.Category?.Slug,
        Author = a.Author?.Name,
        a.PublishedAt,
        a.ReadCount,
        a.IsFeatured,
        a.IsTrending
    };
}
