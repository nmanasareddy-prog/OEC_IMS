using MediatR;
using OEC.IMS.Application.Features.Dashboard.Dtos;

namespace OEC.IMS.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public sealed record GetDashboardMetricsQuery : IRequest<DashboardMetricsDto>;
