using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT16
{
    public class Detail
    {
        public class Query : IRequest<DbLocation>
        {
            public string LocationCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbLocation>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _user = user;
                _context = context;
            }
            public async Task<DbLocation> Handle(Query request, CancellationToken cancellationToken)
            {
                var subj = await _context.Set<DbLocation>().Where(o =>  o.LocationCode == request.LocationCode && o.CompanyCode == _user.Company).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }
        }
    }
}
