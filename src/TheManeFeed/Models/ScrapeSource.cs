namespace TheManeFeed.Models;

public class ScrapeSource
{
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string ArticleSelector { get; set; } = string.Empty;
    public string TitleSelector { get; set; } = string.Empty;
    public string? SummarySelector { get; set; }
    public string? ImageSelector { get; set; }
    public string? LinkSelector { get; set; }
}
