using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

/// <summary>
/// Manages user collections for organizing saved articles into custom groups.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CollectionsController : ControllerBase
{
    private readonly ICollectionRepository _collections;

    public CollectionsController(ICollectionRepository collections) => _collections = collections;

    /// <summary>
    /// Get all collections for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of the user's collections with article counts.</returns>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Get a collection's details including its articles.
    /// </summary>
    /// <param name="id">The collection ID.</param>
    /// <returns>Collection details with a list of contained articles.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Create a new collection for a user.
    /// </summary>
    /// <param name="request">Collection creation details.</param>
    /// <returns>The newly created collection.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
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

    /// <summary>
    /// Update a collection's name and description.
    /// </summary>
    /// <param name="id">The collection ID.</param>
    /// <param name="request">Updated collection details.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCollection(int id, [FromBody] UpdateCollectionRequest request)
    {
        var collection = await _collections.GetByIdAsync(id);
        if (collection is null) return NotFound();

        collection.Name = request.Name;
        collection.Description = request.Description;
        await _collections.UpdateAsync(collection);

        return NoContent();
    }

    /// <summary>
    /// Delete a collection.
    /// </summary>
    /// <param name="id">The collection ID to delete.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        await _collections.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Add an article to a collection.
    /// </summary>
    /// <param name="id">The collection ID.</param>
    /// <param name="articleId">The article ID to add.</param>
    [HttpPost("{id:int}/articles/{articleId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddArticle(int id, int articleId)
    {
        await _collections.AddArticleAsync(new CollectionArticle
        {
            CollectionId = id,
            ArticleId = articleId
        });
        return Created($"/api/collections/{id}", null);
    }

    /// <summary>
    /// Remove an article from a collection.
    /// </summary>
    /// <param name="id">The collection ID.</param>
    /// <param name="articleId">The article ID to remove.</param>
    [HttpDelete("{id:int}/articles/{articleId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveArticle(int id, int articleId)
    {
        await _collections.RemoveArticleAsync(id, articleId);
        return NoContent();
    }
}

/// <summary>
/// Request body for creating a new collection.
/// </summary>
/// <param name="UserId">The owning user's ID.</param>
/// <param name="Name">Collection name.</param>
/// <param name="Description">Optional description.</param>
public record CreateCollectionRequest(int UserId, string Name, string? Description);

/// <summary>
/// Request body for updating a collection.
/// </summary>
/// <param name="Name">New collection name.</param>
/// <param name="Description">New optional description.</param>
public record UpdateCollectionRequest(string Name, string? Description);
