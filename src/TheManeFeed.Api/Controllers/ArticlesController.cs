using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _articles;

    public ArticlesController(IArticleRepository articles) => _articles = articles;

    [HttpGet]
    public async Task<IActionResult> GetArticles(
        [FromQuery] int? categoryId,
        [FromQuery] string? source,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        var articles = await _articles.GetLatestAsync(categoryId, source, limit, offset);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetArticle(int id)
    {
        var article = await _articles.GetByIdAsync(id);
        if (article is null) return NotFound();

        article.ReadCount++;
        await _articles.UpdateAsync(article);

        return Ok(MapToDetailDto(article));
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured([FromQuery] int limit = 5)
    {
        var articles = await _articles.GetFeaturedAsync(limit);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    [HttpGet("trending")]
    public async Task<IActionResult> GetTrending([FromQuery] int limit = 10)
    {
        var articles = await _articles.GetTrendingAsync(limit);
        return Ok(articles.Select(a => MapToDto(a)));
    }

    [HttpGet("search")]
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
        a.ScrapedAt,
        a.ReadCount,
        a.IsFeatured,
        a.IsTrending
    };
}
