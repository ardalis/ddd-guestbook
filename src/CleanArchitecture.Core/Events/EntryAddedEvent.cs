using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Model;

namespace CleanArchitecture.Core.Events
{
    public class EntryAddedEvent : BaseDomainEvent
    {
        public int GuestbookId { get; set; }
        public GuestbookEntry Entry { get; set; }

        public EntryAddedEvent(int guestbookId, GuestbookEntry entry)
        {
            GuestbookId = guestbookId;
            Entry = entry;
        }
    }
}