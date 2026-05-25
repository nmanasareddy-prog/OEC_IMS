using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.OrderId);
        builder.Property(o => o.OrderNumber).HasMaxLength(32).IsRequired();
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);

        builder.HasMany(o => o.OrderLines)
            .WithOne(l => l.Order)
            .HasForeignKey(l => l.OrderId);
    }
}
