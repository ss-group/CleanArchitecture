using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System.Linq;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT20
{
    public class Create
    {
        public class Command : DbDepartment, ICommand<string>
        {

        }

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbDepartment>().Any(o => o.CompanyCode == _user.Company && o.DepartmentCode == request.DepartmentCode))
                {
                    throw new Common.Exceptions.RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT20.departmentCode");
                }
                request.CompanyCode = _user.Company;
                _context.Set<DbDepartment>().Add((DbDepartment)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.DepartmentCode;
            }
        }
    }
}
