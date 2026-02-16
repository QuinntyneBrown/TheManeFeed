using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Infrastructure.Data.Configurations;

public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
{
    public void Configure(EntityTypeBuilder<UserInterest> builder)
    {
        builder.HasKey(ui => ui.Id);
        builder.HasIndex(ui => new { ui.UserId, ui.CategoryId }).IsUnique();

        builder.HasOne(ui => ui.User)
            .WithMany(u => u.Interests)
            .HasForeignKey(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ui => ui.Category)
            .WithMany(c => c.InterestedUsers)
            .HasForeignKey(ui => ui.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
