using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class MilanoWigsScraper : BaseSiteScraper
{
    public override string SourceName => "MilanoWigs";
    protected override string BaseUrl => "https://milanowigs.com";
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
        // Shopify blog layout
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .blog-post, .article-card, .blog-listing__item, .blog__post",
            titleSelector: "h2, h3, .article-card__title, .blog-post__title, .h2",
            summarySelector: ".article-card__excerpt, .blog-post__excerpt, .rte p, .blog__post-excerpt",
            imageSelector: "img");
    }
}
