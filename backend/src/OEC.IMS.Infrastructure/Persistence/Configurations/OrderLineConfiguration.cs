using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Infrastructure.Persistence.Configurations;

public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable("OrderLines");
        builder.HasKey(l => l.OrderLineId);
        builder.Property(l => l.UnitPrice).HasPrecision(18, 2);
        builder.Property(l => l.LineTotal).HasPrecision(18, 2);

        builder.HasOne(l => l.Part)
            .WithMany()
            .HasForeignKey(l => l.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
