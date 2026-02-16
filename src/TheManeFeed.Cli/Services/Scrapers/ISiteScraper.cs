using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public interface ISiteScraper
{
    string SourceName { get; }
    string BaseUrl { get; }
    string? RssFeedPath { get; }
    Task<List<ScrapeResult>> ScrapeAsync();
}
