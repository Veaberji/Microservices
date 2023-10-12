using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService.Data;
using System.Text.Json;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private IServiceScopeFactory _factory;
    private IMapper _mapper;

    public EventProcessor(IServiceScopeFactory factory, IMapper mapper)
    {
        _factory = factory;
        _mapper = mapper;
    }

    public async Task ProcessEventAsync(string message)
    {
        if (DetermineEvent(message) != EventType.Published)
        {
            return;
        }

        using var scope = _factory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
        var platform = JsonSerializer.Deserialize<Platform>(message);

        if (!await repo.IsExternalPlatformExistsAsync(platform.ExternalId)) 
        {
            await repo.CreatePlatformAsync(_mapper.Map<Platform>(platform));
            await repo.SaveChangesAsync();
        }

    }

    private static EventType DetermineEvent(string message) 
    {
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

        return eventType.Event == "Platform Published" ? EventType.Published : EventType.Undetermined;
    }
}
