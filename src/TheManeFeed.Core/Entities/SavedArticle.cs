namespace TheManeFeed.Core.Entities;

public class SavedArticle
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArticleId { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
