using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

/// <summary>
/// Manages hair and beauty articles including browsing, searching, and discovery.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _articles;

    public ArticlesController(IArticleRepository articles) => _articles = articles;

    /// <summary>
    /// Get a paginated list of articles with optional filtering.
    /// </summary>
    /// <param name="categoryId">Filter by category ID.</param>
    /// <param name="source">Filter by source publication name.</param>
    /// <param name="limit">Number of articles to return (default: 20).</param>
    /// <param name="offset">Number of articles to skip for pagination (default: 0).</param>
    /// <returns>A list of article summaries.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticles(
        [FromQuery] int? categoryId,
        [FromQuery] string? source,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        var articles = await _articles.GetLatestAsync(categoryId, source, limit, offset);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    /// <summary>
    /// Get a single article by ID. Increments the article's read count.
    /// </summary>
    /// <param name="id">The article ID.</param>
    /// <returns>Full article details including body content and author info.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticle(int id)
    {
        var article = await _articles.GetByIdAsync(id);
        if (article is null) return NotFound();

        article.ReadCount++;
        await _articles.UpdateAsync(article);

        return Ok(MapToDetailDto(article));
    }

    /// <summary>
    /// Get featured/editor's pick articles.
    /// </summary>
    /// <param name="limit">Number of featured articles to return (default: 5).</param>
    /// <returns>A list of featured article summaries.</returns>
    [HttpGet("featured")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeatured([FromQuery] int limit = 5)
    {
        var articles = await _articles.GetFeaturedAsync(limit);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    /// <summary>
    /// Get currently trending articles based on read count.
    /// </summary>
    /// <param name="limit">Number of trending articles to return (default: 10).</param>
    /// <returns>A list of trending article summaries.</returns>
    [HttpGet("trending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrending([FromQuery] int limit = 10)
    {
        var articles = await _articles.GetTrendingAsync(limit);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    /// <summary>
    /// Search articles by title or summary text.
    /// </summary>
    /// <param name="q">Search query string (required).</param>
    /// <param name="limit">Number of results to return (default: 20).</param>
    /// <param name="offset">Number of results to skip for pagination (default: 0).</param>
    /// <returns>A list of matching article summaries.</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Query parameter 'q' is required.");

        var articles = await _articles.SearchAsync(q, limit, offset);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    private static object MapToDto(Core.Entities.Article a) => new
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
        a.CreatedAt,
        a.ReadCount,
        a.IsFeatured,
        a.IsTrending
    };

    private static object MapToDetailDto(Core.Entities.Article a) => new
    {
        a.Id,
        a.Title,
        a.Summary,
        a.Body,
        a.ImageUrl,
        a.SourceName,
        a.Url,
        Category = a.Category?.Name,
        CategorySlug = a.Category?.Slug,
        Author = a.Author is not null ? new { a.Author.Id, a.Author.Name, a.Author.AvatarUrl, a.Author.Bio } : null,
        a.PublishedAt,
        a.CreatedAt,
        a.ScrapedAt,
        a.ReadCount,
        a.IsFeatured,
        a.IsTrending
    };
}
