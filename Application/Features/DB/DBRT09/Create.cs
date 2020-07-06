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

namespace Application.Features.DB.DBRT09
{
    public class Create
    {
        public class Command : DbEducationTypeLevel, ICommand
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
                if (_context.Set<DbEducationTypeLevel>().Any(o => o.CompanyCode == _user.Company && o.EducationTypeLevel == request.EducationTypeLevel))
                {
                    throw new Common.Exceptions.RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT09.educationTypeLevel");
                }
                request.CompanyCode = _user.Company;
                _context.Set<DbEducationTypeLevel>().Add((DbEducationTypeLevel)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
