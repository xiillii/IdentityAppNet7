using Microsoft.AspNetCore.Identity.UI.Services;

using System.Web;
using MimeKit;
using System.Runtime.Intrinsics.X86;
using MailKit.Net.Smtp;

namespace IdentityApp.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly EmailSenderConfiguration _emailSenderConfiguration;

        public SmtpEmailSender(EmailSenderConfiguration cfg)
        {
            _emailSenderConfiguration = cfg;
        }

        public async Task SendEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            var message = new MessageMailKit(new string[] { emailAddress }, subject, htmlMessage);

            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(MessageMailKit message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Identity App Verifier", _emailSenderConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            

            var builder = new BodyBuilder
            {
                HtmlBody = message.Content
            };

            emailMessage.Body = builder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailSenderConfiguration.SmtpServer, _emailSenderConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailSenderConfiguration.Username,
                    _emailSenderConfiguration.Password);

                await client.SendAsync(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
