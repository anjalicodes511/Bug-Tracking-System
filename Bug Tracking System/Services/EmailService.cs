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
        public void SendEmail(string to, string subject, string body)
        {
            string senderEmail = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string senderPassword = Environment.GetEnvironmentVariable("APP_PASSWORD");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            NetworkCredential cred = new NetworkCredential(senderEmail,senderPassword);
            smtp.Credentials = cred;
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}