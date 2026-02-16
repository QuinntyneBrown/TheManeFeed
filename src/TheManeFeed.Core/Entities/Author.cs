namespace TheManeFeed.Core.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}
