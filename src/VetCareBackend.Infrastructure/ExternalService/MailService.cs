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
        public void SendEmail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("tomas", "tomasburack33@gmail.com"));
            message.To.Add(new MailboxAddress("tomas", "tomasburack22@gmail.com"));
            message.Subject = "How you doin?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Alice,

                        What are you up to this weekend? Monica is throwing one of her parties on
                        Saturday and I was hoping you could make it.

                        Will you be my +1?

                        -- Joey
                        "
            };

            SmtpClient smtp = new SmtpClient();
            smtp.Connect(_mailOptions.Host, _mailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailOptions.UserName, _mailOptions.Password);
            smtp.Send(message);
            smtp.Disconnect(true);

        }
    }
}
