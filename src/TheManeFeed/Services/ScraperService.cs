using Microsoft.Extensions.Logging;
using TheManeFeed.Models;
using TheManeFeed.Services.Scrapers;

namespace TheManeFeed.Services;

public class ScraperService : IScraperService
{
    private readonly IEnumerable<ISiteScraper> _scrapers;
    private readonly ILogger<ScraperService> _logger;

    public ScraperService(IEnumerable<ISiteScraper> scrapers, ILogger<ScraperService> logger)
    {
        _scrapers = scrapers;
        _logger = logger;
    }

    public async Task<List<ScrapeResult>> ScrapeAsync(string? sourceName = null)
    {
        var results = new List<ScrapeResult>();
        var scrapers = _scrapers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(sourceName))
        {
            scrapers = scrapers.Where(s =>
                s.SourceName.Equals(sourceName, StringComparison.OrdinalIgnoreCase));
        }

        var scraperList = scrapers.ToList();

        if (scraperList.Count == 0)
        {
            _logger.LogWarning("No scrapers found for source: {Source}", sourceName ?? "(all)");
            return results;
        }

        foreach (var scraper in scraperList)
        {
            _logger.LogInformation("Scraping {Source}...", scraper.SourceName);

            try
            {
                var scraped = await scraper.ScrapeAsync();
                results.AddRange(scraped);
                _logger.LogInformation("Found {Count} articles from {Source}", scraped.Count, scraper.SourceName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error scraping {Source}", scraper.SourceName);
            }
        }

        return results;
    }
}
