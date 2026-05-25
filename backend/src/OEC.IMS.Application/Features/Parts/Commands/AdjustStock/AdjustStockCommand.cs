using MediatR;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Commands.AdjustStock;

public sealed record AdjustStockCommand(
    int PartId,
    int QuantityChange,
    string? Reason) : IRequest<PartDto>;
