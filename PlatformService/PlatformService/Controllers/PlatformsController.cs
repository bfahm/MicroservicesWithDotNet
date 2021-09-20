using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.HttpClients.Interfaces;
using PlatformService.Models;
using PlatformService.MQ;
using PlatformService.MQ.Messages;
using PlatformService.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;
        private readonly ICommandClient commandClient;
        private readonly IMessageBusClient messageBusClient;

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandClient commandClient, IMessageBusClient messageBusClient)
        {
            this.platformRepository = platformRepository;
            this.mapper = mapper;
            this.commandClient = commandClient;
            this.messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> Get()
        {
            var platforms = platformRepository.GetAllPlatforms();
            var response = mapper.Map<List<PlatformReadDto>>(platforms);
            return Ok(response);
        }

        [HttpGet("{id}", Name = nameof(GetById))]
        public ActionResult<PlatformReadDto> GetById(int id)
        {
            var platform = platformRepository.GetPlatformById(id);

            if (platform == null)
                return NotFound();

            var response = mapper.Map<PlatformReadDto>(platform);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = mapper.Map<Platform>(platformCreateDto);
            platformRepository.CreatePlatform(platformModel);
            platformRepository.SaveChanges();

            var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);
            var platformPublishedDto = mapper.Map<PlatformPublishedMessage>(platformReadDto);

            await TryPostToCommandsService(platformReadDto);
            TryPostToMessageBus(platformPublishedDto);

            return CreatedAtRoute(nameof(GetById), new { Id = platformReadDto.Id }, platformReadDto);
        }

        private async Task TryPostToCommandsService(PlatformReadDto platformReadDto)
        {
            try
            {
                await commandClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }
        }

        private void TryPostToMessageBus(PlatformPublishedMessage platformPublishedDto)
        {
            try
            {
                messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }
        }
    }
}
