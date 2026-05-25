using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Vehicles.Dtos;

namespace OEC.IMS.Application.Features.Vehicles.Queries.GetVehicleModels;

public sealed class GetVehicleModelsQueryHandler
    : IRequestHandler<GetVehicleModelsQuery, IReadOnlyList<VehicleModelDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetVehicleModelsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<VehicleModelDto>> Handle(
        GetVehicleModelsQuery request,
        CancellationToken cancellationToken) =>
        await _context.VehicleModels
            .AsNoTracking()
            .Where(v => v.ManufacturerId == request.ManufacturerId)
            .OrderBy(v => v.Name)
            .ProjectTo<VehicleModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}
