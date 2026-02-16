using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Infrastructure.Data.Configurations;

public class TrendingSearchConfiguration : IEntityTypeConfiguration<TrendingSearch>
{
    public void Configure(EntityTypeBuilder<TrendingSearch> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Query).IsRequired().HasMaxLength(500);
        builder.HasIndex(t => t.SearchCount);
    }
}
