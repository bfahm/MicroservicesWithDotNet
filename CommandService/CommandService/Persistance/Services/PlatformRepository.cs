using CommandService.Models;
using CommandService.Persistance.Data;
using CommandService.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandService.Persistance.Services
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;

        public PlatformRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
                throw new ArgumentException(nameof(plat));

            _context.Platforms.Add(plat);
        }

        public void CreatePlatforms(IEnumerable<Platform> plats)
        {
            if (plats == null)
                throw new ArgumentException(nameof(plats));

            _context.Platforms.AddRange(plats);
        }

        public bool PlaformExits(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.ExternalID == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
