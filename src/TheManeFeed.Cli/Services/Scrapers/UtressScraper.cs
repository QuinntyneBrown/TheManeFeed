using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class UtressScraper : BaseSiteScraper
{
    public override string SourceName => "Utress";
    public override string RssFeedPath => "/feed/";
    public override string BaseUrl => "https://blog.utress.com";
    protected override string PagePath => "/";

    public UtressScraper(
        IBrowserService browserService,
        ILogger<UtressScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        // WordPress Genesis theme - articles use a[title] for both image and text links
        var results = new List<ScrapeResult>();
        var articles = await page.QuerySelectorAllAsync("article.entry, article.post");

        foreach (var article in articles)
        {
            try
            {
                var linkEl = await article.QuerySelectorAsync("a[title]");
                if (linkEl is null) continue;

                var title = await linkEl.GetAttributeAsync("title");
                if (string.IsNullOrWhiteSpace(title)) continue;

                var href = await linkEl.GetAttributeAsync("href") ?? string.Empty;
                if (string.IsNullOrWhiteSpace(href)) continue;

                var imgEl = await article.QuerySelectorAsync("img");
                string? imageUrl = null;
                if (imgEl is not null)
                {
                    imageUrl = await imgEl.GetAttributeAsync("src")
                               ?? await imgEl.GetAttributeAsync("data-src");
                }

                results.Add(new ScrapeResult
                {
                    Title = title,
                    Url = ResolveUrl(href),
                    ImageUrl = imageUrl is not null ? ResolveUrl(imageUrl) : null,
                    SourceName = SourceName,
                    CategoryTags = "hair"
                });
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex, "Error extracting article from {Source}", SourceName);
            }
        }

        return results;
    }
}
