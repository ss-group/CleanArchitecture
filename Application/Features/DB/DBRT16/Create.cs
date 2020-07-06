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

namespace Application.Features.DB.DBRT16
{
    public class Create
    {
        public class Command : DbLocation, ICommand<String>
        {

        }

        public class Handler : IRequestHandler<Command, String>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<String> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbLocation>().Any(o => o.CompanyCode == _user.Company && o.LocationCode == request.LocationCode))
                {
                    throw new Common.Exceptions.RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT16.locationCode");
                }
                request.CompanyCode = _user.Company;
                _context.Set<DbLocation>().Add((DbLocation)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.LocationCode;
            }
        }
    }
}
