using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using PlatformService.HttpClients.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.HttpClients.Services
{
    public class CommandClient : ICommandClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CommandClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var requestPayload = new StringContent(JsonSerializer.Serialize(plat),
                                                   Encoding.UTF8,
                                                   "application/json");

            var uri = configuration["CommandService"];
            var response = await httpClient.PostAsync(uri, requestPayload);

            if(response.IsSuccessStatusCode)
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            else
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
        }
    }
}
