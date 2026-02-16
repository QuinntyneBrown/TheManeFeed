namespace TheManeFeed.Core.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Color { get; set; }
    public int DisplayOrder { get; set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();
    public ICollection<UserInterest> InterestedUsers { get; set; } = new List<UserInterest>();
}
