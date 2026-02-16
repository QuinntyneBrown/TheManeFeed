using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class ByrdieScraper : BaseSiteScraper
{
    public override string SourceName => "Byrdie";
    protected override string BaseUrl => "https://www.byrdie.com";
    protected override string PagePath => "/hair-4843568";

    public ByrdieScraper(
        IBrowserService browserService,
        ILogger<ByrdieScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .card, .comp, .mntl-card",
            titleSelector: "h2, h3, .card__title, .mntl-card__title",
            summarySelector: ".card__description, .excerpt, .mntl-card__description",
            imageSelector: "img");
    }
}
