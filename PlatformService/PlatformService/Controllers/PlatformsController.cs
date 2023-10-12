using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBus;

    public PlatformsController(
        IPlatformRepository repository,
        IMapper mapper,
        IMessageBusClient messageBus)
    {
        _repository = repository;
        _mapper = mapper;
        _messageBus = messageBus;
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

        var publishedPlatform = _mapper.Map<PlatformPublishDto>(result);
        publishedPlatform.Event = "Platform Published";
        _messageBus.PublishPlatform(publishedPlatform);

        return result;
    }
}
