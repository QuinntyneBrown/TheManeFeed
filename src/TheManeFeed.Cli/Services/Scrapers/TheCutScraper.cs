using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class TheCutScraper : BaseSiteScraper
{
    public override string SourceName => "TheCut";
    public override string BaseUrl => "https://www.thecut.com";
    protected override string PagePath => "/tags/hair/";

    public TheCutScraper(
        IBrowserService browserService,
        ILogger<TheCutScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: ".paginated-feed li.article",
            titleSelector: "h2, h3, [class*='headline']",
            linkSelector: "a[href]",
            summarySelector: ".teaser, [class*='teaser'], .dek",
            imageSelector: "picture img, img");
    }
}
