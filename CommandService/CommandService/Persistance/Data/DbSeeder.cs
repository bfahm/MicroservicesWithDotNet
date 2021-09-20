using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
    }
}
