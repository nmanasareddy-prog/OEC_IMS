namespace OEC.IMS.Application.Features.Parts.Dtos;

public sealed class PartDto
{
    public int PartId { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int ReorderLevel { get; init; }
    public int QuantityOnHand { get; init; }
    public bool IsLowStock { get; init; }
}
