using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");
        builder.HasKey(m => m.StockMovementId);
        builder.Property(m => m.MovementType).HasMaxLength(32).IsRequired();
        builder.Property(m => m.Reference).HasMaxLength(64);
        builder.HasIndex(m => m.OccurredAt);
    }
}
