using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Web;
using Newtonsoft.Json;

namespace CleanArchitecture.Tests.Integration.Web
{
    public class ApiToDoItemsControllerListShould : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;
        public ApiToDoItemsControllerListShould(TestServerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task ReturnTwoItems()
        {
            Debug.WriteLine("ReturnTwoItems-Start");
            var response = await _client.GetAsync("/api/todoitems");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<ToDoItem>>(stringResponse).ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.Count(a => a.Title == "Test Item 1"));
            Assert.Equal(1, result.Count(a => a.Title == "Test Item 2"));
            Debug.WriteLine("ReturnTwoItems-End");
        }
    }
}