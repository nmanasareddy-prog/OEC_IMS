namespace OEC.IMS.Domain.Entities;

public class InventoryStock
{
    public int InventoryStockId { get; set; }
    public int PartId { get; set; }
    public int QuantityOnHand { get; set; }

    public Part Part { get; set; } = null!;
}
