using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OEC.IMS.Application.Features.Auth.Commands.Login;

namespace OEC.IMS.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    /// <summary>Mock login — returns JWT for demo users (admin / clerk).</summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new LoginCommand(request.Username, request.Password),
            cancellationToken);
        return Ok(result);
    }

    public sealed record LoginRequest(string Username, string Password);
}
