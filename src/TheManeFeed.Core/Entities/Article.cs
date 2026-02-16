namespace TheManeFeed.Core.Entities;

public class Article
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Body { get; set; }
    public string? ImageUrl { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public int? AuthorId { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime ScrapedAt { get; set; } = DateTime.UtcNow;
    public int ReadCount { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsTrending { get; set; }

    public Category? Category { get; set; }
    public Author? Author { get; set; }
    public ICollection<SavedArticle> SavedByUsers { get; set; } = new List<SavedArticle>();
    public ICollection<CollectionArticle> CollectionArticles { get; set; } = new List<CollectionArticle>();
}
