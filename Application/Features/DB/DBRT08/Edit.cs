using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class Edit
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
                request.tNameConcat = await this.GetEmployeeConcatTha(request, cancellationToken);
                request.eNameConcat = await this.GetEmployeeConcatEng(request, cancellationToken);
                _context.Set<DbEmployee>().Attach((DbEmployee)request);
                _context.Entry((DbEmployee)request).State = EntityState.Modified;
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
