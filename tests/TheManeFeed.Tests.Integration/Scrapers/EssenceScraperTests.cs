using Microsoft.Extensions.Logging;
using TheManeFeed.Cli.Services.Scrapers;
using TheManeFeed.Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace TheManeFeed.Tests.Integration.Scrapers;

[Collection("Playwright")]
public class EssenceScraperTests : ScraperTestBase
{
    public EssenceScraperTests(PlaywrightFixture fixture, ITestOutputHelper output)
        : base(fixture, output) { }

    protected override ISiteScraper CreateScraper() =>
        new EssenceScraper(
            Fixture.BrowserService,
            LoggerFactory.Create(b => b.AddConsole()).CreateLogger<EssenceScraper>(),
            Fixture.ScrapeSettingsOptions);

    [Fact]
    [Trait("Category", "Integration")]
    public async Task ScrapeAsync_ReturnsValidArticles() => await RunScraperTest();
}
