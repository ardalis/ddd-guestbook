using CleanArchitecture.Web;
using CleanArchitecture.Web.ApiModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Xunit;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class ApiGuestbookControllerGetByIdShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ApiGuestbookControllerGetByIdShould(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public void ReturnGuestbookWithOneItem()
        {
            var response = _client.GetAsync("/api/guestbook/1").Result;
            response.EnsureSuccessStatusCode();
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GuestbookDTO>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.Single(result.Entries);
        }

        [Fact]
        public void Return404GivenInvalidId()
        {
            string invalidId = "100";
            var response = _client.GetAsync($"/api/guestbook/{invalidId}").Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(invalidId, stringResponse);
        }
    }
}
