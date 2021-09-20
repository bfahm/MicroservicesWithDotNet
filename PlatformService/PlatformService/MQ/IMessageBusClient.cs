using PlatformService.Dtos;

namespace PlatformService.MQ
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}
