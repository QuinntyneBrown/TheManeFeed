namespace TheManeFeed.Core.Entities;

public class UserInterest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }

    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
