using OEC.IMS.Domain.Common;

namespace OEC.IMS.Domain.Entities;

public class Part : AuditableEntity, ISoftDelete
{
    public int PartId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public decimal UnitPrice { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public Category Category { get; set; } = null!;
    public InventoryStock? InventoryStock { get; set; }
    public ICollection<PartVehicleCompatibility> VehicleCompatibilities { get; set; } = new List<PartVehicleCompatibility>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
