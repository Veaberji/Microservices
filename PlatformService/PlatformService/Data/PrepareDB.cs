using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepareDB
{
    public static async Task PopulateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await SeedDataAsync(scope.ServiceProvider.GetService<AppDbContext>());
    }

    private static async Task SeedDataAsync(AppDbContext context)
    {
        if (context.Platforms.Any())
        {
            return;
        }

        var data = new List<Platform>()
        {
            new () { Name = "Name 1", Publisher = "Publisher 1", Cost = "Free" },
            new () { Name = "Name 2", Publisher = "Publisher 2", Cost = "Free" },
            new () { Name = "Name 3", Publisher = "Publisher 3", Cost = "Free" },
        };
        context.Platforms.AddRange(data);

        await context.SaveChangesAsync();
    }
}
