using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using JordvarmeMonitorV2.Contracts;
using JordvarmeMonitorV2.Constants;
namespace JordvarmeMonitorV2.Transmit;

[ExcludeFromCodeCoverage]
public class EmailSender : IEmailSender
{
    private readonly object _lockObject = new();

    public void Send(string subject, string body)
    {
        lock (_lockObject)
        {
            using var mail = new MailMessage();
            mail.From = new MailAddress(EmailSettings.EmailFromAddress);
            mail.To.Add(EmailSettings.EmailToAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            using var smtp = new SmtpClient(EmailSettings.SmtpAddress, EmailSettings.PortNumber);
            smtp.Credentials = new NetworkCredential(EmailSettings.EmailFromAddress, EmailSettings.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = EmailSettings.EnableSsl;
            smtp.Send(mail);
        }
    }
}