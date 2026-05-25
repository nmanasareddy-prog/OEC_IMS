namespace OEC.IMS.Domain.Entities;

public class VehicleModel
{
    public int VehicleModelId { get; set; }
    public int ManufacturerId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Manufacturer Manufacturer { get; set; } = null!;
    public ICollection<PartVehicleCompatibility> PartCompatibilities { get; set; } = new List<PartVehicleCompatibility>();
}
