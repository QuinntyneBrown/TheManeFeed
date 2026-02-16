using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Cli.Services;
using Xunit;

namespace TheManeFeed.Tests.Integration.Fixtures;

public class PlaywrightFixture : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private BrowserService? _browserService;

    public IBrowserService BrowserService => _browserService!;

    public IOptions<ScrapeSettings> ScrapeSettingsOptions { get; } =
        Options.Create(new ScrapeSettings
        {
            TimeoutSeconds = 60,
            MaxArticlesPerSource = 50,
            Headless = true
        });

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var logger = LoggerFactory
            .Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Information))
            .CreateLogger<BrowserService>();

        _browserService = new BrowserService(logger, ScrapeSettingsOptions);
    }

    public async Task DisposeAsync()
    {
        if (_browserService is not null)
            await _browserService.DisposeAsync();

        if (_browser is not null)
            await _browser.DisposeAsync();

        _playwright?.Dispose();
    }
}

[CollectionDefinition("Playwright")]
public class PlaywrightCollection : ICollectionFixture<PlaywrightFixture>
{
}
