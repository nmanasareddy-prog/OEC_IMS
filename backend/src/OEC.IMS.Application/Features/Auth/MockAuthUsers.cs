namespace OEC.IMS.Application.Features.Auth;

public static class MockAuthUsers
{
    public sealed record MockUser(string UserId, string Username, string Password, string[] Roles);

    public static readonly MockUser[] Users =
    [
        new("admin", "admin", "Admin123!", ["Admin", "InventoryClerk"]),
        new("clerk", "clerk", "Clerk123!", ["InventoryClerk"])
    ];

    public static MockUser? Validate(string username, string password) =>
        Users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            && u.Password == password);
}
