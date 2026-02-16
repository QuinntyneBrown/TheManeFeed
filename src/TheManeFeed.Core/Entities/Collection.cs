namespace TheManeFeed.Core.Entities;

public class Collection
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<CollectionArticle> CollectionArticles { get; set; } = new List<CollectionArticle>();
}
