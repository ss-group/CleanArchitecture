using Application.Common.Interfaces.Email;
using Domain.Entities.SU;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services.Email
{

    public class EmailSender : IEmailSender
    {
        private readonly IEmailSetting _emailSettings;

        public EmailSender(IEmailSetting setting)
        {
            _emailSettings = setting;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName,_emailSettings.UserName));

                mimeMessage.To.Add(new MailboxAddress(email));

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                  //  client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(_emailSettings.Host,_emailSettings.Port, SecureSocketOptions.StartTlsWhenAvailable);
                   

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

        public Task SendEmailAsync(string email, string emailCC, string subject, string message)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(IEnumerable<string> emails, string subject, string message)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailWithTemplateAsysnc(string templateCode, string email, IReadOnlyDictionary<string, string> headerParam, IReadOnlyDictionary<string, string> bodyParam)
        {
            SuEmailTemplate template = await _emailSettings.GetTemplate(templateCode);
            if(template == null)
            {
                throw new Exception($"No template code{templateCode}");
            }

            if(headerParam?.Count > 0)
            {
                foreach (var key in headerParam.Keys)
                {
                    template.Subject = template.Subject.Replace(key, headerParam[key]);
                }
            }
            if (bodyParam?.Count > 0)
            {
                foreach (var key in bodyParam.Keys)
                {
                    template.Content = template.Content.Replace(key, bodyParam[key]);
                }
            }

            await this.SendEmailAsync(email, template.Subject, template.Content);
        }
    }
}
