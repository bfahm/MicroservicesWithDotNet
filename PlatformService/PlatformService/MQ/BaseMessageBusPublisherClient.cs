using Microsoft.Extensions.Options;
using PlatformService.Models;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.MQ
{
    public class BaseMessageBusPublisherClient
    {
        private const string PLATFORM_EXCHANGE = "platform-exchange";

        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public BaseMessageBusPublisherClient(IOptions<AppSettings> options)
        {
            var appSettings = options.Value;

            _factory = new ConnectionFactory()
            {
                HostName = appSettings.RabbitMQConfig.Host,
                Port = appSettings.RabbitMQConfig.Port
            };

            TryEstablishConnection();
            TryCreateChannel();
            TryDeclareExchange();
        }

        protected void SendMessage<T>(T messageObject, string routingKey)
        {
            var message = JsonSerializer.Serialize(messageObject);
            var body = Encoding.UTF8.GetBytes(message);

            if (!_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is closed, msg not sent");
                return;
            }

            _channel.BasicPublish(exchange: PLATFORM_EXCHANGE, routingKey: routingKey, basicProperties: null, body: body);
            Console.WriteLine($"--> Msg sent: {message}");
        }

        private void TryDeclareExchange()
        {
            try
            {
                if (_channel.IsOpen)
                {
                    _channel.ExchangeDeclare(exchange: PLATFORM_EXCHANGE, type: ExchangeType.Direct);

                    Console.WriteLine($"--> Declare Exchange on channel: {_channel}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not declare and exchange in connection: {_connection}, channel: {_channel}, {ex.Message}");
            }
        }

        private void TryCreateChannel()
        {
            try
            {
                if (_connection.IsOpen)
                {
                    _channel = _connection.CreateModel();

                    Console.WriteLine($"--> Created a new channel: {_channel}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not create a channel in connection: {_connection}, {ex.Message}");
            }
        }

        private void TryEstablishConnection()
        {
            try
            {
                _connection = _factory.CreateConnection();
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
