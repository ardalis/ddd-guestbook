using CleanArchitecture.Web;
using CleanArchitecture.Web.ApiModels;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.FunctionalTests.Api
{
    public class ApiGuestbookControllerNewEntryShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ApiGuestbookControllerNewEntryShould(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Return404GivenInvalidId()
        {
            string invalidId = "100";
            var entryToPost = new { EmailAddress = "test@test.com", Message = "test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(entryToPost), Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync($"/api/guestbook/{invalidId}/NewEntry", jsonContent);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(invalidId, stringResponse);
        }

        [Fact]
        public async Task ReturnGuestbookWithOneItem()
        {
            int validId = 1;
            string message = Guid.NewGuid().ToString();
            var entryToPost = new { EmailAddress = "test@test.com", Message = message };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(entryToPost), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"/api/guestbook/{validId}/NewEntry", jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GuestbookDTO>(stringResponse);

            Assert.Equal(validId, result.Id);
            Assert.Contains(result.Entries, e => e.Message == message);
        }
    }
}
