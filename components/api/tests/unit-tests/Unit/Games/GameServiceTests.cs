using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ScoreTracking.App.Database;
using ScoreTracking.App.Interfaces.Helpers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Unit.Games
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<DatabaseContext> _databaseContext = new Mock<DatabaseContext>();
        private readonly Mock<IGameRepository> _gameRepository = new Mock<IGameRepository>(MockBehavior.Strict);
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        private readonly Mock<IRoundRepository> _roundRepositoryMock = new Mock<IRoundRepository>(MockBehavior.Strict);


        public GameServiceTests()
        {
            _gameService = new GameService(_mapperMock.Object, _databaseContext.Object, _gameRepository.Object, _userRepositoryMock.Object, _roundRepositoryMock.Object);
        }

        [Fact]
        public void CreateGame_Should_Return_Created_Game()
        {
            throw new NotImplementedException();
        }
    }
}
