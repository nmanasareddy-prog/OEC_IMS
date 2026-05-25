using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEC.IMS.Application.Features.Vehicles.Commands.LinkPartToVehicle;
using OEC.IMS.Application.Features.Vehicles.Commands.UnlinkPartFromVehicle;
using OEC.IMS.Application.Features.Vehicles.Queries.GetManufacturers;
using OEC.IMS.Application.Features.Vehicles.Queries.GetVehicleModels;
using OEC.IMS.Application.Features.Vehicles.Queries.SearchCompatibleParts;

namespace OEC.IMS.Api.Controllers;

[Authorize]
public class VehiclesController : ApiControllerBase
{
    [HttpGet("manufacturers")]
    public async Task<IActionResult> GetManufacturers(CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetManufacturersQuery(), cancellationToken));

    [HttpGet("models")]
    public async Task<IActionResult> GetModels(
        [FromQuery] int manufacturerId,
        CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new GetVehicleModelsQuery(manufacturerId), cancellationToken));

    [HttpGet("compatible-parts")]
    public async Task<IActionResult> SearchCompatibleParts(
        [FromQuery] int vehicleModelId,
        [FromQuery] int year,
        CancellationToken cancellationToken) =>
        Ok(await Mediator.Send(new SearchCompatiblePartsQuery(vehicleModelId, year), cancellationToken));

    [HttpPost("compatibility")]
    public async Task<IActionResult> LinkCompatibility(
        [FromBody] LinkPartToVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var id = await Mediator.Send(command, cancellationToken);
        return Created(string.Empty, new { compatibilityId = id });
    }

    [HttpDelete("compatibility/{id:int}")]
    public async Task<IActionResult> UnlinkCompatibility(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UnlinkPartFromVehicleCommand(id), cancellationToken);
        return NoContent();
    }
}
