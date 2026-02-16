using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using TheManeFeed.Configuration;

namespace TheManeFeed.Services;

public class BrowserService : IBrowserService
{
    private readonly ILogger<BrowserService> _logger;
    private readonly ScrapeSettings _settings;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public BrowserService(ILogger<BrowserService> logger, IOptions<ScrapeSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task<IBrowser> GetBrowserAsync()
    {
        if (_browser is not null)
            return _browser;

        await _semaphore.WaitAsync();
        try
        {
            if (_browser is not null)
                return _browser;

            _logger.LogInformation("Launching Playwright browser (headless: {Headless})", _settings.Headless);
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless
            });

            return _browser;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.DisposeAsync();
            _browser = null;
        }

        _playwright?.Dispose();
        _playwright = null;

        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
