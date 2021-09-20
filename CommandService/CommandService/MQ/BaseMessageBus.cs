using CommandService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace CommandService.MQ
{
    public abstract class BaseMessageBusConsumerClient
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        protected IModel _channel;

        protected BaseMessageBusConsumerClient(IOptions<AppSettings> options)
        {
            var appSettings = options.Value;

            _factory = new ConnectionFactory()
            {
                HostName = appSettings.RabbitMQConfig.Host,
                Port = appSettings.RabbitMQConfig.Port
            };
            
            TryEstablishConnection();
            TryCreateChannel();

            DeclareExchange();
            DeclareQueue();
            BindExchange();
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
            Console.WriteLine("--> Connection Shutdown");
        }

        public abstract void DeclareExchange();
        public abstract void DeclareQueue();
        public abstract void BindExchange();
        
        public abstract void Consume();
    }
}
