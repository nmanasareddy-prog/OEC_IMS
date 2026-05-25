using MediatR;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Commands.UpdatePart;

public sealed record UpdatePartCommand(
    int PartId,
    string Sku,
    string Name,
    string? Description,
    int CategoryId,
    decimal UnitPrice,
    int ReorderLevel) : IRequest<PartDto>;
