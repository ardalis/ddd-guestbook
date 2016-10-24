using System;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.Core.Handlers
{
    public class GuestbookNotificationHandler : IHandle<EntryAddedEvent>
    {
        private readonly IRepository<Guestbook> _guestbookRepository;
        private readonly IMessageSender _messageSender;

        public GuestbookNotificationHandler(IRepository<Guestbook> guestbookRepository,
            IMessageSender messageSender)
        {
            _guestbookRepository = guestbookRepository;
            _messageSender = messageSender;
        }

        public void Handle(EntryAddedEvent entryAddedEvent)
        {
            var guestbook = _guestbookRepository.GetById(entryAddedEvent.GuestbookId);

            // send updates to previous entries made within last day
            var emailsToNotify = guestbook.Entries
                .Where(e => e.DateTimeCreated > DateTimeOffset.UtcNow.AddDays(-1))
                .Select(e => e.EmailAddress);
            foreach (var emailAddress in emailsToNotify)
            {
                string messageBody = "{entry.EmailAddress} left new message {entry.Message}";
                _messageSender.SendGuestbookNotificationEmail(emailAddress, messageBody);
            }
        }
    }
}