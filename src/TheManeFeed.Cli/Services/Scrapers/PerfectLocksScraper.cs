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
            articleSelector: ".article-item",
            titleSelector: "h3.article-item__title a, h3 a",
            linkSelector: "h3.article-item__title a, a[href*='blogs']",
            summarySelector: ".article-item__excerpt, .rte p",
            imageSelector: "img.article-item__image, img");
    }
}
