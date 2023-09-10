using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests.Authentication;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Providers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Unit.Users
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IJwtProvider> _jwtProviderMock = new Mock<IJwtProvider>();
        private readonly Mock<IUploadFileProvider> _uploadFileProvider = new Mock<IUploadFileProvider>();
        public AuthenticationServiceTests()
        {
            _sut = new AuthenticationService(_unitOfWorkMock.Object, _mapperMock.Object, _userRepositoryMock.Object, _jwtProviderMock.Object, _uploadFileProvider.Object);
        }

        [Fact]
        public async Task Register_Should_Regsiter_New_User()
        {
            // Arrange
            var fixture = new Fixture();
            var userByEmail = fixture.Build<User>()
                              .Without(u => u.Games)
                              .Without(u => u.UserGames)
                              .Create();
            var registerUserRequest = fixture.Create<RegisterUserRequest>();
            var createdUser = new User
            {
                FirstName = registerUserRequest.FirstName,
                LastName = registerUserRequest.LastName,
                Email = registerUserRequest.Email,
                Phone = registerUserRequest.Phone,
                Password = registerUserRequest.Password,
            };

            _userRepositoryMock.Setup(u => u.FindByEmail(It.IsAny<string>())).ReturnsAsync(() => null);
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<RegisterUserRequest>())).Returns(createdUser);
            _userRepositoryMock.Setup(u => u.Create(It.IsAny<User>())).ReturnsAsync(createdUser);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.Register(registerUserRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(createdUser);
        }

        [Fact]
        public async Task Register_Should_Throw_Exception_When_Email_Exists()
        {
            // Arrange
            var fixture = new Fixture();
            var userByEmail = fixture.Build<User>()
                              .Without(u => u.Games)
                              .Without(u => u.UserGames)
                              .Create();
            var registerUserRequest = fixture.Create<RegisterUserRequest>();
            _userRepositoryMock.Setup(u => u.FindByEmail(It.IsAny<string>())).ReturnsAsync(userByEmail);

            // Assert
            //Assertion using fluent assertion.
            await _sut.Invoking(auth => auth.Register(registerUserRequest))
            .Should().ThrowAsync<RessourceNotFoundException>()
            .WithMessage($"{nameof(RegisterUserRequest.Email)} Already Exist");

            //Assertion using xUnit assertion.
            //await Assert.ThrowsAsync<RessourceNotFoundException>(async () => await _sut.Register(registerUserRequest));

        }

        [Fact]
        public async Task SignIn_Should_Return_Valid_Token()
        {

            // Arrange
            var fixture = new Fixture();
            var user = fixture.Build<User>()
                                .Without(u => u.Games)
                                .Without(u => u.UserGames)
                                .With(u => u.Password, "password")
                                .Create();
            var signinUserRequest = fixture.Build<SigninUserRequest>()
                                           .With(u => u.Password, "password")
                                           .Create();
            _userRepositoryMock.Setup(u => u.FindByEmail(It.IsAny<string>())).ReturnsAsync(user);
            _jwtProviderMock.Setup(t => t.Generate(user)).Returns("Token");
            // Act

            // Assert
            await _sut.Invoking(x => x.Signin(signinUserRequest))
                .Should().NotThrowAsync<BadRequestException>();


        }
    }
}
