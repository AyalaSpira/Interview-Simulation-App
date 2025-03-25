using InterviewSim.BLL.Interfaces;
using System.Net.Mail;
using System.Net;

namespace InterviewSim.BLL.Services
{
    public class SmtpMailService : IMailService
    {
        private readonly string _smtpServer;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly int _smtpPort;

        public SmtpMailService(string smtpServer, string smtpUsername, string smtpPassword, int smtpPort)
        {
            _smtpServer = smtpServer;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _smtpPort = smtpPort;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] fileBytes, string fileName)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            // הוספת הקובץ כקובץ מצורף
            using (var memoryStream = new MemoryStream(fileBytes))
            {
                var attachment = new Attachment(memoryStream, fileName, "application/pdf");
                mailMessage.Attachments.Add(attachment);

                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    smtpClient.EnableSsl = true;

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

    }
}
