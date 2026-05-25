using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Extensions;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Common.Models;
using OEC.IMS.Application.Features.Orders.Dtos;

namespace OEC.IMS.Application.Features.Orders.Queries.SearchOrders;

public sealed class SearchOrdersQueryHandler : IRequestHandler<SearchOrdersQuery, PagedResult<OrderListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchOrdersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<OrderListItemDto>> Handle(
        SearchOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Orders.AsNoTracking();

        if (request.Status.HasValue)
        {
            query = query.Where(o => o.Status == request.Status.Value);
        }

        var projected = query
            .OrderByDescending(o => o.CreatedAt)
            .ProjectTo<OrderListItemDto>(_mapper.ConfigurationProvider);

        return await projected.ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);
    }
}
