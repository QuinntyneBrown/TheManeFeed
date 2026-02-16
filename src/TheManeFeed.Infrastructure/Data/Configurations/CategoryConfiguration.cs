using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheManeFeed.Core.Entities;

namespace TheManeFeed.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Color).HasMaxLength(20);

        builder.HasData(
            new Category { Id = 1, Name = "Color Trends", Slug = "color", Color = "#C85A7C", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Celebrity Hair", Slug = "celebrity", Color = "#C9A96E", DisplayOrder = 2 },
            new Category { Id = 3, Name = "Haircuts & Styles", Slug = "cuts", Color = "#8B7D6B", DisplayOrder = 3 },
            new Category { Id = 4, Name = "Hair Care", Slug = "care", Color = "#D4A0A7", DisplayOrder = 4 },
            new Category { Id = 5, Name = "Products & Reviews", Slug = "products", Color = "#1A1A1A", DisplayOrder = 5 },
            new Category { Id = 6, Name = "Salon Guide", Slug = "salon", Color = "#C9A96E", DisplayOrder = 6 },
            new Category { Id = 7, Name = "Styling", Slug = "styling", Color = "#8B7D6B", DisplayOrder = 7 },
            new Category { Id = 8, Name = "Trending", Slug = "trending", Color = "#C85A7C", DisplayOrder = 8 }
        );
    }
}
