using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class LaurenAshtynScraper : BaseSiteScraper
{
    public override string SourceName => "LaurenAshtyn";
    protected override string BaseUrl => "https://thelaurenashtyncollection.com";
    protected override string PagePath => "/blogs/blog";

    public LaurenAshtynScraper(
        IBrowserService browserService,
        ILogger<LaurenAshtynScraper> logger,
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
