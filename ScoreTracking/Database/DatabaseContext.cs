using System;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.Models;

namespace Common.WebApi.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> users { get; set; }

        protected readonly IConfiguration _configuration;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    FirstName = "Robert",
                    LastName = "Lara",
                },
                new User()
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Doe",
                }
                );
        }

    }
}