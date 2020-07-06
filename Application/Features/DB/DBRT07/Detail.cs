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

namespace Application.Features.DB.DBRT07
{
    public class Detail
    {
        public class Query : IRequest<DbPreName>
        {
            public long PreNameId { get; set; }
        }
        public class Handler : IRequestHandler<Query, DbPreName>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbPreName> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Set<DbPreName>().Where(o => o.PreNameId == request.PreNameId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return result;
            }
        }
    }
}
