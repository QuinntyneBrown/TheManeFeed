using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheManeFeed.Cli.Services;
using TheManeFeed.Cli.Services.Scrapers;
using TheManeFeed.Infrastructure;

namespace TheManeFeed.Cli.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTheManeFeed(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ScrapeSettings>(
            configuration.GetSection(ScrapeSettings.SectionName));

        services.AddInfrastructure(configuration);

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
        services.AddScoped<ISiteScraper, WigsComScraper>();
        services.AddScoped<ISiteScraper, PerfectLocksScraper>();
        services.AddScoped<ISiteScraper, UtressScraper>();
        services.AddScoped<ISiteScraper, LaurenAshtynScraper>();
        services.AddScoped<ISiteScraper, ExpressWigBraidsScraper>();
        services.AddScoped<ISiteScraper, LuvmeHairScraper>();
        services.AddScoped<ISiteScraper, MilanoWigsScraper>();

        return services;
    }
}
