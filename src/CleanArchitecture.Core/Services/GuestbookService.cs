using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Core.Services
{
    public class GuestbookService : IGuestbookService
    {
        private readonly IRepository _repository;
        private readonly IMessageSender _messageSender;

        public GuestbookService(IRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
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
            _repository.Update(guestbook);
        }
    }
}
