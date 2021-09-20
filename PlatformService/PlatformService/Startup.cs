using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.Helpers;
using PlatformService.HttpClients.Interfaces;
using PlatformService.HttpClients.Services;
using PlatformService.Models;
using PlatformService.MQ;
using PlatformService.Persistance.Data;
using PlatformService.Persistance.Interfaces;
using PlatformService.Persistance.Services;
using System;

namespace PlatformService
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


        // This method gets called by the runtime. Use this method to add services to the container.
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

            services.AddHttpClient<ICommandClient, CommandClient>();

            services.AddSingleton<IPlatformPublishBusClient, PlatformPublishBusClient>();

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"Commands Service API is hosted at: {appSettings.CommandService}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));

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
