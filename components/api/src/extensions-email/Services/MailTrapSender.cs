using MailKit.Net.Smtp;
using MimeKit;
using ScoreTracking.Extensions.Email.Contracts;
using ScoreTracking.Extensions.Email.Contratcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.Extensions.Email.Services
{
    public class MailTrapSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public MailTrapSender(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task Send(EmailMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                using MimeMessage emailMessage = new MimeMessage();

                var emailFrom = new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);

                var emailTo = new MailboxAddress(message.ReceiverName, message.ReceiverAddress);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = message.Subject;


                var emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = message.Content;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using var mailClient = new SmtpClient();
                await mailClient.ConnectAsync(_emailSettings.Server, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await mailClient.SendAsync(emailMessage);
                await mailClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email", ex);
            }
        }

    }
}
