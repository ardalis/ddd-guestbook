using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System.Collections.Generic;

namespace CleanArchitecture.Core.Entities
{
    public class Guestbook : BaseEntity
    {
        public string Name { get; set; }
        public List<GuestbookEntry> Entries { get; } = new List<GuestbookEntry>();

        public void AddEntry(GuestbookEntry entry)
        {
            Entries.Add(entry);
            Events.Add(new EntryAddedEvent(this.Id, entry));
        }
    }
}