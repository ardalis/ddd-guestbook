using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Specifications
{
    public class GuestbookNotificationPolicy : ISpecification<GuestbookEntry>
    {
        public GuestbookNotificationPolicy(int entryAddedId = 0)
        {
            Criteria = e =>
                e.DateTimeCreated > DateTimeOffset.UtcNow.AddDays(-1) // created after 1 day ago
                && e.Id != entryAddedId; // don't notify the added entry
        }
        public Expression<Func<GuestbookEntry, bool>> Criteria { get; }
    }
}
