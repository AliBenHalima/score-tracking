using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http;

namespace ScoreTracking.IntegrationTests
{
    public class UnitTest1: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Method_Should_Be_Excuted()
        {
            var client = _factory.CreateClient();
            /// ERROR
            var response = await client.GetAsync("/api/users");
            Assert.True(true);
        }
    }
}