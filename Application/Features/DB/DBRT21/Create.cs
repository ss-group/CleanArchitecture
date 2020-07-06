using Application.Common.Behaviors;
using Application.Common.Exceptions;
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

namespace Application.Features.DB.DBRT21
{
    public class Create
    {
        public class Command : DbFacProgram, ICommand<string>
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
                request.CompanyCode = _user.Company;
                if (_context.Set<DbFacProgram>().Any(o => o.CompanyCode == request.CompanyCode & o.FacCode == request.FacCode
                & o.ProgramCode == request.ProgramCode))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT21.DbFacProgram");
                }

                _context.Set<DbFacProgram>().Add((DbFacProgram)request);
                await _context.SaveChangesAsync(cancellationToken);
                return "{companyCode:"+ request.CompanyCode+ ", programCode:" + request.ProgramCode
                    + ", facCode:" + request.FacCode + "}";
            }

        }
    }
}
