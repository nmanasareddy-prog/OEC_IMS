using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Orders.Dtos;

namespace OEC.IMS.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderLines)
            .ThenInclude(l => l.Part)
            .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken);

        return order is null
            ? throw new KeyNotFoundException($"Order {request.OrderId} was not found.")
            : _mapper.Map<OrderDto>(order);
    }
}
