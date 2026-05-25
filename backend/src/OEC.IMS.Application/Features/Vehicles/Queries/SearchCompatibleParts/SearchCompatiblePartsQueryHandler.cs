using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Vehicles.Dtos;

namespace OEC.IMS.Application.Features.Vehicles.Queries.SearchCompatibleParts;

public sealed class SearchCompatiblePartsQueryHandler
    : IRequestHandler<SearchCompatiblePartsQuery, IReadOnlyList<CompatiblePartDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchCompatiblePartsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CompatiblePartDto>> Handle(
        SearchCompatiblePartsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.PartVehicleCompatibilities
            .AsNoTracking()
            .Where(c =>
                c.VehicleModelId == request.VehicleModelId
                && request.Year >= c.YearFrom
                && request.Year <= c.YearTo)
            .Select(c => new CompatiblePartDto
            {
                PartId = c.Part.PartId,
                Sku = c.Part.Sku,
                Name = c.Part.Name,
                QuantityOnHand = c.Part.InventoryStock != null ? c.Part.InventoryStock.QuantityOnHand : 0,
                YearFrom = c.YearFrom,
                YearTo = c.YearTo
            })
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
