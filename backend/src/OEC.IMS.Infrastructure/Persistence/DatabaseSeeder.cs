using Microsoft.EntityFrameworkCore;
using OEC.IMS.Domain.Entities;
using OEC.IMS.Domain.Enums;

namespace OEC.IMS.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Categories.AnyAsync(cancellationToken))
        {
            return;
        }

        var categories = new[]
        {
            new Category { CategoryId = 1, Name = "Engine" },
            new Category { CategoryId = 2, Name = "Brakes" },
            new Category { CategoryId = 3, Name = "Electrical" },
            new Category { CategoryId = 4, Name = "Body" },
        };

        var manufacturers = new[]
        {
            new Manufacturer { ManufacturerId = 1, Name = "Ford" },
            new Manufacturer { ManufacturerId = 2, Name = "Toyota" },
            new Manufacturer { ManufacturerId = 3, Name = "Honda" },
        };

        var models = new[]
        {
            new VehicleModel { VehicleModelId = 1, ManufacturerId = 1, Name = "F-150" },
            new VehicleModel { VehicleModelId = 2, ManufacturerId = 2, Name = "Camry" },
            new VehicleModel { VehicleModelId = 3, ManufacturerId = 3, Name = "Civic" },
        };

        var parts = new[]
        {
            new Part
            {
                PartId = 1,
                Sku = "BRK-PAD-001",
                Name = "Front Brake Pad Set",
                Description = "Ceramic brake pads — front axle",
                CategoryId = 2,
                UnitPrice = 89.99m,
                ReorderLevel = 10,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "seed"
            },
            new Part
            {
                PartId = 2,
                Sku = "ENG-FIL-002",
                Name = "Oil Filter",
                Description = "Standard oil filter",
                CategoryId = 1,
                UnitPrice = 12.50m,
                ReorderLevel = 25,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "seed"
            },
            new Part
            {
                PartId = 3,
                Sku = "ELC-BAT-003",
                Name = "12V Battery",
                Description = "Maintenance-free battery",
                CategoryId = 3,
                UnitPrice = 149.00m,
                ReorderLevel = 5,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "seed"
            },
            new Part
            {
                PartId = 4,
                Sku = "BDY-MIR-004",
                Name = "Side Mirror — Driver",
                Description = "Heated side mirror",
                CategoryId = 4,
                UnitPrice = 75.00m,
                ReorderLevel = 8,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "seed"
            },
        };

        var stocks = new[]
        {
            new InventoryStock { InventoryStockId = 1, PartId = 1, QuantityOnHand = 42 },
            new InventoryStock { InventoryStockId = 2, PartId = 2, QuantityOnHand = 8 },
            new InventoryStock { InventoryStockId = 3, PartId = 3, QuantityOnHand = 15 },
            new InventoryStock { InventoryStockId = 4, PartId = 4, QuantityOnHand = 3 },
        };

        var compatibilities = new[]
        {
            new PartVehicleCompatibility { PartVehicleCompatibilityId = 1, PartId = 1, VehicleModelId = 1, YearFrom = 2018, YearTo = 2024 },
            new PartVehicleCompatibility { PartVehicleCompatibilityId = 2, PartId = 2, VehicleModelId = 2, YearFrom = 2015, YearTo = 2023 },
            new PartVehicleCompatibility { PartVehicleCompatibilityId = 3, PartId = 3, VehicleModelId = 3, YearFrom = 2016, YearTo = 2024 },
        };

        context.Categories.AddRange(categories);
        context.Manufacturers.AddRange(manufacturers);
        context.VehicleModels.AddRange(models);
        context.Parts.AddRange(parts);
        context.InventoryStocks.AddRange(stocks);
        context.PartVehicleCompatibilities.AddRange(compatibilities);

        await context.SaveChangesAsync(cancellationToken);
    }
}
