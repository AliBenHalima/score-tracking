using AutoMapper.Configuration.Annotations;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using ScoreTracking.App.Interfaces.Services;
using ScoreTracking.App.Models;
using ScoreTracking.App.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ScoreTracking.UnitTests.Integration
{
    public class UserTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _webApplicationFactory;
        private readonly Mock<IUserService> _userService = new Mock<IUserService>(MockBehavior.Strict);
        public UserTests(WebApplicationFactory<Program> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task GetUsers_Should_Return_Unauthorized_When_User_Uauthorized()
        {
            var httpClient = _webApplicationFactory.CreateClient();
            var response = await httpClient.GetAsync("/api/users");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUsers_Should_Return_Users_When_User_authorized()
        {
            throw new NotImplementedException();
            // Missing jwt
        }

        [Theory]
        [InlineData(1)]
        public async Task GetUser_Should_Return_User_When_User_authorized(int id)
        {
            // Arrange
            var httpClient = _webApplicationFactory.CreateClient();
            var user = _userService.Setup(x => x.GetUser(It.IsAny<int>())).ReturnsAsync(It.IsAny<User>());
            // Act
            var response = await httpClient.GetAsync($"/api/users/{id}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //TODO : Fix it
        }
    }
}
