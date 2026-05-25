using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Infrastructure.Persistence.Configurations;

public class VehicleModelConfiguration : IEntityTypeConfiguration<VehicleModel>
{
    public void Configure(EntityTypeBuilder<VehicleModel> builder)
    {
        builder.ToTable("VehicleModels");
        builder.HasKey(v => v.VehicleModelId);
        builder.Property(v => v.Name).HasMaxLength(128).IsRequired();

        builder.HasOne(v => v.Manufacturer)
            .WithMany(m => m.VehicleModels)
            .HasForeignKey(v => v.ManufacturerId);
    }
}
