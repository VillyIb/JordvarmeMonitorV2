using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JordvarmeMonitorV2.Constants
{
    public static class EmailSettings
    {
        // ReSharper disable StringLiteralTypo

        internal const string SmtpAddress = "smtp-mail.outlook.com";
        internal const int PortNumber = 587;
        internal const bool EnableSsl = true;
        internal const string EmailFromAddress = "villy.ib.jorgensen@outlook.com"; //Sender Email Address  
        internal const string Password = "Antananarivo447"; //Sender Password  
        internal const string EmailToAddress = "villy.ib.jorgensen@gmail.com"; //Receiver Email Address  
        // ReSharper restore StringLiteralTypo
    }
}
