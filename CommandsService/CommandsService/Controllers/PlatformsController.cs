using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;

namespace CommandsService.Controllers;

[Route("api/commands/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    [HttpPost("PlatformCreated")]
    public void PlatformCreated(PlatformDto platform)
    {
        Console.WriteLine($"Platform Created, {platform.Id}:{platform.Name}");
    }
}
