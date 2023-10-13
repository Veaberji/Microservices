using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private IConfiguration _config;
    private IMapper _mapper;

    public PlatformDataClient(IConfiguration config, IMapper mapper)
    {
        _config = config;
        _mapper = mapper;
    }

    public IEnumerable<Platform> GetPlatforms()
    {
        var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Enumerable.Empty<Platform>();
        }
    }
}