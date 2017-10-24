namespace CleanArchitecture.Core.Interfaces
{
    public interface IMessageSender
    {
        void SendGuestbookNotificationEmail(string toAddress, string messageBody);
    }
}