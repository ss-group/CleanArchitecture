using Domain.Entities.SU;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Email
{
    /// <summary>
    /// Defines configurations to used by SmtpClient object.
    /// </summary>
    public interface IEmailSetting
    {
        /// <summary>
        /// SMTP Host name/IP.
        /// </summary>
        string Host { get; }

        /// <summary>
        /// SMTP Port.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// User name to login to SMTP server.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Password to login to SMTP server.
        /// </summary>
        /// 
        string Password { get; }
        /// <summary>
        /// Display sender name
        /// </summary>
        string SenderName { get; }

        Task<SuEmailTemplate> GetTemplate(string templateCode);
    }
}
