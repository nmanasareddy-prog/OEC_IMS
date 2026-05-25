namespace OEC.IMS.Application.Features.Dashboard.Dtos;

public sealed class DashboardMetricsDto
{
    public int TotalParts { get; init; }
    public int LowStockCount { get; init; }
    public int PendingOrdersCount { get; init; }
    public IReadOnlyList<StockActivityDto> RecentActivity { get; init; } = Array.Empty<StockActivityDto>();
}

public sealed class StockActivityDto
{
    public int StockMovementId { get; init; }
    public string PartSku { get; init; } = string.Empty;
    public string PartName { get; init; } = string.Empty;
    public int QuantityChange { get; init; }
    public string MovementType { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; }
}
