using TheManeFeed.Models;

namespace TheManeFeed.Services;

public interface IScraperService
{
    Task<List<ScrapeResult>> ScrapeAsync(string? sourceName = null);
}
