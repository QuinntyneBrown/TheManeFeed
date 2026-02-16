using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TheManeFeed.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TheManeFeedDbContext>
{
    public TheManeFeedDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TheManeFeedDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new TheManeFeedDbContext(optionsBuilder.Options);
    }
}
