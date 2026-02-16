using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public interface ISiteScraper
{
    string SourceName { get; }
    Task<List<ScrapeResult>> ScrapeAsync();
}
