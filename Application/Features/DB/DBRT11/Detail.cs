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

namespace Application.Features.DB.DBRT11
{
    public class Detail
    {
        public class Query : IRequest<DbFac>
        {
            public string CompanyCode { get; set; }
            public string FacCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbFac>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbFac> Handle(Query request, CancellationToken cancellationToken)
            {
                var subj = await _context.Set<DbFac>().Where(o => o.CompanyCode == _user.Company && o.FacCode == request.FacCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }

 
        }
    }
}
