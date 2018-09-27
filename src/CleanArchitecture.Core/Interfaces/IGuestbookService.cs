using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IGuestbookService
    {
        void RecordEntry(Guestbook guestbook, GuestbookEntry newEntry);
    }
}
