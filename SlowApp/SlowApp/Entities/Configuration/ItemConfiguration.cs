using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SlowApp.Entities.Configuration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            builder.HasKey(e => e.ItemId).IsClustered();
            builder.Property(e => e.ItemId).ValueGeneratedOnAdd();
            builder.Property(e => e.ItemText).HasMaxLength(200);
            builder.Property(e => e.CreatedDate);
            builder.Property(e => e.CreatedBy).HasMaxLength(200);
        }
    }
}
