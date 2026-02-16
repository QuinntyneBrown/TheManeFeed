using Microsoft.Playwright;

namespace TheManeFeed.Services;

public interface IBrowserService : IAsyncDisposable
{
    Task<IBrowser> GetBrowserAsync();
}
