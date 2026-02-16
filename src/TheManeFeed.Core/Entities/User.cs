namespace TheManeFeed.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SavedArticle> SavedArticles { get; set; } = new List<SavedArticle>();
    public ICollection<UserInterest> Interests { get; set; } = new List<UserInterest>();
    public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    public ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
}
