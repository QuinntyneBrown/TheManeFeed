using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class WigsComScraper : BaseSiteScraper
{
    public override string SourceName => "WigsCom";
    public override string RssFeedPath => "/blogs/news.atom";
    public override string BaseUrl => "https://www.wigs.com";
    protected override string PagePath => "/blogs/news";

    public WigsComScraper(
        IBrowserService browserService,
        ILogger<WigsComScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .blog-post, .article-card, .blog-listing__item",
            titleSelector: "h2, h3, .article-card__title, .blog-post__title",
            summarySelector: ".article-card__excerpt, .blog-post__excerpt, .rte p",
            imageSelector: "img");
    }
}
