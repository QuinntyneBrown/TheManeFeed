using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class PerfectLocksScraper : BaseSiteScraper
{
    public override string SourceName => "PerfectLocks";
    protected override string BaseUrl => "https://www.perfectlocks.com";
    protected override string PagePath => "/blogs/all-tressed-up";

    public PerfectLocksScraper(
        IBrowserService browserService,
        ILogger<PerfectLocksScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .blog-post, .article-card, .blog-listing__item",
            titleSelector: "h2, h3, .article-card__title, .blog-post__title, .h3",
            summarySelector: ".article-card__excerpt, .blog-post__excerpt, .rte p",
            imageSelector: "img");
    }
}
