using CommandService.Core;
using CommandService.Helpers;
using CommandService.Models;
using CommandService.MQ;
using CommandService.MQ.BackgroundServices;
using CommandService.Persistance.Data;
using CommandService.Persistance.Interfaces;
using CommandService.Persistance.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            var appSettings = Configuration.Get<AppSettings>();

            if (_env.IsProduction())
            {
                Console.WriteLine($"--> Using SQL Server Database");
                var connectionString = ConnectionStringUtils.Prepare(appSettings.DatabaseConnectionString,
                                                                 appSettings.DatabasePassword);

                Console.WriteLine($"Current connection string: {connectionString}");

                services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseSqlServer(connectionString);
                });
            }
            else
            {
                Console.WriteLine($"--> Using InMem Database");
                services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase("InMem");
                });
            }

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddScoped<ICommandRepository, CommandRepository>();
            
            services.AddSingleton<PublishedPlatformChannel>();

            services.AddScoped<PublishedPlatformCore>();
            services.AddHostedService<PublishedPlatformHostedService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CommandService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommandService v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.SeedData();
        }
    }
}
