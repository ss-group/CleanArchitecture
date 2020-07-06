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

namespace Application.Features.DB.DBRT10
{
    public class Detail
    {
        public class Query : IRequest<DbEducationType>
        {
            public string CompanyCode { get; set; }
            public string EducationTypeCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbEducationType>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbEducationType> Handle(Query request, CancellationToken cancellationToken)
                {
                request.CompanyCode = _user.Company;
                var result = await _context.Set<DbEducationType>().Where(o => o.CompanyCode == request.CompanyCode & o.EducationTypeCode == request.EducationTypeCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
                return result;
            }


        }
    }
}
