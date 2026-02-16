using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Configuration;
using TheManeFeed.Models;

namespace TheManeFeed.Services.Scrapers;

public class GlamourScraper : BaseSiteScraper
{
    public override string SourceName => "Glamour";
    protected override string BaseUrl => "https://www.glamour.com";
    protected override string PagePath => "/topic/hair";

    public GlamourScraper(
        IBrowserService browserService,
        ILogger<GlamourScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .summary-item, .card, .summary-list__item",
            titleSelector: "h2, h3, .summary-item__hed, .card-title",
            summarySelector: ".summary-item__dek, .excerpt, .card-dek",
            imageSelector: "img");
    }
}
