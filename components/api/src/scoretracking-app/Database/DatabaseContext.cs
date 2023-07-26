using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScoreTracking.App.Models;


namespace ScoreTracking.App.Database
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
                    Email = "Test1@gmail.com",
                    Phone = "+21600000001",
                },
                new User()
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "Test2@gmail.com",
                    Phone = "+21600000002",
                }
                );
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Phone).IsUnique();
        }

    }
}