using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;
    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Platform platform)
    {
        await _context.Platforms.AddAsync(platform);
    }

    public async Task<Platform> GetByIdAsync(int id)
    {
        return await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Platform>> GetAllAsync()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
