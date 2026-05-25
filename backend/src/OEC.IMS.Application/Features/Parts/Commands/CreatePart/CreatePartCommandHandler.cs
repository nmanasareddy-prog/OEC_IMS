using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Parts.Dtos;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Application.Features.Parts.Commands.CreatePart;

public sealed class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, PartDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public CreatePartCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<PartDto> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var part = new Part
        {
            Sku = request.Sku.Trim(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            CategoryId = request.CategoryId,
            UnitPrice = request.UnitPrice,
            ReorderLevel = request.ReorderLevel
        };

        _context.Parts.Add(part);
        await _context.SaveChangesAsync(cancellationToken);

        _context.InventoryStocks.Add(new InventoryStock
        {
            PartId = part.PartId,
            QuantityOnHand = request.InitialQuantity
        });

        if (request.InitialQuantity > 0)
        {
            _context.StockMovements.Add(new StockMovement
            {
                PartId = part.PartId,
                QuantityChange = request.InitialQuantity,
                MovementType = "Initial",
                Reference = "CREATE",
                OccurredAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId ?? "system"
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        var created = await _context.Parts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.InventoryStock)
            .FirstAsync(p => p.PartId == part.PartId, cancellationToken);

        return _mapper.Map<PartDto>(created);
    }
}
