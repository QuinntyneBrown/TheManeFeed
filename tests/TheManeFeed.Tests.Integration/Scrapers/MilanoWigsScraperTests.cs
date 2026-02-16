using Microsoft.Extensions.Logging;
using TheManeFeed.Cli.Services.Scrapers;
using TheManeFeed.Tests.Integration.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace TheManeFeed.Tests.Integration.Scrapers;

[Collection("Playwright")]
public class MilanoWigsScraperTests : ScraperTestBase
{
    public MilanoWigsScraperTests(PlaywrightFixture fixture, ITestOutputHelper output)
        : base(fixture, output) { }

    protected override ISiteScraper CreateScraper() =>
        new MilanoWigsScraper(
            Fixture.BrowserService,
            LoggerFactory.Create(b => b.AddConsole()).CreateLogger<MilanoWigsScraper>(),
            Fixture.ScrapeSettingsOptions);

    [Fact]
    [Trait("Category", "Integration")]
    public async Task ScrapeAsync_ReturnsValidArticles() => await RunScraperTest();
}
