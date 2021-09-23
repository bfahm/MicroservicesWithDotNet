using AutoMapper;
using Grpc.Core;
using PlatformService.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.GrpcServers
{
    public class PlatformGrpcService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository platformRepository;
        private readonly IMapper mapper;

        public PlatformGrpcService(IPlatformRepository platformRepository, IMapper mapper)
        {
            this.platformRepository = platformRepository;
            this.mapper = mapper;
        }

        public override Task<GetAllPlatformsResponse> GetAllPlatforms(GetAllPlatformsRequest request, ServerCallContext context)
        {
            var response = new GetAllPlatformsResponse();
            
            var allPlatforms = platformRepository.GetAllPlatforms();

            foreach(var platform in allPlatforms)
            {
                var mappedPlatform = mapper.Map<PlatformItem>(platform);
                response.Platform.Add(mappedPlatform);
            }

            return Task.FromResult(response);
        }
    }
}
