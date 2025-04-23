using System;
using System.Net;
using System.Net.Mail;

namespace Bitirme.BLL.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        private string GetBaseUrl()
        {
            // Assuming this method returns the base URL of the project.
            return "https://example.com";
        }

        public void SendEmail(string to, string subject, string body, string userId)
        {
            var verificationLink = $"{GetBaseUrl()}/verify-email?userId={userId}";

            var smtpClient = new SmtpClient(_smtpServer)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = $"{body}\n\nVerification Link: {verificationLink}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            smtpClient.Send(mailMessage);
        }
    }
}