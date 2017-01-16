using System;
using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using CleanArchitecture.Web.ApiModels;
using Newtonsoft.Json;

namespace CleanArchitecture.Tests.Integration.Web
{
    [Collection("One")]
    public class ApiGuestbookControllerNewEntryShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        //private readonly HttpClient _client;
        public ApiGuestbookControllerNewEntryShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Return404GivenInvalidId()
        {
            var entryToPost = new { EmailAddress = "test@test.com", Message = "test" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(entryToPost), Encoding.UTF8,
                "application/json");
            var response = _fixture.Client.PostAsync("/api/guestbook/100/NewEntry", jsonContent).Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal("100", stringResponse);
        }

        [Fact]
        public void ReturnGuestbookWithOneItem()
        {
            string message = Guid.NewGuid().ToString();
            var entryToPost = new { EmailAddress = "test@test.com", Message = message };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(entryToPost), Encoding.UTF8,
                "application/json");
            var response = _fixture.Client.PostAsync("/api/guestbook/1/NewEntry", jsonContent).Result;
            response.EnsureSuccessStatusCode();
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GuestbookDTO>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.True(result.Entries.Any(e => e.Message == message));
        }
    }
}