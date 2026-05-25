using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEC.IMS.Application.Features.Dashboard.Queries.GetDashboardMetrics;

namespace OEC.IMS.Api.Controllers;

[Authorize]
public class DashboardController : ApiControllerBase
{
    [HttpGet("metrics")]
    public async Task<IActionResult> GetMetrics(CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetDashboardMetricsQuery(), cancellationToken));
}
