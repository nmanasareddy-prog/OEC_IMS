using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Domain.Entities;
using OEC.IMS.Domain.Exceptions;

namespace OEC.IMS.Application.Features.Vehicles.Commands.LinkPartToVehicle;

public sealed class LinkPartToVehicleCommandHandler : IRequestHandler<LinkPartToVehicleCommand, int>
{
    private readonly IApplicationDbContext _context;

    public LinkPartToVehicleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(LinkPartToVehicleCommand request, CancellationToken cancellationToken)
    {
        var partExists = await _context.Parts.AnyAsync(p => p.PartId == request.PartId, cancellationToken);
        var modelExists = await _context.VehicleModels.AnyAsync(
            v => v.VehicleModelId == request.VehicleModelId,
            cancellationToken);

        if (!partExists || !modelExists)
        {
            throw new KeyNotFoundException("Part or vehicle model not found.");
        }

        var duplicate = await _context.PartVehicleCompatibilities.AnyAsync(
            c => c.PartId == request.PartId && c.VehicleModelId == request.VehicleModelId,
            cancellationToken);

        if (duplicate)
        {
            throw new DomainException("Compatibility link already exists for this part and model.");
        }

        var link = new PartVehicleCompatibility
        {
            PartId = request.PartId,
            VehicleModelId = request.VehicleModelId,
            YearFrom = request.YearFrom,
            YearTo = request.YearTo
        };

        _context.PartVehicleCompatibilities.Add(link);
        await _context.SaveChangesAsync(cancellationToken);
        return link.PartVehicleCompatibilityId;
    }
}
