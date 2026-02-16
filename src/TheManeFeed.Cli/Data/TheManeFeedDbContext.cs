using Microsoft.EntityFrameworkCore;
using TheManeFeed.Cli.Models;

namespace TheManeFeed.Cli.Data;

public class TheManeFeedDbContext : DbContext
{
    public TheManeFeedDbContext(DbContextOptions<TheManeFeedDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Articles => Set<Article>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TheManeFeedDbContext).Assembly);
    }
}
