using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categories;
    private readonly IArticleRepository _articles;

    public CategoriesController(ICategoryRepository categories, IArticleRepository articles)
    {
        _categories = categories;
        _articles = articles;
    }

    [HttpGet]
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

    [HttpGet("{slug}")]
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

    [HttpGet("{slug}/articles")]
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
