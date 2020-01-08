using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace WebMailer.SmtpClients
{
    public class FactorySmtp : IAbstractFactorySmtp
    {
        private string User { get; set; }
        private string Password { get; set; }
        public FactorySmtp(string usr, string pass)
        {
            User = usr;
            Password = pass;
        }
        public SmtpClient CreateSmtpClient()
        {
            if (Regex.IsMatch(User, "gmail"))
            {
                return new SmtpGoogle().Client(User, Password);
            }
            else
            {
                return null;
            }

        }
    }
}