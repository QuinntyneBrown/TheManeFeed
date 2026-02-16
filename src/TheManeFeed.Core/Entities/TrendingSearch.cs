namespace TheManeFeed.Core.Entities;

public class TrendingSearch
{
    public int Id { get; set; }
    public string Query { get; set; } = string.Empty;
    public int SearchCount { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
