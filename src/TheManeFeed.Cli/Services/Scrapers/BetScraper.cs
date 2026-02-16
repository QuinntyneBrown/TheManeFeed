using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class BetScraper : BaseSiteScraper
{
    public override string SourceName => "BET";
    protected override string BaseUrl => "https://www.bet.com";
    protected override string PagePath => "/article/category/style-beauty";

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
            articleSelector: "article, .card, .content-card, .article-item",
            titleSelector: "h2, h3, .card-title, .article-title",
            summarySelector: ".card-description, .excerpt, .article-description",
            imageSelector: "img");
    }
}
