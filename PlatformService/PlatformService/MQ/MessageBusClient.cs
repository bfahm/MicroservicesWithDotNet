using Microsoft.Extensions.Options;
using PlatformService.Dtos;
using PlatformService.Models;
using RabbitMQ.Client;
using System;

namespace PlatformService.MQ
{
    public class MessageBusClient : BaseMessageBus, IMessageBusClient
    {
        public const string PLATFORM_EXCHANGE = "platform-exchange";
        private IModel _channel;

        public MessageBusClient(IOptions<AppSettings> options) : base(options)
        {
            try
            {
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: PLATFORM_EXCHANGE, type: ExchangeType.Fanout);

                Console.WriteLine($"--> Created a new channel: {_channel}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not create a channel in connection: {_connection}, {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            SendMessage(platformPublishedDto, _channel, PLATFORM_EXCHANGE);
        }
    }
}
