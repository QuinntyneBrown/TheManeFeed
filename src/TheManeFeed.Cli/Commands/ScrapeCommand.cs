using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheManeFeed.Cli.Services;

namespace TheManeFeed.Cli.Commands;

public class ScrapeCommand : Command
{
    public ScrapeCommand() : base("scrape", "Scrape hair/beauty articles from curated sources")
    {
        var sourceOption = new Option<string?>(
            aliases: new[] { "--source", "-s" },
            description: "Specific source to scrape (e.g., Essence, NaturallyCurly). Omit to scrape all.");

        AddOption(sourceOption);

        this.SetHandler(async (string? source, IServiceProvider sp) =>
        {
            using var scope = sp.CreateScope();
            var scraperService = scope.ServiceProvider.GetRequiredService<IScraperService>();
            var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ScrapeCommand>>();

            logger.LogInformation("Starting scrape{Source}...",
                source is not null ? $" for {source}" : " for all sources");

            var results = await scraperService.ScrapeAsync(source);

            if (results.Count == 0)
            {
                Console.WriteLine("No articles found.");
                return;
            }

            var saved = await articleService.SaveArticlesAsync(results);
            Console.WriteLine($"Scrape complete: {results.Count} found, {saved} new articles saved.");
        }, sourceOption, new ServiceProviderBinder());
    }
}

public class ServiceProviderBinder : System.CommandLine.Binding.BinderBase<IServiceProvider>
{
    protected override IServiceProvider GetBoundValue(System.CommandLine.Binding.BindingContext bindingContext)
    {
        return bindingContext.GetService(typeof(IServiceProvider)) as IServiceProvider
            ?? throw new InvalidOperationException("No service provider available");
    }
}
