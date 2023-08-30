using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs;
using ScoreTracking.App.DTOs.Requests.Games;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        public async Task CreateGame_Should_Return_Created_Game()
        {
            // Arrange
            var transactionMock = new Mock<IDbContextTransaction>();
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
    }
}
