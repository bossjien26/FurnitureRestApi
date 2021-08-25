using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Helpers
{
    public class MailHelper
    {
        public SmtpMailConfig _smtpMailConfig;

        public ILogger<MailHelper> _logger;

        public MailHelper(SmtpMailConfig smtpMailConfig, ILogger<MailHelper> logger)
        {
            _smtpMailConfig = smtpMailConfig;
            _logger = logger;
        }

        public MimeMessage MailMessage(Mailer mailer)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_smtpMailConfig.FromDisplayName, _smtpMailConfig.FromAddress));
            mailMessage.To.Add(new MailboxAddress(mailer.NameTo, mailer.MailTo));
            mailMessage.Subject = mailer.Subject;
            mailMessage.Body = new TextPart(_smtpMailConfig.TextPart)
            {
                Text = mailer.Content

            };
            return mailMessage;
        }

        public void SendMail(Mailer mailer)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Connect(_smtpMailConfig.Server, _smtpMailConfig.Port, _smtpMailConfig.EnableSsl);
            smtpClient.Authenticate(_smtpMailConfig.Account, _smtpMailConfig.Password);
            smtpClient.Send(MailMessage(mailer));
            smtpClient.Disconnect(true);
        }
    }
}