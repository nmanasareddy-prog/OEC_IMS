using MediatR;
using OEC.IMS.Application.Common.Models;
using OEC.IMS.Application.Features.Orders.Dtos;
using OEC.IMS.Domain.Enums;

namespace OEC.IMS.Application.Features.Orders.Queries.SearchOrders;

public sealed record SearchOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    OrderStatus? Status = null) : IRequest<PagedResult<OrderListItemDto>>;
