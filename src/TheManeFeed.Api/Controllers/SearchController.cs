using Microsoft.AspNetCore.Mvc;
using TheManeFeed.Core.Entities;
using TheManeFeed.Core.Interfaces;

namespace TheManeFeed.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchRepository _search;
    private readonly IArticleRepository _articles;

    public SearchController(ISearchRepository search, IArticleRepository articles)
    {
        _search = search;
        _articles = articles;
    }

    [HttpGet]
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

    [HttpGet("trending")]
    public async Task<IActionResult> GetTrending([FromQuery] int limit = 10)
    {
        var trending = await _search.GetTrendingSearchesAsync(limit);
        return Ok(trending.Select(t => new { t.Query, t.SearchCount }));
    }

    [HttpGet("history/{userId:int}")]
    public async Task<IActionResult> GetHistory(int userId, [FromQuery] int limit = 10)
    {
        var history = await _search.GetUserHistoryAsync(userId, limit);
        return Ok(history.Select(h => new { h.Query, h.SearchedAt }));
    }

    [HttpPost("history")]
    public async Task<IActionResult> AddHistory([FromBody] AddSearchHistoryRequest request)
    {
        var history = await _search.AddHistoryAsync(new SearchHistory
        {
            UserId = request.UserId,
            Query = request.Query
        });

        return Created("", new { history.Id, history.Query, history.SearchedAt });
    }

    [HttpDelete("history/{userId:int}")]
    public async Task<IActionResult> ClearHistory(int userId)
    {
        await _search.ClearUserHistoryAsync(userId);
        return NoContent();
    }
}

public record AddSearchHistoryRequest(int UserId, string Query);
