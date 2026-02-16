using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class EssenceScraper : BaseSiteScraper
{
    public override string SourceName => "Essence";
    protected override string BaseUrl => "https://www.essence.com";
    protected override string PagePath => "/beauty/hair/";

    public EssenceScraper(
        IBrowserService browserService,
        ILogger<EssenceScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .card, .content-card, .post-item",
            titleSelector: "h2, h3, .card-title, .entry-title",
            summarySelector: ".card-description, .excerpt, .entry-excerpt",
            imageSelector: "img");
    }
}
