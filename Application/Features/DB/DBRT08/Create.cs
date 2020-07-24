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
            private readonly EmployeeService _es;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user,EmployeeService es)
            {
                _context = context;
                _user = user;
                _es = es;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbEmployee>().Any(o => o.CompanyCode == _user.Company && o.EmployeeCode == request.EmployeeCode))
                {
                    throw new Common.Exceptions.RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT08.employeeCode");
                }
                request.tNameConcat = await _es.GetConcatNameTha(request.PreNameId, request.tFirstName, request.tLastName);
                request.eNameConcat = await _es.GetConcatNameEng(request.PreNameId, request.tFirstName, request.tLastName);

                request.CompanyCode = _user.Company;
                _context.Set<DbEmployee>().Add((DbEmployee)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
