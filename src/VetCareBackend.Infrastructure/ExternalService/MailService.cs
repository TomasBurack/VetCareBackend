using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Configuration;
using MailKit.Net.Smtp;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Infrastructure.ExternalService
{
    public class MailService : IMailService
    {
        private readonly MailOptions _mailOptions;
        public MailService(IOptions<MailOptions> mailOptions)
        {
            _mailOptions = mailOptions.Value;
        }
        public void SendEmail(string ToEmail, string ToName, string subjet, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailOptions.UserName, _mailOptions.UserName);
            message.To.Add(new MailboxAddress(ToName, ToEmail));
            message.Subject = subjet;

            message.Body = new TextPart("plain") { Text = body };

            using SmtpClient smtp = new SmtpClient();
            smtp.Connect(_mailOptions.Host, _mailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailOptions.UserName, _mailOptions.Password);
            smtp.Send(message);
            smtp.Disconnect(true);

        }
    }
}
