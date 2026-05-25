using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Vehicles.Dtos;

namespace OEC.IMS.Application.Features.Vehicles.Queries.GetManufacturers;

public sealed class GetManufacturersQueryHandler
    : IRequestHandler<GetManufacturersQuery, IReadOnlyList<ManufacturerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetManufacturersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ManufacturerDto>> Handle(
        GetManufacturersQuery request,
        CancellationToken cancellationToken) =>
        await _context.Manufacturers
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .ProjectTo<ManufacturerDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}
