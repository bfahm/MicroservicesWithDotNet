using PlatformService.MQ.Messages;

namespace PlatformService.MQ
{
    public interface IPlatformPublishBusClient
    {
        void PublishNewPlatform(PlatformPublishedMessage platformPublishedDto);
    }
}
