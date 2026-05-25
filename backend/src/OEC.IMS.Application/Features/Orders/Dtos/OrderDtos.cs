using OEC.IMS.Domain.Enums;

namespace OEC.IMS.Application.Features.Orders.Dtos;

public sealed class OrderDto
{
    public int OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<OrderLineDto> Lines { get; init; } = Array.Empty<OrderLineDto>();
}

public sealed class OrderLineDto
{
    public int OrderLineId { get; init; }
    public int PartId { get; init; }
    public string PartSku { get; init; } = string.Empty;
    public string PartName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal { get; init; }
}

public sealed class OrderListItemDto
{
    public int OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public int LineCount { get; init; }
}
