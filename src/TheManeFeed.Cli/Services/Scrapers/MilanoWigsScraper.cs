using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class MilanoWigsScraper : BaseSiteScraper
{
    public override string SourceName => "MilanoWigs";
    public override string RssFeedPath => "/blogs/wig-talk-blog.atom";
    public override string BaseUrl => "https://milanowigs.com";
    protected override string PagePath => "/blogs/wig-talk-blog";

    public MilanoWigsScraper(
        IBrowserService browserService,
        ILogger<MilanoWigsScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        // Shopify blog layout - uses .blog__post-item with h3.blog__post-title
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: ".blog__post-item",
            titleSelector: "h3.blog__post-title a, h3 a",
            linkSelector: "h3.blog__post-title a",
            summarySelector: ".blog__post-excerpt, .rte p",
            imageSelector: "img");
    }
}
