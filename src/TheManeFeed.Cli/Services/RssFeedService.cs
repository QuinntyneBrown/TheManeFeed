using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.Logging;
using TheManeFeed.Cli.Models;
using TheManeFeed.Cli.Services.Scrapers;

namespace TheManeFeed.Cli.Services;

public class RssFeedService : IRssFeedService
{
    private readonly IEnumerable<ISiteScraper> _scrapers;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RssFeedService> _logger;

    public RssFeedService(
        IEnumerable<ISiteScraper> scrapers,
        IHttpClientFactory httpClientFactory,
        ILogger<RssFeedService> logger)
    {
        _scrapers = scrapers;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<ScrapeResult>> FetchAllFeedsAsync(string? sourceName = null)
    {
        var scrapers = _scrapers
            .Where(s => s.RssFeedPath is not null)
            .Where(s => sourceName is null ||
                        s.SourceName.Contains(sourceName, StringComparison.OrdinalIgnoreCase));

        var results = new List<ScrapeResult>();

        foreach (var scraper in scrapers)
        {
            var feedUrl = $"{scraper.BaseUrl}{scraper.RssFeedPath}";

            _logger.LogInformation("Fetching RSS feed for {Source} from {Url}", scraper.SourceName, feedUrl);

            var feedResults = await FetchFeedAsync(feedUrl, scraper.SourceName);
            results.AddRange(feedResults);
        }

        return results;
    }

    public async Task<List<ScrapeResult>> FetchFeedAsync(string feedUrl, string sourceName)
    {
        var results = new List<ScrapeResult>();

        try
        {
            var client = _httpClientFactory.CreateClient("RssFeed");
            using var response = await client.GetAsync(feedUrl);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = XmlReader.Create(stream);
            var feed = SyndicationFeed.Load(reader);

            if (feed is null)
            {
                _logger.LogWarning("No feed data returned from {Url}", feedUrl);
                return results;
            }

            foreach (var item in feed.Items)
            {
                var link = item.Links.FirstOrDefault(l => l.RelationshipType == "alternate")?.Uri?.AbsoluteUri
                           ?? item.Links.FirstOrDefault()?.Uri?.AbsoluteUri
                           ?? item.Id;

                if (string.IsNullOrWhiteSpace(link))
                    continue;

                var title = item.Title?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(title))
                    continue;

                var summary = item.Summary?.Text is not null
                    ? StripHtml(item.Summary.Text).Trim()
                    : null;

                // Truncate long summaries
                if (summary is not null && summary.Length > 500)
                    summary = summary[..497] + "...";

                var imageUrl = ExtractImageFromContent(item);

                var publishedAt = item.PublishDate != DateTimeOffset.MinValue
                    ? item.PublishDate.UtcDateTime
                    : item.LastUpdatedTime != DateTimeOffset.MinValue
                        ? item.LastUpdatedTime.UtcDateTime
                        : (DateTime?)null;

                results.Add(new ScrapeResult
                {
                    Title = title,
                    Url = link,
                    Summary = summary,
                    ImageUrl = imageUrl,
                    SourceName = sourceName,
                    PublishedAt = publishedAt,
                    CategoryTags = "hair"
                });
            }

            _logger.LogInformation("Fetched {Count} articles from RSS feed for {Source}", results.Count, sourceName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch RSS feed from {Url} for {Source}", feedUrl, sourceName);
        }

        return results;
    }

    private static string? ExtractImageFromContent(SyndicationItem item)
    {
        // Try to extract image from content or summary HTML
        var html = (item.Content as TextSyndicationContent)?.Text
                   ?? item.Summary?.Text;

        if (string.IsNullOrWhiteSpace(html))
            return null;

        var match = Regex.Match(html, @"<img[^>]+src=[""']([^""']+)[""']", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string StripHtml(string html)
    {
        var text = Regex.Replace(html, @"<[^>]+>", " ");
        text = Regex.Replace(text, @"\s+", " ");
        return System.Net.WebUtility.HtmlDecode(text).Trim();
    }

}
