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

namespace Application.Features.DB.DBRT08
{
    public class Detail
    {
        public class Query : IRequest<DbEmployee>
        {
            public String EmployeeCode { get; set; }
        }
        public class Handler : IRequestHandler<Query, DbEmployee>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbEmployee> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Set<DbEmployee>().Where(o => o.EmployeeCode == request.EmployeeCode && o.CompanyCode == _user.Company).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
                return result;
            }
        }
    }
}
