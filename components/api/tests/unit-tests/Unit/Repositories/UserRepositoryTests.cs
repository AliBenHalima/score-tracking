using AutoFixture;
using Bogus;
using FluentAssertions;
using ScoreTracking.App.Database;
using ScoreTracking.App.Models;
using ScoreTracking.App.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Unit.Repositories
{
    public class UserRepositoryTests
    {
        private readonly InMemoryDatabaseContext _context;
        public UserRepositoryTests()
        {
            _context = new InMemoryDatabaseContext();
        }

        [Theory]
        [MemberData(nameof(GetUserInstance))]
        public async Task Create_Should_Add_User_To_Database(User user)
        {
            // Arrange
            var context = _context.CreateContextForInMemory();
            var sut = new UserRepository(context);
            // Act
            var createdUser = await sut.Create(user);
            var addedUser = context.Users.FirstOrDefault(u => u.Id == user.Id);
            // Assert
            Assert.NotNull(addedUser);
        }

        [Theory]
        [MemberData(nameof(GetUserInstance))]
        public async Task FindByEmail_Should_Return_User(User user)
        {
            // Arrange
            var context = _context.CreateContextForInMemory();
            var sut = new UserRepository(context);

            // Act
            User createdUser = await sut.Create(user);
            User userByEmail = await sut.FindByEmail(user.Email);
            // Assert
            userByEmail.Should().NotBeNull();
            userByEmail.Should().BeEquivalentTo(createdUser);
        }

        [Theory]
        [MemberData(nameof(GetUserInstance))]
        public async Task FindByPhone_Should_Return_User(User user)
        {
            // Arrange
            var context = _context.CreateContextForInMemory();
            var sut = new UserRepository(context);
            // Act
            User createdUser = await sut.Create(user);
            User userByPhone = await sut.FindByPhone(user.Phone);
            // Assert
            userByPhone.Should().NotBeNull();
            userByPhone.Should().BeEquivalentTo(createdUser);
        }

        [Theory]
        [MemberData(nameof(GetUserInstance))]
        public async Task FindById_Should_Return_User(User user)
        {
            // Arrange
            var context = _context.CreateContextForInMemory();
            var sut = new UserRepository(context);
            // Act
            User createdUser = await sut.Create(user);
            User userById = await sut.FindById(user.Id);
            // Assert
            userById.Should().NotBeNull();
            userById.Should().BeEquivalentTo(createdUser);
        }

        [Theory]
        [MemberData(nameof(GetUserInstance))]
        public async Task UpdateUser_Should_Update_User_Successfully(User user)
        {
            // Arrange
            var context = _context.CreateContextForInMemory();
            var sut = new UserRepository(context);
            var faker = new Faker("en");
            // Act
            User createdUser = await sut.Create(user);

            createdUser.FirstName = faker.Name.FirstName();
            createdUser.LastName = faker.Name.LastName();
            createdUser.Email = faker.Internet.Email();
            createdUser.Phone = faker.Phone.PhoneNumber();

            User updatedUser = await sut.Update(createdUser);

            // Assert
            updatedUser.Should().NotBeNull();
            updatedUser.Should().BeEquivalentTo(createdUser);
        }

        public static List<object[]> GetUserInstance()
        {
            return new List<object[]>
           {
               new object[]
               {
                    new Faker<User>()
                    .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                    .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                    .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
                    .Generate()
                }
           };

        }
    }
}
