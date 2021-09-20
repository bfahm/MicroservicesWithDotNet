using CommandService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace CommandService.MQ
{
    public class PublishedPlatformChannel : RabbitMQConnector
    {
        public const string PLATFORM_EXCHANGE = "platform-exchange";
        public EventingBasicConsumer Consumer { get; }

        private IModel _channel;
        private string _queueName;

        public PublishedPlatformChannel(IOptions<AppSettings> options) : base(options)
        {
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: PLATFORM_EXCHANGE, type: ExchangeType.Fanout);

            _queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: _queueName, exchange: PLATFORM_EXCHANGE, routingKey: "");

            Console.WriteLine("--> Listenting on the Message Bus...");

            Consumer = new EventingBasicConsumer(_channel);
        }

        public override void Consume()
        {
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: Consumer);
        }
    }
}
