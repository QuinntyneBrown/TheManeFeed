using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

/// <summary>
/// Manages article categories for organizing content by topic.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categories;
    private readonly IArticleRepository _articles;

    public CategoriesController(ICategoryRepository categories, IArticleRepository articles)
    {
        _categories = categories;
        _articles = articles;
    }

    /// <summary>
    /// Get all available categories.
    /// </summary>
    /// <returns>A list of all categories with their display metadata.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categories.GetAllAsync();
        return Ok(categories.Select(c => new
        {
            c.Id,
            c.Name,
            c.Slug,
            c.Color,
            c.DisplayOrder
        }));
    }

    /// <summary>
    /// Get a single category by its URL slug.
    /// </summary>
    /// <param name="slug">The URL-friendly category slug (e.g. "hair-care").</param>
    /// <returns>The category details.</returns>
    [HttpGet("{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var category = await _categories.GetBySlugAsync(slug);
        if (category is null) return NotFound();

        return Ok(new
        {
            category.Id,
            category.Name,
            category.Slug,
            category.Color,
            category.DisplayOrder
        });
    }

    /// <summary>
    /// Get articles belonging to a specific category.
    /// </summary>
    /// <param name="slug">The category slug.</param>
    /// <param name="limit">Number of articles to return (default: 20).</param>
    /// <param name="offset">Number of articles to skip for pagination (default: 0).</param>
    /// <returns>A paginated list of articles in the given category.</returns>
    [HttpGet("{slug}/articles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryArticles(
        string slug,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        var category = await _categories.GetBySlugAsync(slug);
        if (category is null) return NotFound();

        var articles = await _articles.GetLatestAsync(category.Id, limit: limit, offset: offset);
        return Ok(articles.Select(a => new
        {
            a.Id,
            a.Title,
            a.Summary,
            a.ImageUrl,
            a.SourceName,
            a.Url,
            Author = a.Author?.Name,
            a.PublishedAt,
            a.ReadCount
        }));
    }
}
