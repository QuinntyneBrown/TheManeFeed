using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class TheCutScraper : BaseSiteScraper
{
    public override string SourceName => "TheCut";
    protected override string BaseUrl => "https://www.thecut.com";
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
            articleSelector: "article, .story, .feed-item, .river-item",
            titleSelector: "h2, h3, .story-title, .headline",
            summarySelector: ".story-summary, .excerpt, .dek",
            imageSelector: "img");
    }
}
