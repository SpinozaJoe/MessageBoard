using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MessageBoard.Services
{
    public class MailService : IMailService
    {
        public bool SendMail(string from, string to, string subject, string body)
        {
            try
            {
                MailMessage msg = new MailMessage(from, to, subject, body);
                SmtpClient client = new SmtpClient("uksmtp.markit.partners");

                client.Send(msg);
            }
            catch (Exception)
            {
                // Logging required
                return false;
            }

            return true;
        }
    }
}