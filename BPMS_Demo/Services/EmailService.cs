using System.Net.Mail;
using Contracts;

namespace Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body, string from)
        {
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                UseDefaultCredentials = false
            };

            var mail = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body
            };

            client.Send(mail);
        }
    }
}