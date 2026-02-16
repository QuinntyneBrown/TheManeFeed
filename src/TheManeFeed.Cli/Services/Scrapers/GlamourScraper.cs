using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class GlamourScraper : BaseSiteScraper
{
    public override string SourceName => "Glamour";
    protected override string BaseUrl => "https://www.glamour.com";
    protected override string PagePath => "/lipstick/hair";

    public GlamourScraper(
        IBrowserService browserService,
        ILogger<GlamourScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "[class*='SummaryItemWrapper']",
            titleSelector: "[class*='SummaryItemHedLink'], [class*='summary-item__hed']",
            linkSelector: "a[href*='/story/'], a[href*='/gallery/']",
            summarySelector: "[class*='SummaryItemDek'], [class*='summary-item__dek']",
            imageSelector: "img");
    }
}
