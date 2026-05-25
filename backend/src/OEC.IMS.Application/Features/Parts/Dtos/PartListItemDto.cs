namespace OEC.IMS.Application.Features.Parts.Dtos;

public sealed class PartListItemDto
{
    public int PartId { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int QuantityOnHand { get; init; }
    public int ReorderLevel { get; init; }
    public bool IsLowStock { get; init; }
}
