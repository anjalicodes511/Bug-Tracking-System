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
            //string senderEmail = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            //string senderPassword = Environment.GetEnvironmentVariable("APP_PASSWORD");
            //string host = Environment.GetEnvironmentVariable("SMTP_HOST");
            //int port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));

            string senderEmail = "anjali.d1718@gmail.com";
            string senderPassword = "sjij odji hyxs gipm";
            string host = "smtp.gmail.com";
            int port = 587;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderEmail);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(host, port);
            smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}