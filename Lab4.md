** Lab 4 Testing

* Goal
Demonstrate the testability of the solution we've built, and how to test full stack ASP.NET Core MVC apps.

* Topics Used
Testing, xUnit, Filters

* Requirements

The Guestbook needs to support mobile and/or rich client apps, and thus requires an API. The API needs to support two methods to start:

- ListEntries: Should list the same entries as the current home page
- AddEntry: Should add a new entry (just like the form on the current home page)

* Detailed Steps

- Add a new API Controller for GuestbookController in Web/Api/GuestbookController.cs
- Add a method to get a Guestbook with its last 10 entries
    - Return a 404 Not Found if the Guestbook doesn't exist
- Add a new integration test class for the ListEntries method
    - Use ApiToDoItemsControllerListShould as a model to work from
    - Confirm the 404 behavior
    - Confirm entries are returned correctly
- Add a method to record an entry to a Guestbook
    - Return a 404 Not Found if the Guestbook doesn't exist
- Add a new integration test calss for the ListEntries method
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
