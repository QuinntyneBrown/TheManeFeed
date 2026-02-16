using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class ByrdieScraper : BaseSiteScraper
{
    public override string SourceName => "Byrdie";
    public override string BaseUrl => "https://www.byrdie.com";
    protected override string PagePath => "/hair-4628407";

    public ByrdieScraper(
        IBrowserService browserService,
        ILogger<ByrdieScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        var results = new List<ScrapeResult>();
        var cards = await page.QuerySelectorAllAsync("a.mntl-card-list-items");

        foreach (var card in cards)
        {
            try
            {
                var titleEl = await card.QuerySelectorAsync(".card__title-text");
                var title = titleEl is not null ? (await titleEl.InnerTextAsync()).Trim() : null;
                if (string.IsNullOrWhiteSpace(title)) continue;

                var href = await card.GetAttributeAsync("href") ?? string.Empty;
                if (string.IsNullOrWhiteSpace(href)) continue;

                var imgEl = await card.QuerySelectorAsync("img");
                string? imageUrl = null;
                if (imgEl is not null)
                {
                    imageUrl = await imgEl.GetAttributeAsync("data-src")
                               ?? await imgEl.GetAttributeAsync("src");
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
