using System.Net;
using System.Net.Mail;

namespace JordvarmeMonitorV2.Tx
{
    public class EmailSender
    {
        private static string smtpAddress = "smtp-mail.outlook.com";
        private static int portNumber = 587;
        private static bool enableSSL = true;
        private static string emailFromAddress = "villy.ib.jorgensen@outlook.com"; //Sender Email Address  
        private static string password = "Antananarivo447"; //Sender Password  
        private static string emailToAddress = "villy.ib.jorgensen@gmail.com"; //Receiver Email Address  

        public void Send(string subjectAndBody)
        {
            using MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailFromAddress);
            mail.To.Add(emailToAddress);
            mail.Subject = subjectAndBody;
            mail.Body = subjectAndBody;
            mail.IsBodyHtml = true;
            using SmtpClient smtp = new SmtpClient(smtpAddress, portNumber);
            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = enableSSL;
            smtp.Send(mail);
        }
    }
}
