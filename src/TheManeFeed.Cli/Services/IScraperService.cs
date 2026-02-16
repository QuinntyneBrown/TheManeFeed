using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services;

public interface IScraperService
{
    Task<List<ScrapeResult>> ScrapeAsync(string? sourceName = null);
}
