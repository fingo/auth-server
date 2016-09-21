using Fingo.Auth.ManagementApp.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace Fingo.Auth.ManagementApp.Services.Implementation
{
    public class MessageSender : IMessageSender
    {
        public void SendEmail(MimeMessage message , string senderEmail , string serverPassword , string serverName ,
            int portNumber)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(serverName , portNumber , true);
                client.Authenticate(senderEmail , serverPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public MimeMessage CreateMessage(string clientName , string clientEmail , string senderName , string senderEmail ,
            string messageSubject , TextPart emailContent)
        {
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(clientName , clientEmail));
            message.From.Add(new MailboxAddress(senderName , senderEmail));
            message.Subject = messageSubject;
            message.Body = emailContent;
            return message;
        }

        public TextPart CreateContent(string content , string greeting , string sender)
        {
            var textPart = new TextPart("plain")
            {
                Text = greeting + ",\n" + content + "\n\n" + sender
            };
            return textPart;
        }
    }
}