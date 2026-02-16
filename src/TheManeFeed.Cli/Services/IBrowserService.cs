using Microsoft.Playwright;

namespace TheManeFeed.Cli.Services;

public interface IBrowserService : IAsyncDisposable
{
    Task<IBrowser> GetBrowserAsync();
}
