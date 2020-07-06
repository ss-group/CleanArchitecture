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
            public int CountryId { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbCountry>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<DbCountry> Handle(Query request, CancellationToken cancellationToken)
            {
                var country = await _context.Set<DbCountry>().Where(o =>  o.CountryId == request.CountryId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (country == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
                return country;
            }
        }
    }
}
