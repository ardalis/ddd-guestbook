using System;
using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class ApiGuestbookControllerNewEntryShould : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;
        public ApiGuestbookControllerNewEntryShould(TestServerFixture fixture)
        {
            _client = fixture.Client;
        }


        [Fact]
        public async Task Return404GivenInvalidId()
        {
            var entryToPost = new { EmailAddress = "test@test.com", Message = "test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(entryToPost), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/guestbook/100/NewEntry", jsonContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal("100", stringResponse);
        }

        [Fact]
        public async Task ReturnGuestbookWithOneItem()
        {
            string message = Guid.NewGuid().ToString();
            var entryToPost = new { EmailAddress = "test@test.com", Message = message };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(entryToPost), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/guestbook/1/NewEntry", jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Guestbook>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.True(result.Entries.Any(e => e.Message == message));
        }

    }
}