using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Configuration;
using TheManeFeed.Models;

namespace TheManeFeed.Services.Scrapers;

public class AllureScraper : BaseSiteScraper
{
    public override string SourceName => "Allure";
    protected override string BaseUrl => "https://www.allure.com";
    protected override string PagePath => "/topic/hair";

    public AllureScraper(
        IBrowserService browserService,
        ILogger<AllureScraper> logger,
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
