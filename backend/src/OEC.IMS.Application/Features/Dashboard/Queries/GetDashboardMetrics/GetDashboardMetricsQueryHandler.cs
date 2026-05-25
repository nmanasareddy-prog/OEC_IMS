using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Dashboard.Dtos;
using OEC.IMS.Domain.Enums;

namespace OEC.IMS.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public sealed class GetDashboardMetricsQueryHandler
    : IRequestHandler<GetDashboardMetricsQuery, DashboardMetricsDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardMetricsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardMetricsDto> Handle(
        GetDashboardMetricsQuery request,
        CancellationToken cancellationToken)
    {
        var totalParts = await _context.Parts.CountAsync(cancellationToken);

        var lowStockCount = await _context.Parts.CountAsync(
            p => p.InventoryStock != null && p.InventoryStock.QuantityOnHand <= p.ReorderLevel,
            cancellationToken);

        var pendingOrders = await _context.Orders.CountAsync(
            o => o.Status == OrderStatus.Pending,
            cancellationToken);

        var recent = await _context.StockMovements
            .AsNoTracking()
            .OrderByDescending(m => m.OccurredAt)
            .Take(10)
            .Select(m => new StockActivityDto
            {
                StockMovementId = m.StockMovementId,
                PartSku = m.Part.Sku,
                PartName = m.Part.Name,
                QuantityChange = m.QuantityChange,
                MovementType = m.MovementType,
                OccurredAt = m.OccurredAt
            })
            .ToListAsync(cancellationToken);

        return new DashboardMetricsDto
        {
            TotalParts = totalParts,
            LowStockCount = lowStockCount,
            PendingOrdersCount = pendingOrders,
            RecentActivity = recent
        };
    }
}
