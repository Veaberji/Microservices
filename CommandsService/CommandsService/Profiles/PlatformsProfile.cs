using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace PlatformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
    }
}
