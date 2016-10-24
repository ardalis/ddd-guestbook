using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class ApiGuestbookControllerListShould : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;
        public ApiGuestbookControllerListShould(TestServerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task ReturnGuestbookWithOneItem()
        {
            var response = await _client.GetAsync("/api/guestbook/1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Guestbook>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.Entries.Count());
        }

        [Fact]
        public async Task Return404GivenInvalidId()
        {
            var response = await _client.GetAsync("/api/guestbook/100");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal("100", stringResponse);
        }
    }
}