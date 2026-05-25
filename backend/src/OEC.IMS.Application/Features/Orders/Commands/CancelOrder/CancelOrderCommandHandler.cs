using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Orders.Dtos;
using OEC.IMS.Domain.Entities;
using OEC.IMS.Domain.Enums;
using OEC.IMS.Domain.Exceptions;

namespace OEC.IMS.Application.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CancelOrderCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<OrderDto> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        await _context.ExecuteInTransactionAsync(async () =>
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .ThenInclude(l => l.Part)
                .ThenInclude(p => p.InventoryStock)
                .FirstOrDefaultAsync(o => o.OrderId == request.OrderId, cancellationToken)
                ?? throw new KeyNotFoundException($"Order {request.OrderId} was not found.");

            if (order.Status != OrderStatus.Pending)
            {
                throw new DomainException("Only pending orders can be cancelled.");
            }

            var userId = _currentUser.UserId ?? "system";

            foreach (var line in order.OrderLines)
            {
                if (line.Part.InventoryStock != null)
                {
                    line.Part.InventoryStock.QuantityOnHand += line.Quantity;
                }

                _context.StockMovements.Add(new StockMovement
                {
                    PartId = line.PartId,
                    QuantityChange = line.Quantity,
                    MovementType = "Cancel",
                    Reference = order.OrderNumber,
                    OccurredAt = DateTime.UtcNow,
                    CreatedBy = userId
                });
            }

            order.Status = OrderStatus.Cancelled;
        }, cancellationToken);

        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderLines)
            .ThenInclude(l => l.Part)
            .FirstAsync(o => o.OrderId == request.OrderId, cancellationToken);

        return _mapper.Map<OrderDto>(order);
    }
}
