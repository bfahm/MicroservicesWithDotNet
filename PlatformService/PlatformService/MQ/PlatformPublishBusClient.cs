using Microsoft.Extensions.Options;
using PlatformService.Models;
using PlatformService.MQ.Messages;

namespace PlatformService.MQ
{
    public class PlatformPublishBusClient : BaseMessageBusPublisherClient, IPlatformPublishBusClient
    {
        private const string ROUTING_KEY = "platforms-new-publish"; 

        public PlatformPublishBusClient(IOptions<AppSettings> options) : base(options)
        {
        }

        public void PublishNewPlatform(PlatformPublishedMessage platformPublishedDto)
        {
            SendMessage(platformPublishedDto, ROUTING_KEY);
        }
    }
}
