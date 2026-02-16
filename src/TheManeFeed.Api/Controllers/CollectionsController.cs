using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionsController : ControllerBase
{
    private readonly ICollectionRepository _collections;

    public CollectionsController(ICollectionRepository collections) => _collections = collections;

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserCollections(int userId)
    {
        var collections = await _collections.GetByUserIdAsync(userId);
        return Ok(collections.Select(c => new
        {
            c.Id,
            c.Name,
            c.Description,
            c.CreatedAt,
            ArticleCount = c.CollectionArticles.Count
        }));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCollection(int id)
    {
        var collection = await _collections.GetByIdAsync(id);
        if (collection is null) return NotFound();

        return Ok(new
        {
            collection.Id,
            collection.Name,
            collection.Description,
            collection.CreatedAt,
            Articles = collection.CollectionArticles.Select(ca => new
            {
                ca.AddedAt,
                ca.Article.Id,
                ca.Article.Title,
                ca.Article.Summary,
                ca.Article.ImageUrl,
                ca.Article.SourceName
            })
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionRequest request)
    {
        var collection = await _collections.AddAsync(new Collection
        {
            UserId = request.UserId,
            Name = request.Name,
            Description = request.Description
        });

        return CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, new
        {
            collection.Id,
            collection.Name,
            collection.Description,
            collection.CreatedAt
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCollection(int id, [FromBody] UpdateCollectionRequest request)
    {
        var collection = await _collections.GetByIdAsync(id);
        if (collection is null) return NotFound();

        collection.Name = request.Name;
        collection.Description = request.Description;
        await _collections.UpdateAsync(collection);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        await _collections.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/articles/{articleId:int}")]
    public async Task<IActionResult> AddArticle(int id, int articleId)
    {
        await _collections.AddArticleAsync(new CollectionArticle
        {
            CollectionId = id,
            ArticleId = articleId
        });
        return Created($"/api/collections/{id}", null);
    }

    [HttpDelete("{id:int}/articles/{articleId:int}")]
    public async Task<IActionResult> RemoveArticle(int id, int articleId)
    {
        await _collections.RemoveArticleAsync(id, articleId);
        return NoContent();
    }
}

public record CreateCollectionRequest(int UserId, string Name, string? Description);
public record UpdateCollectionRequest(string Name, string? Description);
