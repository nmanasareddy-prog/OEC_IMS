using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Parts.Dtos;
using OEC.IMS.Domain.Entities;
using OEC.IMS.Domain.Exceptions;

namespace OEC.IMS.Application.Features.Parts.Commands.AdjustStock;

public sealed class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, PartDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public AdjustStockCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<PartDto> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _context.InventoryStocks
            .FirstOrDefaultAsync(s => s.PartId == request.PartId, cancellationToken)
            ?? throw new KeyNotFoundException($"Stock for part {request.PartId} was not found.");

        var newQty = stock.QuantityOnHand + request.QuantityChange;
        if (newQty < 0)
        {
            throw new DomainException("Insufficient stock for this adjustment.");
        }

        stock.QuantityOnHand = newQty;

        _context.StockMovements.Add(new StockMovement
        {
            PartId = request.PartId,
            QuantityChange = request.QuantityChange,
            MovementType = "Adjustment",
            Reference = request.Reason ?? "ADJUST",
            OccurredAt = DateTime.UtcNow,
            CreatedBy = _currentUser.UserId ?? "system"
        });

        await _context.SaveChangesAsync(cancellationToken);

        var part = await _context.Parts
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.InventoryStock)
            .FirstAsync(p => p.PartId == request.PartId, cancellationToken);

        return _mapper.Map<PartDto>(part);
    }
}
