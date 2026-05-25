using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Infrastructure.Persistence.Configurations;

public class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.ToTable("Parts");
        builder.HasKey(p => p.PartId);
        builder.Property(p => p.Sku).HasMaxLength(64).IsRequired();
        builder.HasIndex(p => p.Sku).IsUnique();
        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.Property(p => p.UnitPrice).HasPrecision(18, 2);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Parts)
            .HasForeignKey(p => p.CategoryId);

        builder.HasOne(p => p.InventoryStock)
            .WithOne(s => s.Part)
            .HasForeignKey<InventoryStock>(s => s.PartId)
            .IsRequired(false);
    }
}
