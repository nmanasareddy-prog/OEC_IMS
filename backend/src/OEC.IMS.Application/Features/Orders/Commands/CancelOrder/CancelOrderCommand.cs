using MediatR;
using OEC.IMS.Application.Features.Orders.Dtos;

namespace OEC.IMS.Application.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(int OrderId) : IRequest<OrderDto>;
