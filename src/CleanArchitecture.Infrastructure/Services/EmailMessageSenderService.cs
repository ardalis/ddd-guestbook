using CleanArchitecture.Core.Interfaces;
using System.Net.Mail;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmailMessageSenderService : IMessageSender
    {
        public void SendGuestbookNotificationEmail(string toAddress, string messageBody)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(toAddress));
            message.From = new MailAddress("donotreply@guestbook.com");
            message.Subject = "New guestbook entry added";
            message.Body = messageBody;
            using (var client = new SmtpClient("localhost", 25))
            {
                client.Send(message);
            }
        }
    }
}
