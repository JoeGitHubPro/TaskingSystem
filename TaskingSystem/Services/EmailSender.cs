﻿using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace TaskingSystem.Services
{
    public class EmailSender : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = "SenderEmail";
            var fromPassword = "SenderEmailPassword";

            var message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(email);
            var templatePath = Path.GetFullPath("wwwroot/StaticHTML/MailTemplate.html");
            message.Body = File.ReadAllText(templatePath).ToString().Replace("ID-Confirm-1111-3333", htmlMessage.ToString());
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
