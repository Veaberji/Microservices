using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandClient;

    public PlatformsController(
        IPlatformRepository repository,
        IMapper mapper,
        ICommandDataClient commandClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandClient = commandClient;
    }

    [HttpGet("AllPlatforms")]
    public async Task<IEnumerable<PlatformReadDto>> GetAllPlatforms()
    {
        var platforms = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
    }

    [HttpGet("PlatformById")]
    public async Task<IActionResult> GetPlatformById(int id)
    {
        var platform = await _repository.GetByIdAsync(id);
        return platform is null 
            ? NotFound() 
            : Ok(_mapper.Map<PlatformReadDto>(platform));
    }

    [HttpPost("CreatePlatform")]
    public async Task<PlatformReadDto> CreatePlatform(PlatformCreateDto model)
    {
        var platform = _mapper.Map<Platform>(model);
        await _repository.CreateAsync(platform);
        await _repository.SaveChangesAsync();

        var result = _mapper.Map<PlatformReadDto>(platform);

        await _commandClient.SendPlatformToCommand(result);

        return result;
    }
}
