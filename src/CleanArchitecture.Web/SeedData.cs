using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Web
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext dbContext)
        {
            var toDos = dbContext.ToDoItems;
            foreach (var item in toDos)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.ToDoItems.Add(new ToDoItem()
            {
                Title = "Test Item 1",
                Description = "Test Description One"
            });
            dbContext.ToDoItems.Add(new ToDoItem()
            {
                Title = "Test Item 2",
                Description = "Test Description Two"
            });
            dbContext.SaveChanges();

            // add Guestbook test data; specify Guestbook ID for use in tests
            var guestbook = new Guestbook { Name = "Test Guestbook", Id = 1 };
            dbContext.Guestbooks.Add(guestbook);
            guestbook.AddEntry(new GuestbookEntry
            {
                EmailAddress = "test@test.com",
                Message = "Test message"
            });
            guestbook.Events.Clear();
            dbContext.SaveChanges();
        }
    }
}
