using MimeKit;

namespace Fingo.Auth.ManagementApp.Services.Interfaces
{
    public interface IMessageSender
    {
        void SendEmail(MimeMessage message, string senderEmail, string serverPassword, string serverName, int portNumber);
        MimeMessage CreateMessage(string clientName, string clientEmail, string senderName, string senderEmail, string messageSubject, TextPart emailContent);
        TextPart CreateContent(string content, string greeting, string sender);
    }
}