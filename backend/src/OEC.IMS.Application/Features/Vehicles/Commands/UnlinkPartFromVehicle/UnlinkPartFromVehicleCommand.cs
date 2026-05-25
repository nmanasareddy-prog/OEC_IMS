using MediatR;

namespace OEC.IMS.Application.Features.Vehicles.Commands.UnlinkPartFromVehicle;

public sealed record UnlinkPartFromVehicleCommand(int CompatibilityId) : IRequest;
