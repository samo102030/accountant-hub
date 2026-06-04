using Microsoft.AspNetCore.Mvc;

namespace AccountantHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            success = true,
            message = "OK",
            data = new { status = "healthy" },
            meta = (object?)null
        });
    }
}
