namespace OEC.IMS.Application.Features.Vehicles.Dtos;

public sealed class ManufacturerDto
{
    public int ManufacturerId { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class VehicleModelDto
{
    public int VehicleModelId { get; init; }
    public int ManufacturerId { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class CompatiblePartDto
{
    public int PartId { get; init; }
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int QuantityOnHand { get; init; }
    public int YearFrom { get; init; }
    public int YearTo { get; init; }
}
