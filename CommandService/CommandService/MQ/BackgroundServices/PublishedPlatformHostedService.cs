using CommandService.Core;
using CommandService.Dtos.Platforms;
using CommandService.MQ.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CommandService.MQ.BackgroundServices
{
    public class PublishedPlatformHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PublishedPlatformChannel _publishedPlatformBusSubscriber;

        public PublishedPlatformHostedService(PublishedPlatformChannel publishedPlatformBusSubscriber, IServiceScopeFactory scopeFactory)
        {
            _publishedPlatformBusSubscriber = publishedPlatformBusSubscriber;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var scope = _scopeFactory.CreateScope();

            _publishedPlatformBusSubscriber.Consumer.Received += (moduleHandle, eventArgs) =>
            {
                Console.WriteLine("--> Event Received!");

                var body = eventArgs.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                var coreService = scope.ServiceProvider.GetRequiredService<PublishedPlatformCore>();

                var messageObject = JsonSerializer.Deserialize<GenericMessage<PlatformPublishedMessage>>(notificationMessage);
                coreService.AddPlatform(new PlatformPublishedDto
                {
                    // TODO: Use Automapper
                    Event = messageObject.Event,
                    Id = messageObject.Payload.Id,
                    Name = messageObject.Payload.Name
                });

            };

            _publishedPlatformBusSubscriber.Consume();
            
            return Task.CompletedTask;
        }
    }
}
