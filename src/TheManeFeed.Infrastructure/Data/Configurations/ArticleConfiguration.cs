using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Infrastructure.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Url).IsRequired().HasMaxLength(2048);
        builder.HasIndex(a => a.Url).IsUnique();

        builder.Property(a => a.Title).IsRequired().HasMaxLength(500);
        builder.Property(a => a.Summary).HasMaxLength(2000);
        builder.Property(a => a.Body).HasColumnType("nvarchar(max)");
        builder.Property(a => a.ImageUrl).HasMaxLength(2048);
        builder.Property(a => a.SourceName).IsRequired().HasMaxLength(100);

        builder.HasIndex(a => a.SourceName);
        builder.HasIndex(a => a.ScrapedAt);
        builder.HasIndex(a => a.IsFeatured);
        builder.HasIndex(a => a.IsTrending);

        builder.HasOne(a => a.Category)
            .WithMany(c => c.Articles)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.Author)
            .WithMany(au => au.Articles)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
