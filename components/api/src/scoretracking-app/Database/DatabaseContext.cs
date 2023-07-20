using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ScoreTracking.App.Models;


namespace ScoreTracking.App.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UserGames { get; set; }

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

            modelBuilder.Entity<User>()
            .HasMany(e => e.Games)
            .WithMany(e => e.Users)
            .UsingEntity<UserGame>();

            modelBuilder.Entity<Game>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Games)
            .UsingEntity<UserGame>();

            modelBuilder.Entity<UserGame>()
            .HasKey(ug => new { ug.UserId, ug.GameId});

            modelBuilder.Entity<UserGame>()
                .HasOne(u => u.User)
                .WithMany(ug => ug.UserGames)
                .HasForeignKey(u => u.UserId);
                
            modelBuilder.Entity<UserGame>()
                .HasOne(g => g.Game)
                .WithMany(ug => ug.UserGames)
                .HasForeignKey(g => g.GameId);
        }

    }
}