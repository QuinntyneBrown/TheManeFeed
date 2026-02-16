namespace TheManeFeed.Cli.Models;

public class ScrapeResult
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? ImageUrl { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public string? CategoryTags { get; set; }
}
