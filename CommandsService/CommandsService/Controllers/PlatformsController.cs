using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/commands/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    [HttpPost("Test")]
    public string Test()
    {
        return "a";
    }
}
