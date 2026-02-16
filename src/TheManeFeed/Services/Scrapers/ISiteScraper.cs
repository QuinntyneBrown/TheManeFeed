using TheManeFeed.Models;

namespace TheManeFeed.Services.Scrapers;

public interface ISiteScraper
{
    string SourceName { get; }
    Task<List<ScrapeResult>> ScrapeAsync();
}
