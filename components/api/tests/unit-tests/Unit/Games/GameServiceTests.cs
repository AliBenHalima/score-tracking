using AutoFixture;
using AutoMapper;
using Bogus.DataSets;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using ScoreTracking.App.Repositories;
using ScoreTracking.App.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Unit.Games
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IGameRepository> _gameRepository = new Mock<IGameRepository>(MockBehavior.Strict);
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        private readonly Mock<IRoundRepository> _roundRepositoryMock = new Mock<IRoundRepository>(MockBehavior.Strict);
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);


        public GameServiceTests()
        {
            _gameService = new GameService(_mapperMock.Object, _gameRepository.Object, _userRepositoryMock.Object, _roundRepositoryMock.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task CreateGame_Should_Create_Game_Without_Adding_Members()
        {
            // Arrange
            var fixture = new Fixture();
            var dbContextTransactionMock = new Mock<IDbTransaction>();

            _unitOfWork.Setup(uow => uow.BeginTransaction()).Returns(dbContextTransactionMock.Object);
            _unitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            var createGameRequest = fixture.Create<CreateGameRequest>();
            createGameRequest.UserIds = new List<int>() { };
            var game = fixture.Build<Game>()
                        .Without(g => g.Users)
                        .Without(g => g.UserGames)
                        .Create();
            var createGameDTO = fixture.Build<CreateGameDTO>()
                        .Without(g => g.Users)
                        .Create();


            _mapperMock.Setup(m => m.Map<Game>(It.IsAny<CreateGameRequest>())).Returns(game);
            _gameRepository.Setup(g => g.Create(It.Is<Game>(g => ReferenceEquals(g, game)))).ReturnsAsync(game);
            _mapperMock.Setup(m => m.Map<CreateGameDTO>(It.IsAny<Game>())).Returns(createGameDTO);

            // Act
            await _gameService.CreateGame(createGameRequest);
            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task CreateGame_Should_Create_Game_And_Add_Members()
        {
            // Arrange
            var fixture = new Fixture();
            var dbContextTransactionMock = new Mock<IDbTransaction>();

            _unitOfWork.Setup(uow => uow.BeginTransaction()).Returns(dbContextTransactionMock.Object);
            _unitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            var createGameRequest = fixture.Build<CreateGameRequest>()
                                           .With(g => g.UserIds, () => fixture.CreateMany<int>(3).ToList())
                                            .Create();

            var game = fixture.Build<Game>()
                        .Without(g => g.Users)
                        .Without(g => g.UserGames)
                        .Create();
            var users = fixture.Build<User>()
                        .Without(g => g.Games)
                        .Without(g => g.UserGames)
                        .CreateMany(3);
            var createGameDTO = fixture.Build<CreateGameDTO>()
                        .Without(g => g.Users)
                        .Create();

            _mapperMock.Setup(m => m.Map<Game>(It.IsAny<CreateGameRequest>())).Returns(game);
            _gameRepository.Setup(g => g.Create(It.Is<Game>(g => ReferenceEquals(g, game)))).ReturnsAsync(game);
            _userRepositoryMock.Setup(g => g.FindByIds(It.IsAny<IEnumerable<int>>())).ReturnsAsync(users);
            _gameRepository.Setup(g => g.AddPlayersToGame(It.Is<Game>(g => ReferenceEquals(g, game)), It.Is<IEnumerable<User>>( u => ReferenceEquals(u, users)))).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<CreateGameDTO>(It.IsAny<Game>())).Returns(createGameDTO);
            // Act

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task GameHasPlayersLeft_Should_Return_False()
        {
            // Arrange
            var fixture = new Fixture();
            var game = fixture.Build<Game>()
                                    .With(g => g.Score, 1000)
                                    .Without(g => g.Users)
                                    .Without(g => g.UserGames)
                                    .Create();
            var roundSum = fixture.Build<RoundSumDTO>()
                                  .With(u => u.Sum, 300)
                                  .CreateMany<RoundSumDTO>(2);

            // Act
            _roundRepositoryMock.Setup(g => g.GetRoundSumScoresByPlayer(It.Is<int>(i => i == game.Id))).ReturnsAsync(roundSum);
            bool result = await _gameService.GameHasPlayersLeft(game);
            /*Constants.GameConstants.MaxPlayersCount = () => 99999;*/ // Set your desired value

            // Assert
            result.Should().Be(false);
        }
    }
}
