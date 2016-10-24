# Lab 4 Testing

## Goal
Demonstrate the testability of the solution we've built, and how to test full stack ASP.NET Core MVC apps.

## Topics Used
Testing, xUnit, Filters

## Requirements

The Guestbook needs to support mobile and/or rich client apps, and thus requires an API. The API needs to support two methods to start:

- ListEntries: Should list the same entries as the current home page
- AddEntry: Should add a new entry (just like the form on the current home page)

## Detailed Steps

- Add a new API Controller for GuestbookController in Web/Api/GuestbookController.cs
- Add a method to get a Guestbook
    - Return a 404 Not Found if the Guestbook doesn't exist
- Add a new integration test class for the ListEntries method
    - Use ApiToDoItemsControllerListShould as a model to work from
    - Add test data to Web/Startup.cs PopulateTestData() method
        - Use Entries.Add() instead of AddEntry() when populating test data
        - Use a disposable TestServerFixture (see below)
    - Confirm the 404 behavior
    - Confirm entries are returned correctly
- Add an API method to record an entry to a Guestbook
    - Accept a Guestbook ID and a GuestbookEntry
    - Return a 404 Not Found if no Guestbook exists for the ID
    - Return the updated Guestbook if successful
- Add a new integration test class for the AddEntry method
    - Confirm the 404 behavior
    - Confirm the entry is created and sent to the repository successfully
- Extract the 404 behavior into a new VerifyGuestbookExistsAttribute

Add a filter to handle 404 policy
- Create a new Web/Filters/VerifyGuestbookExistsAttribute.cs file
- See https://github.com/ardalis/GettingStartedWithFilters for reference
- Inherit from TypeFilterAttribute
- Create a constructor that chains to base() passing in the typeof(VerifyGuestbookExistsFilter)
- Create a private class VerifyGuestbookExistsFilter
- Inherit from IAsyncActionFilter
- Create a constructor that takes an IGuestbookRepository
- Implement OnActionExecutionAsync:

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.ContainsKey("id"))
        {
            var id = context.ActionArguments["id"] as int?;
            if (id.HasValue)
            {
                if ((await _guestbookRepository.GetById(id.Value)) == null)
                {
                    context.Result = new NotFoundObjectResult(id.Value);
                    return;
                }
            }
        }
        await next();
    }

- Add the attribute to the API action methods that should return 404 when no guestbook is Found
    - [VerifyGuestbookExists]
- Remove the logic from the API methods to do guestbook existence checks and 404 response
- Confirm (via integration tests) that the behavior remains the same

## Examples:

**TestServerFixture**

    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using CleanArchitecture.Web;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;

    namespace CleanArchitecture.Tests.Integration.Web
    {
        // http://www.stefanhendriks.com/2016/04/29/integration-testing-your-dot-net-core-app-with-an-in-memory-database/
        public class TestServerFixture : IDisposable
        {
            public TestServer Server { get; }
            public HttpClient Client { get; }

            public TestServerFixture()
            {
                var builder = new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseEnvironment("Testing"); // ensure ConfigureTesting is called in Startup

                Server = new TestServer(builder);
                Client = Server.CreateClient();

                // client always expects json results
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            public void Dispose()
            {
                Server.Dispose();
                Client.Dispose();
            }
        }
    }

**ApiGuestbookControllerListShould**
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using System.Linq;
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
        }
    }