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

namespace Application.Features.DB.DBRT14
{
    public class Detail
    {
        public class Query : IRequest<DbProject>
        {
            public String ProjectCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbProject>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbProject> Handle(Query request, CancellationToken cancellationToken)
            {
                var subj = await _context.Set<DbProject>().Where(o =>  o.ProjectCode == request.ProjectCode && o.CompanyCode == _user.Company).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
                return subj;
            }
        }
    }
}
