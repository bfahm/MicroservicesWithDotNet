using CommandService.GrpcClient;
using CommandService.Models;
using CommandService.Persistance.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace CommandService.Persistance.Data
{
    public static class DbSeeder
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var environment = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                
                if (environment.IsProduction())
                {
                    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                    TryMigrate(dbContext);
                }

                var platformRepo = scope.ServiceProvider.GetService<IPlatformRepository>();
                var grpcService = scope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcService.ReturnAllPlatforms();
                SeedData(platformRepo, platforms);
            }
        }

        private static void TryMigrate(AppDbContext dbContext)
        {
            try
            {
                Console.WriteLine($"--> Attempting Migrations..");
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        private static void SeedData(IPlatformRepository repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var plat in platforms ?? new List<Platform>())
            {
                if (!repo.ExternalPlatformExists(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}
