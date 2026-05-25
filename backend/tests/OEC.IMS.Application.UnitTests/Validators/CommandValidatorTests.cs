using OEC.IMS.Application.Features.Auth.Commands.Login;
using OEC.IMS.Application.Features.Orders.Commands.CreateOrder;
using OEC.IMS.Application.Features.Parts.Commands.AdjustStock;

namespace OEC.IMS.Application.UnitTests.Validators;

public class CommandValidatorTests
{
    [Fact]
    public void LoginCommandValidator_RejectsEmptyCredentials()
    {
        var validator = new LoginCommandValidator();
        var result = validator.Validate(new LoginCommand("", ""));
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Username));
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(LoginCommand.Password));
    }

    [Fact]
    public void LoginCommandValidator_AcceptsValidCredentials()
    {
        var validator = new LoginCommandValidator();
        var result = validator.Validate(new LoginCommand("admin", "Admin123!"));
        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateOrderCommandValidator_RequiresAtLeastOneLine()
    {
        var validator = new CreateOrderCommandValidator();
        var result = validator.Validate(new CreateOrderCommand(Array.Empty<CreateOrderLineRequest>()));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateOrderCommandValidator_RejectsInvalidLineQuantity()
    {
        var validator = new CreateOrderCommandValidator();
        var result = validator.Validate(
            new CreateOrderCommand([new CreateOrderLineRequest(1, 0)]));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateOrderCommandValidator_AcceptsValidLines()
    {
        var validator = new CreateOrderCommandValidator();
        var result = validator.Validate(
            new CreateOrderCommand([new CreateOrderLineRequest(1, 2)]));
        Assert.True(result.IsValid);
    }

    [Fact]
    public void AdjustStockCommandValidator_RejectsZeroChange()
    {
        var validator = new AdjustStockCommandValidator();
        var result = validator.Validate(new AdjustStockCommand(1, 0, null));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void AdjustStockCommandValidator_AcceptsNonZeroChange()
    {
        var validator = new AdjustStockCommandValidator();
        var result = validator.Validate(new AdjustStockCommand(1, -5, "Cycle count"));
        Assert.True(result.IsValid);
    }
}
