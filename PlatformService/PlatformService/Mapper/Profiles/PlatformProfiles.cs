using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.MQ.Messages;

namespace PlatformService.Mapper.Profiles
{
    public class PlatformProfiles : Profile
    {
        public PlatformProfiles()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedMessage>();
        }
    }
}
