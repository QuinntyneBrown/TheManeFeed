namespace TheManeFeed.Models;

public class Article
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public string? CategoryTags { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;
}
