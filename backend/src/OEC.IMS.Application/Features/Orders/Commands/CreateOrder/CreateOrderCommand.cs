using MediatR;
using OEC.IMS.Application.Features.Orders.Dtos;

namespace OEC.IMS.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderLineRequest(int PartId, int Quantity);

public sealed record CreateOrderCommand(IReadOnlyList<CreateOrderLineRequest> Lines) : IRequest<OrderDto>;
