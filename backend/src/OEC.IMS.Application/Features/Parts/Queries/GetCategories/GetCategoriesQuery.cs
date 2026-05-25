using MediatR;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Queries.GetCategories;

public sealed record GetCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
