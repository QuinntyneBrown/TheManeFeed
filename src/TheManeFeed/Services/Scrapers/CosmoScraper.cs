using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Configuration;
using TheManeFeed.Models;

namespace TheManeFeed.Services.Scrapers;

public class CosmoScraper : BaseSiteScraper
{
    public override string SourceName => "Cosmopolitan";
    protected override string BaseUrl => "https://www.cosmopolitan.com";
    protected override string PagePath => "/style-beauty/beauty/g/hair-ideas/";

    public CosmoScraper(
        IBrowserService browserService,
        ILogger<CosmoScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: "article, .full-item, .simple-item, .listicle-slide",
            titleSelector: "h2, h3, .full-item-title, .listicle-slide-hed",
            summarySelector: ".full-item-dek, .excerpt, .listicle-slide-dek",
            imageSelector: "img");
    }
}
