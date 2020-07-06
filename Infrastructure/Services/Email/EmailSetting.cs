using Application.Common.Interfaces;
using Application.Common.Interfaces.Email;
using Domain.Entities.SU;
using System.Threading.Tasks;

namespace Infrastructure.Services.Email
{
    public class EmailSetting : IEmailSetting
    {
        public string Host
        {
            get
            {
                return _context.QueryFirstOrDefaultAsync<string>("SELECT parameter_value FROM su_parameter WHERE parameter_group_code = @group AND parameter_code = @code", new { group = "Email", code = "Host" }).Result;
            }
        }
        public int Port
        {
            get
            {
                return _context.QueryFirstOrDefaultAsync<int>("SELECT parameter_value FROM su_parameter WHERE parameter_group_code = @group AND parameter_code = @code", new { group = "Email", code = "Port" }).Result;
            }
        }
        public string UserName
        {
            get
            {
                return _context.QueryFirstOrDefaultAsync<string>("SELECT parameter_value FROM su_parameter WHERE parameter_group_code = @group AND parameter_code = @code", new { group = "Email", code = "UserName" }).Result;
            }
        }
        public string Password
        {
            get
            {
                return _context.QueryFirstOrDefaultAsync<string>("SELECT parameter_value FROM su_parameter WHERE parameter_group_code = @group AND parameter_code = @code", new { group = "Email", code = "Password" }).Result;
            }
        }
        public string SenderName
        {
            get
            {
                return _context.QueryFirstOrDefaultAsync<string>("SELECT parameter_value FROM su_parameter WHERE parameter_group_code = @group AND parameter_code = @code", new { group = "Email", code = "SenderName" }).Result;
            }
        }

        private readonly ICleanDbContext _context;
        public EmailSetting(ICleanDbContext conn)
        {
            _context = conn;
        }

        public async Task<SuEmailTemplate> GetTemplate(string templateCode)
        {
            return await _context.QueryFirstOrDefaultAsync<SuEmailTemplate>("SELECT subject,content FROM su_email_template WHERE email_template_code = @Code", new { Code = templateCode });
        }
    }
}
