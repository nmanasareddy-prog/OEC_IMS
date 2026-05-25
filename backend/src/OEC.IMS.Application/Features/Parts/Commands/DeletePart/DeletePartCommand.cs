using MediatR;

namespace OEC.IMS.Application.Features.Parts.Commands.DeletePart;

public sealed record DeletePartCommand(int PartId) : IRequest;
