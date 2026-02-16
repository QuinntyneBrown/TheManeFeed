using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services;

public interface IRssFeedService
{
    Task<List<ScrapeResult>> FetchFeedAsync(string feedUrl, string sourceName);
    Task<List<ScrapeResult>> FetchAllFeedsAsync(string? sourceName = null);
}
