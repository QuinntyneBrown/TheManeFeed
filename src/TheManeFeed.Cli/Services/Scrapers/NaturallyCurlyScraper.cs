using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public class NaturallyCurlyScraper : BaseSiteScraper
{
    public override string SourceName => "NaturallyCurly";
    public override string BaseUrl => "https://www.naturallycurly.com";
    protected override string PagePath => "/curlreading";

    public NaturallyCurlyScraper(
        IBrowserService browserService,
        ILogger<NaturallyCurlyScraper> logger,
        IOptions<ScrapeSettings> settings)
        : base(browserService, logger, settings)
    {
    }

    protected override async Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page)
    {
        return await ExtractWithSelectorsAsync(
            page,
            articleSelector: ".article-card, .post-card, article, .story-card",
            titleSelector: "h2, h3, .title, .article-title",
            summarySelector: ".excerpt, .summary, .description",
            imageSelector: "img");
    }
}
