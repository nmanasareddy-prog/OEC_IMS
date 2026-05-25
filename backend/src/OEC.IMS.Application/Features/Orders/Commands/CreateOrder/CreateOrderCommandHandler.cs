using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Orders.Dtos;
using OEC.IMS.Domain.Entities;
using OEC.IMS.Domain.Enums;
using OEC.IMS.Domain.Exceptions;

namespace OEC.IMS.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CreateOrderCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order? createdOrder = null;

        await _context.ExecuteInTransactionAsync(async () =>
        {
            var partIds = request.Lines.Select(l => l.PartId).Distinct().ToList();
            var parts = await _context.Parts
                .Include(p => p.InventoryStock)
                .Where(p => partIds.Contains(p.PartId))
                .ToListAsync(cancellationToken);

            if (parts.Count != partIds.Count)
            {
                throw new KeyNotFoundException("One or more parts were not found.");
            }

            var orderNumber = await GenerateOrderNumberAsync(cancellationToken);
            var order = new Order
            {
                OrderNumber = orderNumber,
                Status = OrderStatus.Pending,
                TotalAmount = 0
            };

            decimal total = 0;
            var userId = _currentUser.UserId ?? "system";

            foreach (var lineRequest in request.Lines)
            {
                var part = parts.First(p => p.PartId == lineRequest.PartId);
                var stock = part.InventoryStock
                    ?? throw new DomainException($"No inventory record for part {part.Sku}.");

                if (stock.QuantityOnHand < lineRequest.Quantity)
                {
                    throw new DomainException(
                        $"Insufficient stock for {part.Sku}. Available: {stock.QuantityOnHand}, requested: {lineRequest.Quantity}.");
                }

                var lineTotal = part.UnitPrice * lineRequest.Quantity;
                total += lineTotal;

                order.OrderLines.Add(new OrderLine
                {
                    PartId = part.PartId,
                    Quantity = lineRequest.Quantity,
                    UnitPrice = part.UnitPrice,
                    LineTotal = lineTotal
                });

                stock.QuantityOnHand -= lineRequest.Quantity;

                _context.StockMovements.Add(new StockMovement
                {
                    PartId = part.PartId,
                    QuantityChange = -lineRequest.Quantity,
                    MovementType = "Order",
                    Reference = orderNumber,
                    OccurredAt = DateTime.UtcNow,
                    CreatedBy = userId
                });
            }

            order.TotalAmount = total;
            _context.Orders.Add(order);
            createdOrder = order;
        }, cancellationToken);

        return await LoadOrderDtoAsync(createdOrder!.OrderId, cancellationToken);
    }

    private async Task<OrderDto> LoadOrderDtoAsync(int orderId, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderLines)
            .ThenInclude(l => l.Part)
            .FirstAsync(o => o.OrderId == orderId, cancellationToken);

        return _mapper.Map<OrderDto>(order);
    }

    private async Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.ToString("yyyyMMdd");
        var prefix = $"ORD-{today}-";
        var count = await _context.Orders.CountAsync(
            o => o.OrderNumber.StartsWith(prefix),
            cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
