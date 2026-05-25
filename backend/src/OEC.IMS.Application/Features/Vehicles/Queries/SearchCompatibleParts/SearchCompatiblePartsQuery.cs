using MediatR;
using OEC.IMS.Application.Features.Vehicles.Dtos;

namespace OEC.IMS.Application.Features.Vehicles.Queries.SearchCompatibleParts;

public sealed record SearchCompatiblePartsQuery(
    int VehicleModelId,
    int Year) : IRequest<IReadOnlyList<CompatiblePartDto>>;
