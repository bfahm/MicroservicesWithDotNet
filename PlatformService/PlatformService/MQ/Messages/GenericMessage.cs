namespace PlatformService.MQ.Messages
{
    // This class should be part of a shared library
    public class GenericMessage<T>
    {
        public GenericMessage(string @event, T payload)
        {
            Event = @event;
            Payload = payload;
        }

        public string Event { get; set; }
        public T Payload { get; set; }
    }
}
