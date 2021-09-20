using AutoMapper;
using CommandService.Dtos.Platforms;
using CommandService.Models;
using CommandService.Persistance.Interfaces;
using System;

namespace CommandService.Core
{
    public class PublishedPlatformCore
    {
        private readonly IPlatformRepository _platfromsRepository;
        private readonly IMapper _mapper;

        public PublishedPlatformCore(IPlatformRepository platfromsRepository, IMapper mapper)
        {
            _platfromsRepository = platfromsRepository;
            _mapper = mapper;
        }

        public void AddPlatform(PlatformPublishedDto platformPublishedMessage)
        {
            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedMessage);
                if (!_platfromsRepository.ExternalPlatformExists(plat.ExternalID))
                {
                    _platfromsRepository.CreatePlatform(plat);
                    _platfromsRepository.SaveChanges();

                    Console.WriteLine("--> Platform added!");
                }
                else
                    Console.WriteLine("--> Platform already exisits...");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
            }
        }
    }
}
