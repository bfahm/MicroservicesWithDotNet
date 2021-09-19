using AutoMapper;
using CommandService.Dtos.Commands;
using CommandService.Models;

namespace PlatformService.Mapper.Profiles
{
    public class CommandProfiles : Profile
    {
        public CommandProfiles()
        {
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
        }
    }
}
