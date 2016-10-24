using CleanArchitecture.Core.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace CleanArchitecture.Infrastructure
{
    public class EmailMessageSenderService : IMessageSender
    {
        public void SendGuestbookNotificationEmail(string toAddress, string messageBody)
        {
            string fromAddress = "donotreply@guestbook.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Guestbook", fromAddress));
            message.To.Add(new MailboxAddress(toAddress, toAddress));
            message.Subject = "New Message on GuestBook";
            message.Body = new TextPart("plain") { Text = messageBody };
            using (var client = new SmtpClient())
            {
                client.Connect("localhost", 25);
                client.Send(message);
                client.Disconnect(true);
            }

            // If SMTP4Dev Crashes, go to Options - Server and uncheck STARTTLS
        }
    }
}