using System.Net;
using Api.Data;
using Api.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;


namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var logger = provider.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = provider.GetRequiredService<VaccineContext>();
                    var companyRepository = provider.GetService<ICompanyRepository>();
                    context.Database.Migrate();
                    Seed.SeedCompanies(companyRepository, logger);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during migration");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
             .ConfigureLogging(logging =>
              {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
                logging.SetMinimumLevel(LogLevel.Information);
              })
              .ConfigureLogging((hostingContext, builder) =>
                {
                    var configuration = hostingContext.Configuration.GetSection("Logging");
                    builder.AddFile(configuration);
                })
             .UseStartup<Startup>();
                
    }
}
