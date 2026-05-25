using MediatR;
using OEC.IMS.Application.Common.Models;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Queries.SearchParts;

public sealed record SearchPartsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    int? CategoryId = null,
    bool? LowStockOnly = null,
    string Sort = "name",
    string SortDirection = "asc") : IRequest<PagedResult<PartListItemDto>>;
