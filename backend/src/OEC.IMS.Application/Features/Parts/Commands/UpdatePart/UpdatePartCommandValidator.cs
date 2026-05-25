using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OEC.IMS.Application.Common.Interfaces;

namespace OEC.IMS.Application.Features.Parts.Commands.UpdatePart;

public sealed class UpdatePartCommandValidator : AbstractValidator<UpdatePartCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdatePartCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.PartId).GreaterThan(0);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReorderLevel).GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .MustAsync(BeUniqueSkuForPart)
            .WithMessage("SKU already exists.");

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExists)
            .WithMessage("Category does not exist.");
    }

    private async Task<bool> BeUniqueSkuForPart(UpdatePartCommand cmd, CancellationToken cancellationToken) =>
        !await _context.Parts.AnyAsync(
            p => p.Sku == cmd.Sku && p.PartId != cmd.PartId,
            cancellationToken);

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken) =>
        await _context.Categories.AnyAsync(c => c.CategoryId == categoryId, cancellationToken);
}
