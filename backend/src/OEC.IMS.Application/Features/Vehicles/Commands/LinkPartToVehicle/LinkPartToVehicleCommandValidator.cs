using FluentValidation;

namespace OEC.IMS.Application.Features.Vehicles.Commands.LinkPartToVehicle;

public sealed class LinkPartToVehicleCommandValidator : AbstractValidator<LinkPartToVehicleCommand>
{
    public LinkPartToVehicleCommandValidator()
    {
        RuleFor(x => x.PartId).GreaterThan(0);
        RuleFor(x => x.VehicleModelId).GreaterThan(0);
        RuleFor(x => x.YearFrom).InclusiveBetween(1980, 2030);
        RuleFor(x => x.YearTo).InclusiveBetween(1980, 2030);
        RuleFor(x => x.YearTo).GreaterThanOrEqualTo(x => x.YearFrom);
    }
}
