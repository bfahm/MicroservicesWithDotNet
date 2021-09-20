using PlatformService.MQ.Messages;

namespace PlatformService.MQ
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedMessage platformPublishedDto);
    }
}
