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
        public DbSet<Round> Rounds { get; set; }
        public DbSet<UserGameRound> UserGameRounds { get; set; }

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
            //

            modelBuilder.Entity<User>()
            .HasMany(e => e.Games)
            .WithMany(e => e.Users)
            .UsingEntity<UserGame>();

            modelBuilder.Entity<User>().Property<uint>("Version").IsRowVersion();

            modelBuilder.Entity<Game>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Games)
            .UsingEntity<UserGame>();

            modelBuilder.Entity<UserGame>()
            .HasKey(ug => new { ug.Id});

            modelBuilder.Entity<UserGame>()
                .HasOne(u => u.User)
                .WithMany(ug => ug.UserGames)
                .HasForeignKey(u => u.UserId);
                
            modelBuilder.Entity<UserGame>()
                .HasOne(g => g.Game)
                .WithMany(ug => ug.UserGames)
                .HasForeignKey(g => g.GameId);

            ////
            modelBuilder.Entity<Round>()
           .HasMany(e => e.UserGames)
           .WithMany(e => e.Rounds)
           .UsingEntity<UserGameRound>(
            j => j.HasOne(ugrl => ugrl.UserGame).WithMany(),
            j => j.HasOne(ugrl => ugrl.Round).WithMany()
        );

            modelBuilder.Entity<UserGame>()
            .HasMany(e => e.Rounds)
            .WithMany(e => e.UserGames)
            .UsingEntity<UserGameRound>(
              j => j.HasOne(ugrl => ugrl.Round).WithMany(),
              j => j.HasOne(ugrl => ugrl.UserGame).WithMany()
        );


            modelBuilder.Entity<UserGameRound>()
           .HasKey(ug => ug.Id);

            modelBuilder.Entity<UserGameRound>()
                .HasOne(u => u.UserGame)
                .WithMany(ug => ug.UserGameRounds)
                .HasForeignKey(u => u.UserGameId);

            modelBuilder.Entity<UserGameRound>()
                .HasOne(g => g.Round)
                .WithMany(ug => ug.UserGameRounds)
                .HasForeignKey(g => g.RoundId);
        }

    }
}