using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Events
{
    public class EntryAddedEvent : BaseDomainEvent
    {
        public int GuestbookId { get; }
        public GuestbookEntry Entry { get; }

        public EntryAddedEvent(int guestbookId, GuestbookEntry entry)
        {
            GuestbookId = guestbookId;
            Entry = entry;
        }
    }
}
