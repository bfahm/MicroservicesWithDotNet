using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Persistance.Interfaces
{
    public interface IPlatformRepository
    {
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);
        void CreatePlatforms(IEnumerable<Platform> plats);
        bool SaveChanges();
    }
}
