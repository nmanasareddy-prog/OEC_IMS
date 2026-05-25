using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;

namespace OEC.IMS.Application.Features.Parts.Commands.DeletePart;

public sealed class DeletePartCommandHandler : IRequestHandler<DeletePartCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePartCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePartCommand request, CancellationToken cancellationToken)
    {
        var part = await _context.Parts
            .FirstOrDefaultAsync(p => p.PartId == request.PartId, cancellationToken)
            ?? throw new KeyNotFoundException($"Part {request.PartId} was not found.");

        part.IsDeleted = true;
        part.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
