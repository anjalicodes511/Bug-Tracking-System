using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Bug_Tracking_System.Services
{
    public class EmailService
    {
        public void Send(string to, string subject, string body)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = body;

            SmtpClient client = new SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.EnableSsl = true;

            client.Credentials =
            new NetworkCredential("yourmail@gmail.com", "app-password");

            client.Send(msg);
        }
    }
}