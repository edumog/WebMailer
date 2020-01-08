using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebMailer.SmtpClients
{
    public class SmtpGoogle : ISmtpClient
    {
        public SmtpClient Client(string usr, string pass)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 25)
            {
                EnableSsl = true,
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(usr, pass)
            };
            return client;
        }
    }
}