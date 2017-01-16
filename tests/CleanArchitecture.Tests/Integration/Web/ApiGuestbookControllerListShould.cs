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
using System.Threading.Tasks;
using CleanArchitecture.Web.ApiModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using Newtonsoft.Json;

namespace CleanArchitecture.Tests.Integration.Web
{
    [Collection("One")]
    public class ApiGuestbookControllerListShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public ApiGuestbookControllerListShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ReturnGuestbookWithOneItem()
        {
            var response = _fixture.Client.GetAsync("/api/guestbook/1").Result;
            response.EnsureSuccessStatusCode();
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GuestbookDTO>(stringResponse);

            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.Entries.Count());
        }

        [Fact]
        public void Return404GivenInvalidId()
        {
            var response = _fixture.Client.GetAsync("/api/guestbook/100").Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var stringResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal("100", stringResponse);
        }
    }
}