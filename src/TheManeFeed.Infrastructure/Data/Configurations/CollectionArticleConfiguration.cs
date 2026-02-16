using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Infrastructure.Data.Configurations;

public class CollectionArticleConfiguration : IEntityTypeConfiguration<CollectionArticle>
{
    public void Configure(EntityTypeBuilder<CollectionArticle> builder)
    {
        builder.HasKey(ca => ca.Id);
        builder.HasIndex(ca => new { ca.CollectionId, ca.ArticleId }).IsUnique();

        builder.HasOne(ca => ca.Collection)
            .WithMany(c => c.CollectionArticles)
            .HasForeignKey(ca => ca.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ca => ca.Article)
            .WithMany(a => a.CollectionArticles)
            .HasForeignKey(ca => ca.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
