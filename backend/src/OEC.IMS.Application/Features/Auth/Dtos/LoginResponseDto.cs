namespace OEC.IMS.Application.Features.Auth.Dtos;

public sealed class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
}
