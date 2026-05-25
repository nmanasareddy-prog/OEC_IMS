using MediatR;
using OEC.IMS.Application.Features.Vehicles.Dtos;

namespace OEC.IMS.Application.Features.Vehicles.Queries.GetManufacturers;

public sealed record GetManufacturersQuery : IRequest<IReadOnlyList<ManufacturerDto>>;
