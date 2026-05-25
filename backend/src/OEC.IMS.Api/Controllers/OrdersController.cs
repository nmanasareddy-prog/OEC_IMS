using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEC.IMS.Application.Features.Orders.Commands.CancelOrder;
using OEC.IMS.Application.Features.Orders.Commands.CreateOrder;
using OEC.IMS.Application.Features.Orders.Queries.GetOrderById;
using OEC.IMS.Application.Features.Orders.Queries.SearchOrders;

namespace OEC.IMS.Api.Controllers;

[Authorize]
public class OrdersController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] SearchOrdersQuery query,
        CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetOrderByIdQuery(id), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new CancelOrderCommand(id), cancellationToken));
}
