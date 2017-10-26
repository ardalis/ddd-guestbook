using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Specifications;
using System.Linq;

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
            var guestbook = _guestbookRepository.GetById(entryAddedEvent.GuestbookId);
            var notificationPolicy = new GuestbookNotificationPolicy(entryAddedEvent.Entry.Id);

            // send updates to previous entries made in the last day
            var emailsToNotify = _guestbookRepository.ListEntries(notificationPolicy)
                .Select(e => e.EmailAddress);

            foreach(var emailAddress in emailsToNotify)
            {
                string messageBody = $"{entryAddedEvent.Entry.EmailAddress} left a new message {entryAddedEvent.Entry.Message}.";
                _messageSender.SendGuestbookNotificationEmail(emailAddress, messageBody);
            }
        }
    }
}
