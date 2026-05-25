using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEC.IMS.Application.Features.Parts.Commands.AdjustStock;
using OEC.IMS.Application.Features.Parts.Commands.CreatePart;
using OEC.IMS.Application.Features.Parts.Commands.DeletePart;
using OEC.IMS.Application.Features.Parts.Commands.UpdatePart;
using OEC.IMS.Application.Features.Parts.Queries.GetCategories;
using OEC.IMS.Application.Features.Parts.Queries.GetPartById;
using OEC.IMS.Application.Features.Parts.Queries.SearchParts;

namespace OEC.IMS.Api.Controllers;

[Authorize]
public class PartsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] SearchPartsQuery query,
        CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(query, cancellationToken));

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetCategoriesQuery(), cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetPartByIdQuery(id), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePartCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.PartId }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePartRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePartCommand(
            id,
            request.Sku,
            request.Name,
            request.Description,
            request.CategoryId,
            request.UnitPrice,
            request.ReorderLevel);
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePartCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:int}/adjust-stock")]
    public async Task<IActionResult> AdjustStock(
        int id,
        [FromBody] AdjustStockRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AdjustStockCommand(id, request.QuantityChange, request.Reason);
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    public sealed record UpdatePartRequest(
        string Sku,
        string Name,
        string? Description,
        int CategoryId,
        decimal UnitPrice,
        int ReorderLevel);

    public sealed record AdjustStockRequest(int QuantityChange, string? Reason);
}
