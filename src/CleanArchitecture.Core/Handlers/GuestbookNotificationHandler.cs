using System;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Specifications;

namespace CleanArchitecture.Core.Handlers
{
    public class GuestbookNotificationHandler : IHandle<EntryAddedEvent>
    {
        private readonly IGuestbookRepository _guestbookRepository;
        private readonly IMessageSender _messageSender;

        public GuestbookNotificationHandler(IGuestbookRepository guestbookRepository,
            IMessageSender messageSender)
        {
            _guestbookRepository = guestbookRepository;
            _messageSender = messageSender;
        }

        public void Handle(EntryAddedEvent entryAddedEvent)
        {
            var emailsToNotify = _guestbookRepository.ListEntries(
                new GuestbookNotificationPolicy(entryAddedEvent.GuestbookId))
                .Select(e => e.EmailAddress);
            foreach (var emailAddress in emailsToNotify)
            {
                string messageBody = "{entry.EmailAddress} left new message {entry.Message}";
                _messageSender.SendGuestbookNotificationEmail(emailAddress, messageBody);
            }
        }
    }
}