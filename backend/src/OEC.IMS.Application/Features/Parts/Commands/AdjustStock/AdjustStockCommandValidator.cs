using FluentValidation;

namespace OEC.IMS.Application.Features.Parts.Commands.AdjustStock;

public sealed class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(x => x.PartId).GreaterThan(0);
        RuleFor(x => x.QuantityChange).NotEqual(0);
        RuleFor(x => x.Reason).MaximumLength(256);
    }
}
