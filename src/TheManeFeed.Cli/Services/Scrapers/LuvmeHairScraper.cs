using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class LuvmeHairScraper : BaseSiteScraper
{
    public override string SourceName => "LuvmeHair";
    protected override string BaseUrl => "https://shop.luvmehair.com";
    protected override string PagePath => "/blogs/wigs-101";

    public LuvmeHairScraper(
        IBrowserService browserService,
        ILogger<LuvmeHairScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        var results = new List<ScrapeResult>();
        var cards = await page.QuerySelectorAllAsync("article.how-to-card");

        foreach (var card in cards)
        {
            try
            {
                var linkEl = await card.QuerySelectorAsync("a[href]");
                if (linkEl is null) continue;

                var href = await linkEl.GetAttributeAsync("href") ?? string.Empty;
                if (string.IsNullOrWhiteSpace(href)) continue;

                var title = await linkEl.GetAttributeAsync("data-classification");
                if (string.IsNullOrWhiteSpace(title))
                    title = (await linkEl.InnerTextAsync()).Trim();
                if (string.IsNullOrWhiteSpace(title)) continue;

                var imgEl = await card.QuerySelectorAsync("img");
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
