using Microsoft.EntityFrameworkCore;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Infrastructure.Data;

public class TheManeFeedDbContext : DbContext
{
    public TheManeFeedDbContext(DbContextOptions<TheManeFeedDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<User> Users => Set<User>();
    public DbSet<SavedArticle> SavedArticles => Set<SavedArticle>();
    public DbSet<UserInterest> UserInterests => Set<UserInterest>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionArticle> CollectionArticles => Set<CollectionArticle>();
    public DbSet<SearchHistory> SearchHistories => Set<SearchHistory>();
    public DbSet<TrendingSearch> TrendingSearches => Set<TrendingSearch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TheManeFeedDbContext).Assembly);
    }
}
