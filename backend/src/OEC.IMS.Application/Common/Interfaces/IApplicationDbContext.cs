using Microsoft.EntityFrameworkCore;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Part> Parts { get; }
    DbSet<Category> Categories { get; }
    DbSet<InventoryStock> InventoryStocks { get; }
    DbSet<StockMovement> StockMovements { get; }
    DbSet<Manufacturer> Manufacturers { get; }
    DbSet<VehicleModel> VehicleModels { get; }
    DbSet<PartVehicleCompatibility> PartVehicleCompatibilities { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderLine> OrderLines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task ExecuteInTransactionAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default);
}
