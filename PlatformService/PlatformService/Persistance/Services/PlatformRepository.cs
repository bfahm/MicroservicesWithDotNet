using PlatformService.Models;
using PlatformService.Persistance.Data;
using PlatformService.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Persistance.Services
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
