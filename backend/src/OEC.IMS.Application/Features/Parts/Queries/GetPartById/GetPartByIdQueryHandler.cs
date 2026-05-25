using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Queries.GetPartById;

public sealed class GetPartByIdQueryHandler : IRequestHandler<GetPartByIdQuery, PartDto>
{
    private readonly IApplicationDbContext _context;

    public GetPartByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PartDto> Handle(GetPartByIdQuery request, CancellationToken cancellationToken)
    {
        var part = await _context.Parts
            .AsNoTracking()
            .Where(p => p.PartId == request.PartId)
            .Select(p => new PartDto
            {
                PartId = p.PartId,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                UnitPrice = p.UnitPrice,
                ReorderLevel = p.ReorderLevel,
                QuantityOnHand = p.InventoryStock != null ? p.InventoryStock.QuantityOnHand : 0,
                IsLowStock = p.InventoryStock != null && p.InventoryStock.QuantityOnHand <= p.ReorderLevel
            })
            .FirstOrDefaultAsync(cancellationToken);

        return part ?? throw new KeyNotFoundException($"Part {request.PartId} was not found.");
    }
}
