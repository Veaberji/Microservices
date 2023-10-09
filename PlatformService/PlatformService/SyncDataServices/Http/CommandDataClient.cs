using PlatformService.Dtos;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http;

public class CommandDataClient : ICommandDataClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public CommandDataClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platform)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(platform),
            Encoding.UTF8,
            "application/json");

        await _http.PostAsync($"{_config["CommandService"]}", content);
    }
} 