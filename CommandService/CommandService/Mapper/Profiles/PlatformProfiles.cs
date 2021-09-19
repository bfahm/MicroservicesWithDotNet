using AutoMapper;
using CommandService.Dtos.Platforms;
using CommandService.Models;

namespace CommandService.Mapper.Profiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
            CreateMap<Platform, PlatformreadDto>();
        }
    }
}
