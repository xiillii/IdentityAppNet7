using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Web;

namespace IdentityApp.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            System.Console.WriteLine("---New Email----");
            System.Console.WriteLine($"To: {emailAddress}");
            System.Console.WriteLine($"Subject: {subject}");
            System.Console.WriteLine(HttpUtility.HtmlDecode(htmlMessage));
            System.Console.WriteLine("-------");
            return Task.CompletedTask;
        }
    }
}
