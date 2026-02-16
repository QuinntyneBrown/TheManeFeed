using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheManeFeed.Cli.Services;

namespace TheManeFeed.Cli.Commands;

public class FeedCommand : Command
{
    public FeedCommand() : base("feed", "Fetch articles via RSS/Atom feeds from sources that support them")
    {
        var sourceOption = new Option<string?>(
            aliases: new[] { "--source", "-s" },
            description: "Specific source to fetch (e.g., WigsCom, PerfectLocks). Omit to fetch all RSS-enabled sources.");

        AddOption(sourceOption);

        this.SetHandler(async (string? source, IServiceProvider sp) =>
        {
            using var scope = sp.CreateScope();
            var feedService = scope.ServiceProvider.GetRequiredService<IRssFeedService>();
            var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<FeedCommand>>();

            logger.LogInformation("Fetching RSS feeds{Source}...",
                source is not null ? $" for {source}" : " for all RSS-enabled sources");

            var results = await feedService.FetchAllFeedsAsync(source);

            if (results.Count == 0)
            {
                Console.WriteLine("No articles found from RSS feeds.");
                return;
            }

            var saved = await articleService.SaveArticlesAsync(results);
            Console.WriteLine($"Feed fetch complete: {results.Count} found, {saved} new articles saved.");
        }, sourceOption, new ServiceProviderBinder());
    }
}
