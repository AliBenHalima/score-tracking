using AutoFixture;
using FluentAssertions;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Unit.Repositories
{
    public class GameRepositoryTests
    {
        private readonly InMemoryDatabaseContext _context;
        public GameRepositoryTests()
        {
            _context = new InMemoryDatabaseContext();
        }

        [Fact]
        public async Task GetGamesByUser_Should_Return_Games_By_User()
        {
            // Arrange
            var context = _context.CreateContextForInMemory();
            var fixture = new Fixture();
            IEnumerable<Game> games = fixture.Build<Game>()
                                    .Without(g => g.Users)
                                    .Without(g => g.UserGames)
                                    .CreateMany(2);
            var user = fixture.Build<User>()
                                    .Without(u => u.Games)
                                    .Without(u => u.UserGames)
                                    .Create();

            // Act
            context.Users.Add(user);
            context.Games.AddRange(games);
            foreach (var game in games)
            {
                user.Games.Add(game);
            }
            await context.SaveChangesAsync();
            // Assert
            var dbUser = await context.Users.FindAsync(user.Id);
            dbUser.Games.Should().NotBeNull();
            dbUser.Games.Should().Contain(game => games.Contains(game));
        }

    }
}
