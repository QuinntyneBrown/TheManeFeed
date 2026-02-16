using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using TheManeFeed.Cli.Services;

namespace TheManeFeed.Cli.Commands;

public class ListCommand : Command
{
    public ListCommand() : base("list", "List stored articles")
    {
        var sourceOption = new Option<string?>(
            aliases: new[] { "--source", "-s" },
            description: "Filter by source name");

        var limitOption = new Option<int>(
            aliases: new[] { "--limit", "-l" },
            getDefaultValue: () => 20,
            description: "Maximum number of articles to display");

        AddOption(sourceOption);
        AddOption(limitOption);

        this.SetHandler(async (string? source, int limit, IServiceProvider sp) =>
        {
            using var scope = sp.CreateScope();
            var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();

            var articles = await articleService.GetArticlesAsync(source, limit);

            if (articles.Count == 0)
            {
                Console.WriteLine("No articles found.");
                return;
            }

            Console.WriteLine($"{"Source",-18} {"Title",-60} {"Scraped",-12}");
            Console.WriteLine(new string('-', 92));

            foreach (var article in articles)
            {
                var title = article.Title.Length > 57
                    ? article.Title[..57] + "..."
                    : article.Title;

                Console.WriteLine($"{article.SourceName,-18} {title,-60} {article.ScrapedAt:yyyy-MM-dd}");
            }

            Console.WriteLine($"\nTotal: {articles.Count} article(s)");
        }, sourceOption, limitOption, new ServiceProviderBinder());
    }
}
