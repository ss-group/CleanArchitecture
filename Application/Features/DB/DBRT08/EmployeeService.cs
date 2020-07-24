using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class EmployeeService : IService
    {
        private readonly ICleanDbContext _context;
        private readonly ICurrentUserAccessor _user;
        protected virtual CancellationToken CancellationToken => default;
        public EmployeeService(ICleanDbContext context,ICurrentUserAccessor user)
        {
            _context = context;
            _user = user;
        }

        public async Task<string> GetConcatNameTha(int? preNameId,string firstName,string lastName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" select * from db_employee_concat(@Lang,@PreNameId,@FirstName,@LastName) ");
            return await _context.ExecuteScalarAsync<string>(sql.ToString(), new { Lang = "th", PreNameId = preNameId, FirstName = firstName, LastName = lastName }, CancellationToken);
        }

        public async Task<string> GetConcatNameEng(int? preNameId, string firstName, string lastName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" select * from db_employee_concat(@Lang,@PreNameId,@FirstName,@LastName) ");
            return await _context.ExecuteScalarAsync<string>(sql.ToString(), new { Lang = "en", PreNameId = preNameId, FirstName = firstName, LastName = lastName }, CancellationToken);
        }
    }
}
