using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

/// <summary>
/// Manages user profiles, saved articles, and category interests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly ISavedArticleRepository _saved;
    private readonly ICollectionRepository _collections;

    public UsersController(
        IUserRepository users,
        ISavedArticleRepository saved,
        ICollectionRepository collections)
    {
        _users = users;
        _saved = saved;
        _collections = collections;
    }

    /// <summary>
    /// Get a user's profile including stats and interests.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>User profile with saved count, topic count, collection count, and interest categories.</returns>
    [HttpGet("{id:int}/profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(int id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user is null) return NotFound();

        var savedCount = await _saved.GetCountByUserIdAsync(id);
        var userCollections = await _collections.GetByUserIdAsync(id);

        return Ok(new
        {
            user.Id,
            user.DisplayName,
            user.Username,
            user.Email,
            user.AvatarUrl,
            Stats = new
            {
                Saved = savedCount,
                Topics = user.Interests.Count,
                Collections = userCollections.Count
            },
            Interests = user.Interests.Select(i => new
            {
                i.Category.Id,
                i.Category.Name,
                i.Category.Slug,
                i.Category.Color
            })
        });
    }

    /// <summary>
    /// Get a user's saved articles.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="limit">Number of saved articles to return (default: 20).</param>
    /// <param name="offset">Number of entries to skip for pagination (default: 0).</param>
    /// <returns>A paginated list of saved articles with timestamps.</returns>
    [HttpGet("{id:int}/saved")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSaved(
        int id,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        var saved = await _saved.GetByUserIdAsync(id, limit, offset);
        return Ok(saved.Select(s => new
        {
            s.SavedAt,
            Article = new
            {
                s.Article.Id,
                s.Article.Title,
                s.Article.Summary,
                s.Article.ImageUrl,
                s.Article.SourceName,
                Category = s.Article.Category?.Name,
                s.Article.PublishedAt
            }
        }));
    }

    /// <summary>
    /// Save an article to a user's reading list.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="articleId">The article ID to save.</param>
    /// <returns>The created saved article record.</returns>
    [HttpPost("{id:int}/saved/{articleId:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SaveArticle(int id, int articleId)
    {
        var existing = await _saved.GetAsync(id, articleId);
        if (existing is not null)
            return Conflict("Article already saved.");

        var savedArticle = await _saved.AddAsync(new SavedArticle
        {
            UserId = id,
            ArticleId = articleId
        });

        return Created($"/api/users/{id}/saved", new { savedArticle.Id, savedArticle.SavedAt });
    }

    /// <summary>
    /// Remove an article from a user's reading list.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="articleId">The article ID to unsave.</param>
    [HttpDelete("{id:int}/saved/{articleId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UnsaveArticle(int id, int articleId)
    {
        await _saved.RemoveAsync(id, articleId);
        return NoContent();
    }

    /// <summary>
    /// Update a user's category interests.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="categoryIds">List of category IDs the user is interested in.</param>
    [HttpPut("{id:int}/interests")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInterests(int id, [FromBody] List<int> categoryIds)
    {
        var user = await _users.GetByIdAsync(id);
        if (user is null) return NotFound();

        user.Interests.Clear();
        foreach (var catId in categoryIds)
        {
            user.Interests.Add(new UserInterest { UserId = id, CategoryId = catId });
        }

        await _users.UpdateAsync(user);
        return NoContent();
    }
}
