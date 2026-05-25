using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Parts.Dtos;
using OEC.IMS.Domain.Exceptions;

namespace OEC.IMS.Application.Features.Parts.Commands.UpdatePart;

public sealed class UpdatePartCommandHandler : IRequestHandler<UpdatePartCommand, PartDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePartCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PartDto> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
    {
        var part = await _context.Parts
            .FirstOrDefaultAsync(p => p.PartId == request.PartId, cancellationToken)
            ?? throw new KeyNotFoundException($"Part {request.PartId} was not found.");

        part.Sku = request.Sku.Trim();
        part.Name = request.Name.Trim();
        part.Description = request.Description?.Trim();
        part.CategoryId = request.CategoryId;
        part.UnitPrice = request.UnitPrice;
        part.ReorderLevel = request.ReorderLevel;

        await _context.SaveChangesAsync(cancellationToken);

        var updated = await _context.Parts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.InventoryStock)
            .FirstAsync(p => p.PartId == part.PartId, cancellationToken);

        return _mapper.Map<PartDto>(updated);
    }
}
