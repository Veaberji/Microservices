using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepository
{
    Task CreateAsync(Platform platform);

    Task<Platform> GetByIdAsync(int id);

    Task<IEnumerable<Platform>> GetAllAsync();

    Task<bool> SaveChangesAsync();
}
