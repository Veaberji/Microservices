using AutoMapper;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;

namespace CommandsService.Controllers;

[Route("api/commands/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(
        ICommandRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [Route("platforms")]
    [HttpGet]
    public async Task<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        return _mapper.Map<IEnumerable<PlatformReadDto>>(await _repository.GetAllPlatformsAsync());
    }

    [Route("platformCreated")]
    [HttpPost]
    public void PlatformCreated(PlatformReadDto platform)
    {
        Console.WriteLine($"Platform Created, {platform.Id}:{platform.Name}");
    }
}
