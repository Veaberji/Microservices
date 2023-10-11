using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;

namespace CommandsService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public CommandsController(
        ICommandRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [Route("commandsForPlatform")]
    [HttpGet]
    public async Task<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        return _mapper.Map<IEnumerable<CommandReadDto>>(await _repository.GetCommandsForPlatformAsync(platformId));
    }

    [Route("command")]
    [HttpGet]
    public async Task<CommandReadDto> GetCommand(int commandId)
    {
        return _mapper.Map<CommandReadDto>(await _repository.GetCommandAsync(commandId));
    }


    [Route("command")]
    [HttpPost]
    public async Task<CommandReadDto> CreateCommand(CommandCreateDto model, int platformId)
    {
        var command = _mapper.Map<Command>(model);
        command.PlatformId = platformId;
        await _repository.CreateCommandAsync(command);
        await _repository.SaveChangesAsync();

        return _mapper.Map<CommandReadDto>(command);
    }
}
