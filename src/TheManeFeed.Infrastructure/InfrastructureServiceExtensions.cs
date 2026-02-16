using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheManeFeed.Core.Interfaces;
using TheManeFeed.Infrastructure.Data;
using TheManeFeed.Infrastructure.Repositories;

namespace TheManeFeed.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TheManeFeedDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISavedArticleRepository, SavedArticleRepository>();
        services.AddScoped<ICollectionRepository, CollectionRepository>();
        services.AddScoped<ISearchRepository, SearchRepository>();

        return services;
    }
}
