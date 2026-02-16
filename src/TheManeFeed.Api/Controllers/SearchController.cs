using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

/// <summary>
/// Provides article search, trending searches, and user search history management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SearchController : ControllerBase
{
    private readonly ISearchRepository _search;
    private readonly IArticleRepository _articles;

    public SearchController(ISearchRepository search, IArticleRepository articles)
    {
        _search = search;
        _articles = articles;
    }

    /// <summary>
    /// Search articles by title or summary text.
    /// </summary>
    /// <param name="q">Search query string (required).</param>
    /// <param name="limit">Number of results to return (default: 20).</param>
    /// <param name="offset">Number of results to skip for pagination (default: 0).</param>
    /// <returns>A list of matching articles.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Query parameter 'q' is required.");

        var results = await _articles.SearchAsync(q, limit, offset);
        return Ok(results.Select(a => new
        {
            a.Id,
            a.Title,
            a.Summary,
            a.ImageUrl,
            a.SourceName,
            Category = a.Category?.Name,
            a.PublishedAt
        }));
    }

    /// <summary>
    /// Get trending search queries across all users.
    /// </summary>
    /// <param name="limit">Number of trending searches to return (default: 10).</param>
    /// <returns>A list of trending search queries with their counts.</returns>
    [HttpGet("trending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrending([FromQuery] int limit = 10)
    {
        var trending = await _search.GetTrendingSearchesAsync(limit);
        return Ok(trending.Select(t => new { t.Query, t.SearchCount }));
    }

    /// <summary>
    /// Get a user's recent search history.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="limit">Number of history entries to return (default: 10).</param>
    /// <returns>A list of recent search queries with timestamps.</returns>
    [HttpGet("history/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(int userId, [FromQuery] int limit = 10)
    {
        var history = await _search.GetUserHistoryAsync(userId, limit);
        return Ok(history.Select(h => new { h.Query, h.SearchedAt }));
    }

    /// <summary>
    /// Record a search query in the user's history.
    /// </summary>
    /// <param name="request">The search history entry to add.</param>
    /// <returns>The created search history record.</returns>
    [HttpPost("history")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddHistory([FromBody] AddSearchHistoryRequest request)
    {
        var history = await _search.AddHistoryAsync(new SearchHistory
        {
            UserId = request.UserId,
            Query = request.Query
        });

        return Created("", new { history.Id, history.Query, history.SearchedAt });
    }

    /// <summary>
    /// Clear all search history for a specific user.
    /// </summary>
    /// <param name="userId">The user ID whose history to clear.</param>
    [HttpDelete("history/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearHistory(int userId)
    {
        await _search.ClearUserHistoryAsync(userId);
        return NoContent();
    }
}

/// <summary>
/// Request body for adding a search history entry.
/// </summary>
/// <param name="UserId">The user ID.</param>
/// <param name="Query">The search query text.</param>
public record AddSearchHistoryRequest(int UserId, string Query);
