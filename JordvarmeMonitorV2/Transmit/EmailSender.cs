using System.Net;
using System.Net.Mail;
using JordvarmeMonitorV2.Contracts;

namespace JordvarmeMonitorV2.Transmit;

public class EmailSender : IEmailSender
{
    // ReSharper disable StringLiteralTypo
    private const string SmtpAddress = "smtp-mail.outlook.com";
    private const int PortNumber = 587;
    private const bool EnableSsl = true;
    private const string EmailFromAddress = "villy.ib.jorgensen@outlook.com"; //Sender Email Address  
    private const string Password = "Antananarivo447"; //Sender Password  
    private const string EmailToAddress = "villy.ib.jorgensen@gmail.com"; //Receiver Email Address  
    // ReSharper restore StringLiteralTypo

    private readonly object _lockObject = new();

    public void Send(string subject, string body)
    {
        lock (_lockObject)
        {
            using var mail = new MailMessage();
            mail.From = new MailAddress(EmailFromAddress);
            mail.To.Add(EmailToAddress);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            using var smtp = new SmtpClient(SmtpAddress, PortNumber);
            smtp.Credentials = new NetworkCredential(EmailFromAddress, Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = EnableSsl;
            smtp.Send(mail);
        }
    }
}