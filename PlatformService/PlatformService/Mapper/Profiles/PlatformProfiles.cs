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
            CreateMap<Platform, PlatformItem>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
