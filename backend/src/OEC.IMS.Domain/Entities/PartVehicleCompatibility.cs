namespace OEC.IMS.Domain.Entities;

public class PartVehicleCompatibility
{
    public int PartVehicleCompatibilityId { get; set; }
    public int PartId { get; set; }
    public int VehicleModelId { get; set; }
    public int YearFrom { get; set; }
    public int YearTo { get; set; }

    public Part Part { get; set; } = null!;
    public VehicleModel VehicleModel { get; set; } = null!;
}
