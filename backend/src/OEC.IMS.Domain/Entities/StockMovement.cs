namespace OEC.IMS.Domain.Entities;

public class StockMovement
{
    public int StockMovementId { get; set; }
    public int PartId { get; set; }
    public int QuantityChange { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public DateTime OccurredAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public Part Part { get; set; } = null!;
}
