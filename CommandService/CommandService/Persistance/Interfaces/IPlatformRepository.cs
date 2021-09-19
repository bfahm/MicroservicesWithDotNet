using CommandService.Models;
using System.Collections.Generic;

namespace CommandService.Persistance.Interfaces
{
    public interface IPlatformRepository
    {
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);
        void CreatePlatforms(IEnumerable<Platform> plats);
        bool PlaformExits(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);
        bool SaveChanges();
    }
}
