using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet("{id:int}/profile")]
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

    [HttpGet("{id:int}/saved")]
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

    [HttpPost("{id:int}/saved/{articleId:int}")]
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

    [HttpDelete("{id:int}/saved/{articleId:int}")]
    public async Task<IActionResult> UnsaveArticle(int id, int articleId)
    {
        await _saved.RemoveAsync(id, articleId);
        return NoContent();
    }

    [HttpPut("{id:int}/interests")]
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
