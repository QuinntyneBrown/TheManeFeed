namespace TheManeFeed.Core.Entities;

public class CollectionArticle
{
    public int Id { get; set; }
    public int CollectionId { get; set; }
    public int ArticleId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public Collection Collection { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
