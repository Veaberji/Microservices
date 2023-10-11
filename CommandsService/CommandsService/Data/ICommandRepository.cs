using CommandsService.Models;

namespace PlatformService.Data;

public interface ICommandRepository
{
    Task CreatePlatformAsync(Platform platform);

    Task<IEnumerable<Platform>> GetAllPlatformsAsync();

    Task CreateCommandAsync(Command command);

    Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId);

    Task<Command> GetCommandAsync(int id);

    Task<bool> SaveChangesAsync();
}
