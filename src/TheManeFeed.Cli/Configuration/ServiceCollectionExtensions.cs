using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheManeFeed.Cli.Data;
using TheManeFeed.Cli.Services;
using TheManeFeed.Cli.Services.Scrapers;

namespace TheManeFeed.Cli.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTheManeFeed(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ScrapeSettings>(
            configuration.GetSection(ScrapeSettings.SectionName));

        services.AddDbContext<TheManeFeedDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddSingleton<IBrowserService, BrowserService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IScraperService, ScraperService>();

        services.AddScoped<ISiteScraper, EssenceScraper>();
        services.AddScoped<ISiteScraper, NaturallyCurlyScraper>();
        services.AddScoped<ISiteScraper, TheCutScraper>();
        services.AddScoped<ISiteScraper, BetScraper>();
        services.AddScoped<ISiteScraper, AllureScraper>();
        services.AddScoped<ISiteScraper, CosmoScraper>();
        services.AddScoped<ISiteScraper, ByrdieScraper>();
        services.AddScoped<ISiteScraper, GlamourScraper>();

        return services;
    }
}
