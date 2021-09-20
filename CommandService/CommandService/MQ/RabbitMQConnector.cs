using CommandService.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace CommandService.MQ
{
    public abstract class RabbitMQConnector
    {
        protected IConnection _connection;

        protected RabbitMQConnector(IOptions<AppSettings> options)
        {
            var appSettings = options.Value;

            var factory = new ConnectionFactory()
            {
                HostName = appSettings.RabbitMQConfig.Host,
                Port = appSettings.RabbitMQConfig.Port
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public abstract void Consume();
    }
}
