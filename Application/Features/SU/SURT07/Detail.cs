using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT07
{
    public class Detail
    {
        public class Query : IRequest<SuPasswordPolicy>
        {
            public string PasswordPolicyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuPasswordPolicy>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<SuPasswordPolicy> Handle(Query request, CancellationToken cancellationToken)
            {
                SuPasswordPolicy passwordPolicy = await _context.Set<SuPasswordPolicy>().Where(i => i.PasswordPolicyCode == request.PasswordPolicyCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                
                if (passwordPolicy == null)
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");

                return passwordPolicy;
            }
        }
    }
}
