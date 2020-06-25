using System;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();

                    // Applies lates migrations in case database is already exists
                    await context.Database.MigrateAsync();

                    // TODO: dispose?
                    var seed = new StoreContextSeed(context, loggerFactory);
                    await seed.SeedAsync();
                    
                    logger.LogInformation("Database migration was triggered.");
                }
                catch(Exception ex)
                {
                    logger.LogError(ex, "An error occured during database migration");
                }

                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
