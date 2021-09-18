using PlatformService.Dtos;
using System.Threading.Tasks;

namespace PlatformService.HttpClients.Interfaces
{
    public interface ICommandClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}
