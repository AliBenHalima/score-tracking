using AutoFixture;
using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Database;
using ScoreTracking.App.Enum;
using ScoreTracking.App.Models;

namespace ScoreTracking.IntegrationTests.Seeders.Seeders
{
    public class DatabaseSeeder
    {
        public static void Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            UserSeeder(dbContext);
            GameSeeder(dbContext);
            AddPlayersSeeder(dbContext);
            RoundSeeder(dbContext);
            dbContext.SaveChanges();

        }

        private static void UserSeeder(DatabaseContext dbContext)
        {
            var fixture = new Fixture();
            var users = new Faker<User>()
                         .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                         .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                         .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                         .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
                         .RuleFor(u => u.Password, (f, u) => f.Internet.Password())
                         .Generate(10);

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(users);
            }

        }

        private static void GameSeeder(DatabaseContext dbContext)
        {
            var games = new Faker<Game>()
                         .RuleFor(u => u.Name, (f, u) => f.Lorem.Slug())
                         .RuleFor(u => u.Code, (f, u) => f.Lorem.Word())
                         .RuleFor(u => u.Score, (f, u) => 1000)
                         .RuleFor(u => u.HasJokerPenalty, (f, u) => true)
                         .RuleFor(u => u.JokerPenaltyValue, (f, u) => 50)
                         .Generate(10);

            if (!dbContext.Games.Any())
            {
                dbContext.Games.AddRange(games);
            }
        }

        private static async Task AddPlayersSeeder(DatabaseContext dbContext)
        {
            var random = new Random();
            var users = dbContext.Users.Local.ToList();
            var games = dbContext.Games.Local.ToList();
            foreach (Game game in games)
            {
                var randomUsers = users.OrderBy(x => random.Next()).Take(4).ToList();
                game.Users.AddRange(randomUsers);
            }
        }

        private static void RoundSeeder(DatabaseContext dbContext)
        {
            var faker = new Faker();
            var games = dbContext.Games.Local.ToList();
            foreach (var game in games)
            {
                var round = new Faker<Round>()
                       .RuleFor(u => u.Number, (f, u) => f.Random.Number())
                       .RuleFor(u => u.Status, (f, u) => f.PickRandom<GameRoundStatus>())
                       .Generate();
                dbContext.Rounds.Add(round);
                foreach (var userGame in game.UserGames)
                {
                    dbContext.UserGameRounds.Add(new UserGameRound()
                    {
                        UserGameId = userGame.Id,
                        RoundId = round.Id,
                        Jokers = 50,
                        Score = faker.Random.Number(100, 200),
                    });

                }
            }
        }

    }
}
