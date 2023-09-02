using ScoreTracking.Integ.Tests;
using ScoreTracking.Api;
using System.Net;
using System.Net.Http.Headers;
using ScoreTracking.App.Database;
using ScoreTracking.App.Interfaces.Providers;
using ScoreTracking.App.Models;
using FluentAssertions;
using ScoreTracking.App.DTOs.Requests.Users;
using Bogus;
using System.Text;

namespace ScoreTracking.IntegrationTests
{
    public class UserIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UserIntegrationTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetUser_Should_Return_Response_Ok_When_Token_Is_Valid()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SeedDatabase();
            // Act
            string tokenString = GenerateToken(_factory.Services);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
            var response = await client.GetAsync("/api/users");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUser_Should_Return_Response_Ok_When_Token_Is_Invalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SeedDatabase();
            // Act
            var response = await client.GetAsync("/api/users");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetUser_Should_Return_Ok_Response(int id)
        {
            // Arrange
            var client = _factory.CreateClient();
            _factory.SeedDatabase();
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var user = dbContext.Users.Where(u => u.Id == id).FirstOrDefault();

            // Act
            var response = await client.GetAsync($"/api/users/{id}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateUser_Should_Save_User_To_Database()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var client = _factory.CreateClient();
            _factory.SeedDatabase();
            var createUserRequest = new Faker<CreateUserRequest>()
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Phone, (f, u) => "+21612345678")
            .Generate();

            var jsonContent = new StringContent(
            Newtonsoft.Json.JsonConvert.SerializeObject(createUserRequest),
            Encoding.UTF8,
            "application/json"
            );

            // Act
            var response = await client.PostAsync("/api/users/", jsonContent);

            var createdUser = dbContext.Users.Where(u => u.Email == createUserRequest.Email).FirstOrDefault();
            // Assert
            createdUser.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteUser_Should_Remove_User_From_Database(int id)
        {

            // Arrange
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var client = _factory.CreateClient();
            _factory.SeedDatabase();
            // Act
            var response = await client.DeleteAsync($"/api/users/{id}");

            var user = dbContext.Users.Where(u => u.Id == id).FirstOrDefault();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Should().BeNull();
        }

        private string GenerateToken(IServiceProvider services)
        {
            using var scope = _factory.Services.CreateScope();
            var jwtProvider = scope.ServiceProvider.GetRequiredService<IJwtProvider>();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            User? randomUser = dbContext.Users.FirstOrDefault();
            var tokenString = jwtProvider.Generate(randomUser);
            return tokenString;
        }
        public void Dispose()
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.EnsureDeleted();
        }
    }
}