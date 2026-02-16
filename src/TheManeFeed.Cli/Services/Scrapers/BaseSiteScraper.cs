using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Services.Scrapers;

public abstract class BaseSiteScraper : ISiteScraper
{
    protected readonly IBrowserService BrowserService;
    protected readonly ILogger Logger;
    protected readonly ScrapeSettings Settings;

    public abstract string SourceName { get; }
    protected abstract string BaseUrl { get; }
    protected abstract string PagePath { get; }

    protected BaseSiteScraper(
        IBrowserService browserService,
        ILogger logger,
        IOptions<ScrapeSettings> settings)
    {
        BrowserService = browserService;
        Logger = logger;
        Settings = settings.Value;
    }

    public async Task<List<ScrapeResult>> ScrapeAsync()
    {
        var results = new List<ScrapeResult>();

        var browser = await BrowserService.GetBrowserAsync();
        var page = await browser.NewPageAsync();

        try
        {
            var url = $"{BaseUrl}{PagePath}";
            Logger.LogInformation("Navigating to {Url}", url);

            await page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = Settings.TimeoutSeconds * 1000
            });

            await page.WaitForTimeoutAsync(2000);

            results = await ExtractArticlesAsync(page);

            if (results.Count > Settings.MaxArticlesPerSource)
                results = results.Take(Settings.MaxArticlesPerSource).ToList();

            Logger.LogInformation("Extracted {Count} articles from {Source}", results.Count, SourceName);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to scrape {Source}", SourceName);
        }
        finally
        {
            await page.CloseAsync();
        }

        return results;
    }

    protected abstract Task<List<ScrapeResult>> ExtractArticlesAsync(IPage page);

    protected string ResolveUrl(string href)
    {
        if (string.IsNullOrWhiteSpace(href))
            return string.Empty;

        if (href.StartsWith("http"))
            return href;

        return $"{BaseUrl}{(href.StartsWith('/') ? "" : "/")}{href}";
    }

    protected async Task<List<ScrapeResult>> ExtractWithSelectorsAsync(
        IPage page,
        string articleSelector,
        string titleSelector,
        string? linkSelector = null,
        string? summarySelector = null,
        string? imageSelector = null)
    {
        var results = new List<ScrapeResult>();
        var articles = await page.QuerySelectorAllAsync(articleSelector);

        foreach (var article in articles)
        {
            try
            {
                var titleEl = await article.QuerySelectorAsync(titleSelector);
                var title = titleEl is not null ? (await titleEl.InnerTextAsync()).Trim() : null;

                if (string.IsNullOrWhiteSpace(title))
                    continue;

                var href = string.Empty;
                var linkEl = linkSelector is not null
                    ? await article.QuerySelectorAsync(linkSelector)
                    : await article.QuerySelectorAsync("a[href]");

                if (linkEl is not null)
                    href = await linkEl.GetAttributeAsync("href") ?? string.Empty;

                if (string.IsNullOrWhiteSpace(href))
                    continue;

                string? summary = null;
                if (summarySelector is not null)
                {
                    var summaryEl = await article.QuerySelectorAsync(summarySelector);
                    summary = summaryEl is not null ? (await summaryEl.InnerTextAsync()).Trim() : null;
                }

                string? imageUrl = null;
                if (imageSelector is not null)
                {
                    var imgEl = await article.QuerySelectorAsync(imageSelector);
                    if (imgEl is not null)
                    {
                        imageUrl = await imgEl.GetAttributeAsync("src")
                                   ?? await imgEl.GetAttributeAsync("data-src")
                                   ?? await imgEl.GetAttributeAsync("data-lazy-src");

                        // Fall back to srcset (first entry)
                        if (string.IsNullOrWhiteSpace(imageUrl) || imageUrl.Contains("data:image"))
                        {
                            var srcset = await imgEl.GetAttributeAsync("srcset")
                                         ?? await imgEl.GetAttributeAsync("data-srcset");
                            if (!string.IsNullOrWhiteSpace(srcset))
                            {
                                imageUrl = srcset.Split(',')[0].Trim().Split(' ')[0];
                            }
                        }

                        // Skip data URIs / blank placeholders
                        if (imageUrl is not null && imageUrl.StartsWith("data:"))
                            imageUrl = null;
                    }
                }

                results.Add(new ScrapeResult
                {
                    Title = title,
                    Url = ResolveUrl(href),
                    Summary = summary,
                    ImageUrl = imageUrl is not null ? ResolveUrl(imageUrl) : null,
                    SourceName = SourceName,
                    CategoryTags = "hair"
                });
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex, "Error extracting article element from {Source}", SourceName);
            }
        }

        return results;
    }
}
