using CommandsService.Data;
using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using Command = CommandsService.Models.Command;

namespace PlatformService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;
    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateCommandAsync(Command command)
    {
        await _context.Commands.AddAsync(command);
    }

    public async Task CreatePlatformAsync(Platform platform)
    {
        await _context.Platforms.AddAsync(platform);
    }

    public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
    {
        return await _context.Platforms.ToArrayAsync();
    }

    public async Task<Command> GetCommandAsync(int id)
    {
        return await _context.Commands.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId)
    {
        return await _context.Commands.Where(c => c.PlatformId == platformId).ToArrayAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
