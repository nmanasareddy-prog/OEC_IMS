using MediatR;
using OEC.IMS.Application.Features.Vehicles.Dtos;

namespace OEC.IMS.Application.Features.Vehicles.Queries.GetVehicleModels;

public sealed record GetVehicleModelsQuery(int ManufacturerId) : IRequest<IReadOnlyList<VehicleModelDto>>;
