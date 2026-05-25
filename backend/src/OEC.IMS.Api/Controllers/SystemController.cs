using Microsoft.AspNetCore.Mvc;

namespace OEC.IMS.Api.Controllers;

/// <summary>
/// System endpoints for health and version checks during development.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class SystemController : ControllerBase
{
    /// <summary>
    /// Returns API status — use until feature modules are implemented.
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetStatus() =>
        Ok(new
        {
            application = "OEC Inventory Management System",
            version = "0.1.0-scaffold",
            phase = "Phase 2 — structure ready; features pending"
        });
}
