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

namespace Application.Features.DB.DBRT20
{
    public class Detail
    {
        public class Query : IRequest<DbDepartment>
        {
            public string DepartmentCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbDepartment>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbDepartment> Handle(Query request, CancellationToken cancellationToken)
            {
                var subj = await _context.Set<DbDepartment>().Where(o =>  o.DepartmentCode == request.DepartmentCode && o.CompanyCode == _user.Company).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }
        }
    }
}
