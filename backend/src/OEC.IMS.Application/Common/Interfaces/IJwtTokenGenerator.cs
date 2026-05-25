namespace OEC.IMS.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string displayName, IEnumerable<string> roles);
}
