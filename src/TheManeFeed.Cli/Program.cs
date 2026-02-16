using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TheManeFeed.Cli.Commands;
using TheManeFeed.Cli.Configuration;
using TheManeFeed.Infrastructure.Data;
using TheManeFeed.Cli.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    var services = new ServiceCollection();
    services.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddSerilog(dispose: true);
    });
    services.AddSingleton<IConfiguration>(configuration);
    services.AddTheManeFeed(configuration);

    var serviceProvider = services.BuildServiceProvider();

    // Auto-migrate database
    try
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TheManeFeedDbContext>();
        await db.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Could not auto-migrate database. Ensure SQL Server/LocalDB is available");
    }

    var rootCommand = new RootCommand("TheManeFeed - Curated hair & beauty news aggregator");
    rootCommand.AddCommand(new ScrapeCommand());
    rootCommand.AddCommand(new ListCommand());

    var parser = new CommandLineBuilder(rootCommand)
        .UseDefaults()
        .AddMiddleware(async (context, next) =>
        {
            context.BindingContext.AddService<IServiceProvider>(_ => serviceProvider);
            await next(context);
        })
        .Build();

    var result = await parser.InvokeAsync(args);

    // Dispose browser
    if (serviceProvider.GetService<IBrowserService>() is IAsyncDisposable disposable)
        await disposable.DisposeAsync();

    await serviceProvider.DisposeAsync();
    return result;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
