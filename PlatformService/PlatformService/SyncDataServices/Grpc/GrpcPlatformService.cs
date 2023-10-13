using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using static PlatformService.GrpcPlatform;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : GrpcPlatformBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;

    public GrpcPlatformService(IPlatformRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public override async Task<PlatformResponce> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var responce = new PlatformResponce();
        var platforms = await _repository.GetAllAsync();

        foreach (var platform in platforms)
        {
            responce.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
        }

        return responce;
    }

}
