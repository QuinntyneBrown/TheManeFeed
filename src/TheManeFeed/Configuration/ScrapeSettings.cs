using TheManeFeed.Models;

namespace TheManeFeed.Configuration;

public class ScrapeSettings
{
    public const string SectionName = "ScrapeSettings";

    public int TimeoutSeconds { get; set; } = 30;
    public int MaxArticlesPerSource { get; set; } = 20;
    public bool Headless { get; set; } = true;
    public List<ScrapeSource> Sources { get; set; } = new();
}
