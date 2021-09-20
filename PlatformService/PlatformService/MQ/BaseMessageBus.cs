using Microsoft.Extensions.Options;
using PlatformService.Models;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.MQ
{
    public class BaseMessageBus
    {
        protected IConnection _connection;

        public BaseMessageBus(IOptions<AppSettings> options)
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

                Console.WriteLine("--> Connected to MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        protected void SendMessage<T>(T messageObject, IModel channel, string exchange)
        {
            var message = JsonSerializer.Serialize(messageObject);
            var body = Encoding.UTF8.GetBytes(message);

            if (!_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is closed, msg not sent");
                return;
            }

            channel.BasicPublish(exchange: exchange, routingKey: string.Empty, basicProperties: null, body: body);
            Console.WriteLine($"--> Msg sent: {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
