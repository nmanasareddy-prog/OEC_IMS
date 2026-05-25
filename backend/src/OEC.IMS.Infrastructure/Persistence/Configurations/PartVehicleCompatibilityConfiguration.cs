using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Infrastructure.Persistence.Configurations;

public class PartVehicleCompatibilityConfiguration : IEntityTypeConfiguration<PartVehicleCompatibility>
{
    public void Configure(EntityTypeBuilder<PartVehicleCompatibility> builder)
    {
        builder.ToTable("PartVehicleCompatibilities");
        builder.HasKey(c => c.PartVehicleCompatibilityId);
        builder.HasIndex(c => new { c.PartId, c.VehicleModelId, c.YearFrom, c.YearTo });

        builder.HasOne(c => c.Part)
            .WithMany(p => p.VehicleCompatibilities)
            .HasForeignKey(c => c.PartId);

        builder.HasOne(c => c.VehicleModel)
            .WithMany(v => v.PartCompatibilities)
            .HasForeignKey(c => c.VehicleModelId);
    }
}
