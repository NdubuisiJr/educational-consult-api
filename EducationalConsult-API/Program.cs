using EducationalConsultAPI.DBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace EducationalConsultAPI {
    public class Program {
        public static void Main(string[] args) {
            var host = CreateHostBuilder(args).Build();
            ApplyMigrations(host);
            host.Run();
        }

        private static void ApplyMigrations(IHost host) {
            // migrate the database.  Best practice = in Main, using service scope
            using (var scope = host.Services.CreateScope()) {
                try {
                    var context = scope.ServiceProvider.GetService<EducationalDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex) {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "4000";
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:" + $"{port}");
                });
    }
}
