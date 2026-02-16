using System.Text.Json;
using TheManeFeed.Cli.Models;
using TheManeFeed.Cli.Services.Scrapers;
using TheManeFeed.Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace TheManeFeed.Tests.Integration;

public abstract class ScraperTestBase : IClassFixture<PlaywrightFixture>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private static readonly string ResultsDir = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "test-results"));

    protected readonly PlaywrightFixture Fixture;
    protected readonly ITestOutputHelper Output;

    protected ScraperTestBase(PlaywrightFixture fixture, ITestOutputHelper output)
    {
        Fixture = fixture;
        Output = output;
    }

    protected abstract ISiteScraper CreateScraper();

    protected async Task RunScraperTest()
    {
        var scraper = CreateScraper();

        Output.WriteLine($"Starting scrape for {scraper.SourceName}...");
        var results = await scraper.ScrapeAsync();
        Output.WriteLine($"Scraped {results.Count} articles from {scraper.SourceName}.");

        // Write results to JSON for Agent 2 verification
        Directory.CreateDirectory(ResultsDir);
        var jsonPath = Path.Combine(ResultsDir, $"{scraper.SourceName}.json");
        var json = JsonSerializer.Serialize(results, JsonOptions);
        await File.WriteAllTextAsync(jsonPath, json);
        Output.WriteLine($"Results written to {jsonPath}");

        // Assertions
        Assert.NotEmpty(results);

        foreach (var result in results)
        {
            Assert.False(string.IsNullOrWhiteSpace(result.Title),
                $"Article has empty Title. Url: {result.Url}");
            Assert.False(string.IsNullOrWhiteSpace(result.Url),
                $"Article has empty Url. Title: {result.Title}");
            Assert.False(string.IsNullOrWhiteSpace(result.SourceName),
                $"Article has empty SourceName. Title: {result.Title}");
            Assert.StartsWith("http", result.Url);
        }
    }
}
