using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class UtressScraper : BaseSiteScraper
{
    public override string SourceName => "Utress";
    protected override string BaseUrl => "https://blog.utress.com";
    protected override string PagePath => "/best-human-hair-wigs/";

    public UtressScraper(
        IBrowserService browserService,
        ILogger<UtressScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        // WordPress blog layout
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .post, .entry, .blog-post",
            titleSelector: "h2 a, h3 a, .entry-title a, .post-title a",
            summarySelector: ".entry-content p, .entry-summary p, .excerpt, .post-excerpt",
            imageSelector: "img, .post-thumbnail img, .wp-post-image");
    }
}
