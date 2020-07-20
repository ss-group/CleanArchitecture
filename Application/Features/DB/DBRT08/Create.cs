using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class Create
    {
        public class Command : DbEmployee, ICommand
        {

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbEmployee>().Any(o => o.CompanyCode == _user.Company && o.EmployeeCode == request.EmployeeCode))
                {
                    throw new Common.Exceptions.RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT08.employeeCode");
                }
                request.tNameConcat = await this.GetEmployeeConcatTha(request, cancellationToken);
                request.eNameConcat = await this.GetEmployeeConcatEng(request, cancellationToken);
                request.CompanyCode = _user.Company;
                _context.Set<DbEmployee>().Add((DbEmployee)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            private Task<string> GetEmployeeConcatTha(Command request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" select * from db_employee_concat(@Lang,@PreNameId,@FirstName,@LastName) ");
                return _context.ExecuteScalarAsync<string>(sql.ToString(), new { Lang = "th", PreNameId = request.PreNameId, FirstName = request.tFirstName, LastName = request.tLastName }, cancellationToken);
            }

            private Task<string> GetEmployeeConcatEng(Command request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" select * from db_employee_concat(@Lang,@PreNameId,@FirstName,@LastName) ");
                return _context.ExecuteScalarAsync<string>(sql.ToString(), new { Lang = "en", PreNameId = request.PreNameId, FirstName = request.eFirstName, LastName = request.eLastName }, cancellationToken);
            }
        }
    }
}
