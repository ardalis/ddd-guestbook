using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Core.Services
{
    public class GuestbookService : IGuestbookService
    {
        private readonly IRepository<Guestbook> _guestbookRepository;
        private readonly IMessageSender _messageSender;

        public GuestbookService(IRepository<Guestbook> guestbookRepository,
            IMessageSender messageSender)
        {
            _guestbookRepository = guestbookRepository;
            _messageSender = messageSender;
        }
        public void RecordEntry(Guestbook guestbook, GuestbookEntry newEntry)
        {
            // notify all previous entries
            foreach (var entry in guestbook.Entries)
            {
                _messageSender.SendGuestbookNotificationEmail(entry.EmailAddress, newEntry.Message);
            }

            guestbook.Entries.Add(newEntry);
            _guestbookRepository.Update(guestbook);
        }
    }
}
