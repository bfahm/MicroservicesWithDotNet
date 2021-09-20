using Microsoft.Extensions.Options;
using PlatformService.Dtos;
using PlatformService.Models;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.MQ
{
    public class MessageBusClient : IMessageBusClient
    {
        private const string EXCHANGE_NAME = "platform-exchange";
        private IConnection _connection;
        private IModel _channel;

        public MessageBusClient(IOptions<AppSettings> options)
        {
            var appSettings = options.Value;

            var factory = new ConnectionFactory()
            {
                HostName = appSettings.RabbitMQConfig.Host,
                Port = appSettings.RabbitMQConfig.Port
            };

            try
            {
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: ExchangeType.Fanout);

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            SendMessage(platformPublishedDto);
        }

        private void SendMessage<T>(T messageObject)
        {
            var message = JsonSerializer.Serialize(messageObject);
            var body = Encoding.UTF8.GetBytes(message);

            if (!_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is closed, msg not sent");
                return;
            }

            _channel.BasicPublish(exchange: EXCHANGE_NAME, routingKey: string.Empty, basicProperties: null, body: body);
            Console.WriteLine($"--> Msg sent: {message}");
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
