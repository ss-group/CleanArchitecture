using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailAsync(string email,string emailCC, string subject, string message);
        Task SendEmailAsync(IEnumerable<string> emails, string subject, string message);

        Task SendEmailWithTemplateAsysnc(string templateCode, string email, IReadOnlyDictionary<string, string> headerParam, IReadOnlyDictionary<string, string> bodyParam);
    }
}
