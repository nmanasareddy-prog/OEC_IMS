namespace OEC.IMS.Domain.Entities;

public class OrderLine
{
    public int OrderLineId { get; set; }
    public int OrderId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }

    public Order Order { get; set; } = null!;
    public Part Part { get; set; } = null!;
}
