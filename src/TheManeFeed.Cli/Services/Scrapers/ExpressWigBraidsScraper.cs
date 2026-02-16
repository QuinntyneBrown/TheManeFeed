using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class ExpressWigBraidsScraper : BaseSiteScraper
{
    public override string SourceName => "ExpressWigBraids";
    public override string RssFeedPath => "/blogs/news.atom";
    public override string BaseUrl => "https://expresswigbraids.com";
    protected override string PagePath => "/blogs/news";

    public ExpressWigBraidsScraper(
        IBrowserService browserService,
        ILogger<ExpressWigBraidsScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        // Shopify blog layout - uses li.blog-item with h3.article-title
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "li.blog-item",
            titleSelector: "h3.article-title a, h3 a",
            linkSelector: "h3.article-title a",
            summarySelector: ".article-excerpt, .rte p",
            imageSelector: "img");
    }
}
