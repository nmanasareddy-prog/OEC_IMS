using MediatR;
using OEC.IMS.Application.Features.Parts.Dtos;

namespace OEC.IMS.Application.Features.Parts.Queries.GetPartById;

public sealed record GetPartByIdQuery(int PartId) : IRequest<PartDto>;
