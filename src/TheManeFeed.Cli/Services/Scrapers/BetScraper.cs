using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class BetScraper : BaseSiteScraper
{
    public override string SourceName => "BET";
    public override string BaseUrl => "https://www.bet.com";
    protected override string PagePath => "/tag/bopy0s/beauty";

    public BetScraper(
        IBrowserService browserService,
        ILogger<BetScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: ".item.article",
            titleSelector: "h2 span",
            linkSelector: "a[href]",
            summarySelector: ".description",
            imageSelector: "img");
    }
}
