using MediatR;
using OEC.IMS.Application.Features.Orders.Dtos;

namespace OEC.IMS.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(int OrderId) : IRequest<OrderDto>;
