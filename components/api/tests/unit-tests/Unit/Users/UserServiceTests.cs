using AutoFixture;
using AutoMapper;
using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using ScoreTracking.App.Database;
using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.DTOs.Requests.Users;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Helpers;
using ScoreTracking.App.Interfaces.Repositories;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Unit.Users
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        private readonly Mock<ILogger<UserService>> _loggerMock = new Mock<ILogger<UserService>>();
        private readonly Mock<IApplicationHelper> _globalHelperMock = new Mock<IApplicationHelper>();
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);


        public UserServiceTests()
        {
            _userService = new UserService(_unitOfWork.Object, _mapperMock.Object, _userRepositoryMock.Object, _loggerMock.Object, _globalHelperMock.Object);
        }
        [Fact]
        public void GetUsers_Should_Return_List_Of_Users()
        {
            // Arrange
            //var filterDto = new FilterDTO
            //{
            //    SearchTerm = "test",
            //    Page = 1,
            //    PageSize = 15
            //};
            //var usersQuery = new List<User>
            //{
            //    new User { Id = 1, FirstName = "Alice", LastName = "John", Email = "a@gmailcom", Phone = "+216548748945" },
            //}.AsQueryable();
            //// Act
            //_userRepositoryMock.Setup(x => x.FindAll(filterDto, It.IsAny<CancellationToken>())).Returns(usersQuery);
            //var query = _userService.GetUsers(filterDto, It.IsAny<CancellationToken>());
            //// Assert
            //Assert.Equal(usersQuery, query);
        }

        [Fact]
        public async Task GetUser_Should_Return_User_When_User_Exists()
        {
            // Arrange
            var user = new User { Id = 1, FirstName = "Alice", LastName = "John", Email = "a@gmailcom", Phone = "+216548748945" };
            Expression<Func<User, bool>> expression = (u) => u.Id == user.Id;
            // Act
            _userRepositoryMock.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
            var result = await _userService.GetUser(user.Id);
            // Assert
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetUser_Should_Throw_exception_When_User_Does_Not_Exist()
        {
            // Arrange
            var user = new User { Id = 1, FirstName = "Alice", LastName = "John", Email = "a@gmailcom", Phone = "+216548748945" };
            // Act
            _userRepositoryMock.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(() => null);
            // Assert
            await Assert.ThrowsAsync<RessourceNotFoundException>(async () => await _userService.GetUser(user.Id));
        }

        [Fact]
        public async Task CreateUser_Should_Return_New_User()
        {
            // Arrange
            string email = "a@gmailcom";
            var user = new Faker<User>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber()).Generate();

            var createUserRequest = new Faker<CreateUserRequest>()
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber()).Generate();

            // Act

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<CreateUserRequest>()))
                .Returns((CreateUserRequest request) => new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                });

            //User mappedUser = _mapperMock.Object.Map<User>(createUserRequest);
            _userRepositoryMock.Setup(x => x.FindByEmail(It.Is<string>(email => email == createUserRequest.Email))).ReturnsAsync(() => null);
            _userRepositoryMock.Setup(x => x.FindByPhone(It.Is<string>(phone => phone == createUserRequest.Phone))).ReturnsAsync(() => null);
            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(user);
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);


            User newUser = await _userService.CreateUser(createUserRequest);
            // Assert
            Assert.Equal(createUserRequest.FirstName, newUser.FirstName);
        }

        [Fact]
        public async Task CreateUser_Should_Throw_Exception_When_Email_Already_Exists()
        {
            // Arrange
            var request = new Faker<CreateUserRequest>()
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
               .Generate();
            // Act
            _userRepositoryMock.Setup(x => x.FindByEmail(It.Is<string>(email => email == request.Email))).ReturnsAsync(new User());
            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _userService.CreateUser(request));
        }

        [Fact]
        public async Task CreateUser_Should_Throw_Exception_When_Phone_Already_Exists()
        {
            // Arrange
            var request = new Faker<CreateUserRequest>()
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
               .Generate();
            // Act
            _userRepositoryMock.Setup(x => x.FindByEmail(It.Is<string>(email => email == request.Email))).ReturnsAsync(() => null);
            _userRepositoryMock.Setup(x => x.FindByPhone(It.Is<string>(phone => phone == request.Phone))).ReturnsAsync(new User());

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _userService.CreateUser(request));
        }

        //[Fact]
        public async Task UpdateUser_Should_Update_User()
        {
            // Arrange
            var request = new Faker<UpdateUserRequest>()
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
               .Generate();
            var user = new Faker<User>()
               .RuleFor(u => u.Id, (f, u) => f.Random.Number(20, 30))
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
               .Generate();
            // Act
            _userRepositoryMock.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.FindByEmail(It.Is<string>(email => email == request.Email))).ReturnsAsync(() => null);
            _userRepositoryMock.Setup(x => x.FindByPhone(It.Is<string>(phone => phone == request.Phone))).ReturnsAsync(() => null);
            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UpdateUserRequest>())).Returns((UpdateUserRequest request) => new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
            });

            _userRepositoryMock.Setup(x => x.Update(It.Is<User>(u => u.FirstName == user.FirstName))).ReturnsAsync(user);
            _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _userService.UpdateUser(user.Id, request);
            // Assert
            Assert.Equal(result.Id, user.Id);
        }

        [Fact]
        public async Task UpdateUser_Should_Throw_Exception_When_Email_Exists_For_Different_User()
        {
            // Arrange
            var request = new Faker<UpdateUserRequest>()
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
               .Generate();
            var user = new Faker<User>()
               .RuleFor(u => u.Id, (f, u) => f.Random.Number(20, 30))
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
               .Generate();

            _globalHelperMock.Setup(x => x.AreIntegersEqual(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            // Act
            _userRepositoryMock.Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.FindByEmail(It.Is<string>(email => email == request.Email))).ReturnsAsync(user);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _userService.UpdateUser(user.Id, request));
        }

        [Fact]
        public async Task DeleteUser_Should_Delete_User_When_User_Exists()
        {
            // Arrange
            var fixture = new Fixture();
            var user = fixture.Build<User>()
                 .Without(u => u.Games)
                 .Without(u => u.UserGames)
                 .Create();

            // Act
             _userRepositoryMock.Setup(u => u.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);
             _userRepositoryMock.Setup(u => u.Delete(It.IsAny<User>())).Returns(Task.CompletedTask);
             _unitOfWork.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _userService.DeleteUser(user.Id);

            // Assert
            _userRepositoryMock.Verify(ur => ur.Delete(user), Times.Once);

        }

    }
}
