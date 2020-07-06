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

namespace Application.Features.DB.DBRT01
{
    public class Detail
    {
        public class Query : IRequest<DbCountry>
        {
            public string CountryCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbCountry>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbCountry> Handle(Query request, CancellationToken cancellationToken)
            {
                var subj = await _context.Set<DbCountry>().Where(o =>  o.CountryCode == request.CountryCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
                return subj;
            }
        }
    }
}
