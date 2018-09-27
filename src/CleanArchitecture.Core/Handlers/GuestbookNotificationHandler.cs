using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Specifications;
using System.Linq;

namespace CleanArchitecture.Core.Handlers
{
    public class GuestbookNotificationHandler : IHandle<EntryAddedEvent>
    {
        private readonly IRepository _repository;
        private readonly IMessageSender _messageSender;

        public GuestbookNotificationHandler(IRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
            _messageSender = messageSender;
        }

        public void Handle(EntryAddedEvent entryAddedEvent)
        {
            var notificationPolicy = new GuestbookNotificationPolicy(entryAddedEvent.Entry.Id);

            // send updates to previous entries made in the last day
            var emailsToNotify = _repository.List(notificationPolicy).Select(e => e.EmailAddress);

            foreach (var emailAddress in emailsToNotify)
            {
                string messageBody = $"{entryAddedEvent.Entry.EmailAddress} left a new message {entryAddedEvent.Entry.Message}.";
                _messageSender.SendGuestbookNotificationEmail(emailAddress, messageBody);
            }
        }
    }
}
