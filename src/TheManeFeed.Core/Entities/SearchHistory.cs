namespace TheManeFeed.Core.Entities;

public class SearchHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Query { get; set; } = string.Empty;
    public DateTime SearchedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}
