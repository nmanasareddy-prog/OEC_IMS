using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Extensions;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Common.Models;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Queries.SearchParts;

public sealed class SearchPartsQueryHandler : IRequestHandler<SearchPartsQuery, PagedResult<PartListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public SearchPartsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PartListItemDto>> Handle(
        SearchPartsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Parts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(p =>
                p.Sku.Contains(term) || p.Name.Contains(term));
        }

        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        if (request.LowStockOnly == true)
        {
            query = query.Where(p =>
                p.InventoryStock != null && p.InventoryStock.QuantityOnHand <= p.ReorderLevel);
        }

        query = ApplySort(query, request.Sort, request.SortDirection);

        var projected = query.Select(p => new PartListItemDto
        {
            PartId = p.PartId,
            Sku = p.Sku,
            Name = p.Name,
            CategoryName = p.Category.Name,
            UnitPrice = p.UnitPrice,
            QuantityOnHand = p.InventoryStock != null ? p.InventoryStock.QuantityOnHand : 0,
            ReorderLevel = p.ReorderLevel,
            IsLowStock = p.InventoryStock != null && p.InventoryStock.QuantityOnHand <= p.ReorderLevel
        });

        return await projected.ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);
    }

    private static IQueryable<Domain.Entities.Part> ApplySort(
        IQueryable<Domain.Entities.Part> query,
        string sort,
        string direction)
    {
        var desc = direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
        return sort.ToLowerInvariant() switch
        {
            "sku" => desc ? query.OrderByDescending(p => p.Sku) : query.OrderBy(p => p.Sku),
            "price" => desc ? query.OrderByDescending(p => p.UnitPrice) : query.OrderBy(p => p.UnitPrice),
            "stock" => desc
                ? query.OrderByDescending(p => p.InventoryStock!.QuantityOnHand)
                : query.OrderBy(p => p.InventoryStock!.QuantityOnHand),
            _ => desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
        };
    }
}
