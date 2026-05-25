namespace OEC.IMS.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Part> Parts { get; set; } = new List<Part>();
}
