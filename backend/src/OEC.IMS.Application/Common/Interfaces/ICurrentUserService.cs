namespace OEC.IMS.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
}
