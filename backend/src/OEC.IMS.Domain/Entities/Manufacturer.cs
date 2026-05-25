namespace OEC.IMS.Domain.Entities;

public class Manufacturer
{
    public int ManufacturerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<VehicleModel> VehicleModels { get; set; } = new List<VehicleModel>();
}
