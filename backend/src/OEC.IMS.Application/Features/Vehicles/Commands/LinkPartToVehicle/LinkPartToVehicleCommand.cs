using MediatR;

namespace OEC.IMS.Application.Features.Vehicles.Commands.LinkPartToVehicle;

public sealed record LinkPartToVehicleCommand(
    int PartId,
    int VehicleModelId,
    int YearFrom,
    int YearTo) : IRequest<int>;
