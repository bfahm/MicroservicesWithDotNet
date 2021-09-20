using AutoMapper;
using CommandService.Dtos.Commands;
using CommandService.Models;
using CommandService.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly IPlatformRepository _platfromsRepository;
        private readonly ICommandRepository _commandsRepository;
        private readonly IMapper _mapper;

        public CommandsController(IPlatformRepository platfromsRepository, ICommandRepository commandsRepository, IMapper mapper)
        {
            _platfromsRepository = platfromsRepository;
            _commandsRepository = commandsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            bool platformExists = _platfromsRepository.PlaformExits(platformId);
            if (!platformExists)
                return NotFound();

            var commands = _commandsRepository.GetCommandsForPlatform(platformId);
            var response = _mapper.Map<IEnumerable<CommandReadDto>>(commands);

            return Ok(response);
        }

        [HttpGet("{commandId}", Name = nameof(GetCommandForPlatform))]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            bool platformExists = _platfromsRepository.PlaformExits(platformId);
            if (!platformExists)
                return NotFound();

            var command = _commandsRepository.GetCommand(platformId, commandId);

            if (command == null)
                return NotFound();

            var response = _mapper.Map<CommandReadDto>(command);
            return base.Ok(response);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            bool platformExists = _platfromsRepository.PlaformExits(platformId);
            if (!platformExists)
                return NotFound();

            var command = _mapper.Map<Command>(commandDto);

            _commandsRepository.CreateCommand(platformId, command);
            _commandsRepository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}
