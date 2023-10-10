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
    public static string Rgb(int r, int g, int b)
    {
        return $"{ToHex(Round(r))}{ToHex(Round(g))}{ToHex(Round(b))}";
    }

    private static int Round(int number)
    {
        number = number < 0 ? 0 : number;
        return number > 255 ? 255 : number;
    }
    private static string ToHex(int number)
    {
        return number.ToString("X2");
    }
} 