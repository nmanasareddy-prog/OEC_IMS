using MediatR;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Commands.CreatePart;

public sealed record CreatePartCommand(
    string Sku,
    string Name,
    string? Description,
    int CategoryId,
    decimal UnitPrice,
    int ReorderLevel,
    int InitialQuantity) : IRequest<PartDto>;
