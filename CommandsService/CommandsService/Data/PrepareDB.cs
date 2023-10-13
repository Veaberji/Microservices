using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

namespace CommandsService.Data;

public static class PrepareDB
{
    public static async Task PopulateAsync(this WebApplication app, bool isProduction)
    {
        using var scope = app.Services.CreateScope();
        var grpcClient = scope.ServiceProvider.GetService<IPlatformDataClient>();
        var repo = scope.ServiceProvider.GetService<ICommandRepository>();

        await SeedDataAsync(grpcClient, repo, isProduction);
    }

    private static async Task SeedDataAsync(IPlatformDataClient grpcClient, ICommandRepository repo, bool isProduction)
    {
        var data = grpcClient.GetPlatforms();
        foreach (var platform in data) 
        {
            if (await repo.IsExternalPlatformExistsAsync(platform.Id)) return;

            await repo.CreatePlatformAsync(platform);
        }

        await repo.SaveChangesAsync();
    }
}
