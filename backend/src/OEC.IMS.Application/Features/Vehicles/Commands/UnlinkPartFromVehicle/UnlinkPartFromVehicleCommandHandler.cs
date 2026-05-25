using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;

namespace OEC.IMS.Application.Features.Vehicles.Commands.UnlinkPartFromVehicle;

public sealed class UnlinkPartFromVehicleCommandHandler : IRequestHandler<UnlinkPartFromVehicleCommand>
{
    private readonly IApplicationDbContext _context;

    public UnlinkPartFromVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UnlinkPartFromVehicleCommand request, CancellationToken cancellationToken)
    {
        var link = await _context.PartVehicleCompatibilities
            .FirstOrDefaultAsync(c => c.PartVehicleCompatibilityId == request.CompatibilityId, cancellationToken)
            ?? throw new KeyNotFoundException($"Compatibility {request.CompatibilityId} was not found.");

        _context.PartVehicleCompatibilities.Remove(link);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
