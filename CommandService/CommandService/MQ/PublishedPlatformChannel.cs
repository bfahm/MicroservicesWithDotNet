using CommandService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace CommandService.MQ
{
    public class PlatformPublishBusClient : BaseMessageBusConsumerClient
    {
        private const string PLATFORM_EXCHANGE = "platform-exchange";
        private const string PLATFORM_PUBLISH_ROUTING_KEY = "platforms-new-publish";
        
        public EventingBasicConsumer Consumer { get; }

        private string _queueName;

        public PlatformPublishBusClient(IOptions<AppSettings> options) : base(options)
        {
            Consumer = new EventingBasicConsumer(_channel);
            Console.WriteLine("--> Listenting on the Platform Publish Message Bus...");
        }

        public override void DeclareExchange()
        {
            _channel.ExchangeDeclare(exchange: PLATFORM_EXCHANGE, type: ExchangeType.Direct);
        }

        public override void DeclareQueue()
        {
            _queueName = _channel.QueueDeclare().QueueName;
        }

        public override void BindExchange()
        {
            _channel.QueueBind(queue: _queueName, exchange: PLATFORM_EXCHANGE, routingKey: PLATFORM_PUBLISH_ROUTING_KEY);
        }

        public override void Consume()
        {
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: Consumer);
        }
    }
}
