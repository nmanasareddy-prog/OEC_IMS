using MediatR;
using OEC.IMS.Application.Common.Interfaces;
using OEC.IMS.Application.Features.Auth.Dtos;

namespace OEC.IMS.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginCommandHandler(IJwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = MockAuthUsers.Validate(request.Username, request.Password)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        var token = _tokenGenerator.GenerateToken(user.UserId, user.Username, user.Roles);

        return Task.FromResult(new LoginResponseDto
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            Roles = user.Roles
        });
    }
}
