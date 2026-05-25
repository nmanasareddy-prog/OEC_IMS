namespace OEC.IMS.Application.UnitTests;

public class ArchitectureTests
{
    [Fact]
    public void Application_Assembly_Loads()
    {
        var assembly = typeof(OEC.IMS.Application.DependencyInjection).Assembly;
        Assert.NotNull(assembly);
    }
}
