using System;
using System.Collections;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace FindbookApi.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string admEmail = Environment.GetEnvironmentVariable("email");
            string password = Environment.GetEnvironmentVariable("password");
            string server = Environment.GetEnvironmentVariable("server");
            if (admEmail == null || password == null || server == null)
                throw new OperationCanceledException("Email service params not found");

            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress("FindBook", admEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
             
            using var client = new SmtpClient();
            await client.ConnectAsync(server, 25, false);
            await client.AuthenticateAsync(admEmail, password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}