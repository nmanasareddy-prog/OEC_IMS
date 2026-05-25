using OEC.IMS.Domain.Common;
using OEC.IMS.Domain.Enums;

namespace OEC.IMS.Domain.Entities;

public class Order : AuditableEntity
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
