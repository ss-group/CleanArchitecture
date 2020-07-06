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

namespace Application.Features.DB.DBRT17
{
    public class Create
    {
        public class Command : DbBuilding, ICommand<long>
        {

        }
        public class Handler : IRequestHandler<Command, long>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_context.Set<DbBuilding>().Any(o => o.CompanyCode == _user.Company && o.BuildingId == request.BuildingId))
                {
                    throw new Common.Exceptions.RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT17.buildingCode");
                }
                request.CompanyCode = _user.Company;
                _context.Set<DbBuilding>().Add((DbBuilding)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.BuildingId;
            }
        }
    }
}
