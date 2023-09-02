
using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.Models;
using ScoreTracking.IntegrationTests.Seeders.Seeders;
using System.ComponentModel;
using System.Data.Common;


namespace ScoreTracking.Integ.Tests
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DatabaseContext>));

                services.Remove(dbContextDescriptor);

                services.AddDbContext<DatabaseContext>((container, options) =>
                {
                    options.UseInMemoryDatabase(databaseName: "InMemoryDB");
                });
            });

        }
        public void SeedDatabase()
        {
            DatabaseSeeder.Seed(Services);
        }


    }

}
