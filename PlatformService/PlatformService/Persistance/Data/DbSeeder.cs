using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using PlatformService.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Persistance.Data
{
    public static class DbSeeder
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var platformRepo = scope.ServiceProvider.GetService<IPlatformRepository>();
                SeedIfNotSeeded(platformRepo);
            }
        }

        private static void SeedIfNotSeeded(IPlatformRepository repository)
        {
            var currentPlatforms = repository.GetAllPlatforms();
            if (currentPlatforms.Any())
                Console.WriteLine("--> Data Already Exsists, nothing was seeded.");
            else
            {
                var platforms = PreparePlatforms();
                repository.CreatePlatforms(platforms);
                repository.SaveChanges();
                Console.WriteLine("--> Data Seeded Successfully.");
            }
        }

        private static IEnumerable<Platform> PreparePlatforms()
        {
            return new List<Platform> 
            {
                new Platform{Name = "Dot Net", Publisher = "Microsoft", Cost= "Free" },
                new Platform{Name = "SQL Server Express", Publisher = "Microsoft", Cost= "Free" },
                new Platform{Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost= "Free" }
            };
        }
    }
}
