using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManeFeed.Models;

namespace TheManeFeed.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Url)
            .IsRequired()
            .HasMaxLength(2048);

        builder.HasIndex(a => a.Url)
            .IsUnique();

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.Summary)
            .HasMaxLength(2000);

        builder.Property(a => a.SourceName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.SourceName);

        builder.Property(a => a.CategoryTags)
            .HasMaxLength(500);

        builder.Property(a => a.ImageUrl)
            .HasMaxLength(2048);

        builder.HasIndex(a => a.ScrapedAt);
    }
}
