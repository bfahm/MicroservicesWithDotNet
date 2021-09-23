using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using PlatformService;
using System;
using System.Collections.Generic;

namespace CommandService.GrpcClient
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public PlatformDataClient(IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_appSettings.GrpcPlatform}");

            var channel = GrpcChannel.ForAddress(_appSettings.GrpcPlatform);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllPlatformsRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                var mappedReply = _mapper.Map<IEnumerable<Platform>>(reply.Platform);
                return mappedReply;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}
