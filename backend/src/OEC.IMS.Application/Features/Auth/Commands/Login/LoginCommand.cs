using MediatR;
using OEC.IMS.Application.Features.Auth.Dtos;

namespace OEC.IMS.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;
