using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScoreTracking.App.Database;
using ScoreTracking.App.Database.Seeders;
using ScoreTracking.App.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace ScoreTracking.App.Extensions.Database
{
    public static class DatabaseSeederExtension
    {
        public static async Task<IHost> DatabaseSeederAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var dbseeder = new DatabaseSeeder(dbContext);
                await dbseeder.Seed();
            } catch(Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<IModuleMarker>>();
                logger.LogError(ex, ex.Message);
            }
            return host;

        }
    }
}
