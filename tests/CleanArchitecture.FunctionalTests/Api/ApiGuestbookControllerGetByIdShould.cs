using CleanArchitecture.Core.Entities;
using CleanArchitecture.Web;
using CleanArchitecture.Web.ApiModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.FunctionalTests.Api
{
    public class ApiGuestbookControllerGetByIdShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ApiGuestbookControllerGetByIdShould(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnGuestbookWithOneItem()
        {
            var response = await _client.GetAsync("/api/guestbook/1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GuestbookDTO>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.Single(result.Entries);
        }

        [Fact]
        public async Task Return404GivenInvalidId()
        {
            string invalidId = "100";
            var response = await _client.GetAsync($"/api/guestbook/{invalidId}");
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(invalidId, stringResponse);
        }
    }
}
